using AutoMapper;
using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Vouchers
{
    public class ReceiptVoucherService : IReceiptVoucherService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<PaymentVoucherService> _logger;
        private readonly IFinanceTransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly ISettingsService _settings;
        private readonly IFinanceAdditional _additionalService;
        private readonly IFinancePaymentService _paymentService;
        private readonly IUserTrackService _userTrackService;
        private readonly IPaymentVoucherService _paymentVoucherService;
        public ReceiptVoucherService(DFCoreContext context, IAuthService authService, ILogger<PaymentVoucherService> logger, IFinanceTransactionService transactionService, IMapper mapper, ISettingsService settings,
                                      IFinanceAdditional additionalService, IFinancePaymentService paymentService, IUserTrackService userTrackService, IPaymentVoucherService paymentVoucherService)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
            _transactionService = transactionService;
            _mapper = mapper;
            _settings = settings;
            _additionalService = additionalService;
            _paymentService = paymentService;
            _userTrackService = userTrackService;
           _paymentVoucherService = paymentVoucherService;
        }

        public CommonResponse FillMaVoucher(int? VoucherId,int? PageId)
        {
            try
            {
                if (PageId == null)
                {
                    var result = _context.VoucherView
                        .FromSqlRaw($"EXEC LeftGridMasterSP @Criteria ='FillMaVouchers', @Id = '{VoucherId}'").AsEnumerable()
                        .FirstOrDefault();
                    return CommonResponse.Ok(result);
                }
                else
                {
                    var result = _context.VoucherView
                        .FromSqlRaw($"EXEC LeftGridMasterSP @Criteria ='FillMaVouchersUsingPageID', @Id = '{PageId}'").AsEnumerable()
                        .FirstOrDefault();
                    return CommonResponse.Ok(result);
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse FillMaster(int? TransId,int? PageId)
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;
                int createdBy = _authService.GetId().Value;
                var key = "LeftGridStatus";
                var value = _context.MaSettings.Where(i => i.Key == key)
                       .Select(i => i.Value)
                       .SingleOrDefault();

                if (TransId == null)
                {
                    if (value == "true")
                    {
                        var result = _context.FillVoucher
                       .FromSqlRaw($"EXEC LeftGridMasterSP @Criteria ='FillVoucher', @BranchID = '{branchId}',@MaPageMenuID='{PageId}',@UserID='{createdBy}'").AsEnumerable()
                       .FirstOrDefault();
                        return CommonResponse.Ok(result);
                    }
                    else
                    {
                        var result = _context.FillVoucher
                       .FromSqlRaw($"EXEC LeftGridMasterSP @Criteria ='FillVoucher', @BranchID = '{branchId}',@MaPageMenuID='{PageId}'").AsEnumerable()
                       .FirstOrDefault();
                        return CommonResponse.Ok(result);
                    }

                }
                else 
                {
                    if (value == "true")
                    {
                        var result = _context.FillVouTranId
                       .FromSqlRaw($"EXEC VoucherSP @Criteria ='FillVoucherByTransactionID', @TransactionID = '{TransId}',@userID='{createdBy}'").AsEnumerable()
                       .FirstOrDefault();
                        return CommonResponse.Ok(result);
                    }
                    else
                    {
                        var result = _context.FillVouTranId
                       .FromSqlRaw($"EXEC VoucherSP @Criteria ='FillVoucherByTransactionID', @TransactionID = '{TransId}'").AsEnumerable()
                       .FirstOrDefault();
                        return CommonResponse.Ok(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        private CommonResponse PermissionDenied(string msg)
        {
            _logger.LogInformation("No Permission for " + msg);
            return CommonResponse.Error("No Permission ");
        }
        private CommonResponse PageNotValid(int pageId)
        {
            _logger.LogInformation("Page not Exists :" + pageId);
            return CommonResponse.Error("Page not Exists");
        }
        public CommonResponse SaveReceiptVou(FinanceTransactionDto receiptVoucherDto, int PageId, int voucherId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!_authService.IsPageValid(PageId))
                    {
                        return PageNotValid(PageId);
                    }
                    if (!_authService.UserPermCheck(PageId, 2))
                    {
                        return PermissionDenied("Save ReceiptVoucher");
                    }
                    string Status = "Approved";


                    var normalAmount = Convert.ToDecimal(receiptVoucherDto.Cash.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(receiptVoucherDto.Cheque?.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(receiptVoucherDto.Card?.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(receiptVoucherDto.Epay?.Sum(x => x.Amount) ?? 0);
                    var sumOfDebit = receiptVoucherDto.AccountDetails.Sum(accountDetail => accountDetail.Amount);


                    if (normalAmount != sumOfDebit)
                    {
                        return CommonResponse.Error("Sum of Debit and Credit entry must be equal!!");
                    }

                    //save
                    int TransId = (int)_transactionService.SaveTransaction(receiptVoucherDto, PageId, voucherId, Status).Data;

                    if (receiptVoucherDto != null)
                    {
                        _additionalService.SaveTransactionAdditional(receiptVoucherDto, TransId, voucherId);
                    }

                    if (receiptVoucherDto != null)
                    {

                        int TransEntId = (int)_paymentService.SaveTransactionEntries(receiptVoucherDto, PageId, TransId, TransId).Data;

                        if (receiptVoucherDto.BillandRef != null && receiptVoucherDto.BillandRef.Any(a => a.VID != null || a.VID != 0))
                        {
                            _transactionService.SaveVoucherAllocation(TransId, TransId, receiptVoucherDto);
                        }
                    }
                    if (receiptVoucherDto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    _logger.LogInformation("Successfully Created");
                    transaction.Commit();
                    return CommonResponse.Created("Created Successfully");


                }

                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transaction.Rollback();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }

        public CommonResponse UpdateReceiptVoucher(FinanceTransactionDto receiptVoucherDto, int PageId, int voucherId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!_authService.IsPageValid(PageId))
                    {
                        return PageNotValid(PageId);
                    }
                    if (!_authService.UserPermCheck(PageId, 2))
                    {
                        return PermissionDenied("Save PaymentVoucher");
                    }

                    string Status = "Approved";
                    var normalAmount = Convert.ToDecimal(receiptVoucherDto.Cash.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(receiptVoucherDto.Cheque?.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(receiptVoucherDto.Card?.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(receiptVoucherDto.Epay?.Sum(x => x.Amount) ?? 0);
                    var sumOfDebit = receiptVoucherDto.AccountDetails.Sum(accountDetail => accountDetail.Amount);
                    if (normalAmount != sumOfDebit)
                    {
                        return CommonResponse.Error("Sum of Debit and Credit entry must be equal!!");
                    }
                    int TransId = (int)_transactionService.SaveTransaction(receiptVoucherDto, PageId, voucherId, Status).Data;

                    if (receiptVoucherDto != null)
                    {
                        _additionalService.SaveTransactionAdditional(receiptVoucherDto, TransId, voucherId);
                    }
                    if (receiptVoucherDto != null)
                    {

                        int TransEntId = (int)_paymentService.SaveTransactionEntries(receiptVoucherDto, PageId, TransId, TransId).Data;

                        if (receiptVoucherDto.BillandRef != null && receiptVoucherDto.BillandRef.Any(a => a.VID != null || a.VID != 0))
                        {
                            _transactionService.UpdateVoucherAllocation(TransId, TransId, receiptVoucherDto);
                        }
                    }
                    if (receiptVoucherDto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    _logger.LogInformation("Successfully Updated");
                    transaction.Commit();
                    return CommonResponse.Created("Updated Successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transaction.Rollback();
                    return CommonResponse.Error(ex.Message);
                }
            }

        }

        public CommonResponse DeleteReceiptVoucher(int TransId, int pageId)
        {
            try
            {
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 3))
                {
                    return PermissionDenied("Delete Receipt Voucher");
                }
                var transid = _context.FiTransaction.Any(x => x.Id == TransId);
                if (!transid)
                {
                    return CommonResponse.NotFound("Transaction Not Found");


                }
                string criteria = "DeleteTransactions";

                _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @ID={1}",
                                       criteria, TransId);

                _logger.LogInformation("Delete successfully");
                return CommonResponse.Ok("Delete successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }



    }
}
