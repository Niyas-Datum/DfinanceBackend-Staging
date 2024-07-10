using AutoMapper;
using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dfinance.Finance.Vouchers
{
    public class OpeningVoucherService : IOpeningVoucherService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<OpeningVoucherService> _logger;
        private readonly IFinancePaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly IFinanceTransactionService _transactionService;
        private readonly IUserTrackService _userTrackService;
        public OpeningVoucherService(DFCoreContext context, IAuthService authService, ILogger<OpeningVoucherService> logger, IMapper mapper, IFinancePaymentService paymentService, IFinanceTransactionService transactionService, IUserTrackService userTrackService)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _paymentService = paymentService;
            _transactionService = transactionService;
            _userTrackService = userTrackService;

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
        public CommonResponse SaveOpeningVoucher(OpeningVoucherDto openVouDto,int PageId,int VoucherId)
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
                        return PermissionDenied("Save OpeningVoucher");
                    }
                    string Status = "Approved";

                    var openingVouDto = _mapper.Map<OpeningVoucherDto, FinanceTransactionDto>(openVouDto);

                    int TransId = (int)_transactionService.SaveTransaction(openingVouDto, PageId, VoucherId, Status).Data;
                    if (openingVouDto != null)
                    {
                        int TransEntId = (int)_paymentService.SaveTransactionEntries(openingVouDto, PageId, TransId, TransId).Data;
                    }
                    if (openingVouDto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }

                    string? reason = "Added";
                    int actionId = 0;
                    int transId = TransId;
                    decimal? amount = null;
                    foreach (var accountDetail in openVouDto.AccountDetails)
                    {
                        if (accountDetail.Debit.HasValue && accountDetail.Debit != 0)
                        {
                             amount = accountDetail.Debit.Value;
                        }
                       
                    }
                    var jsonOpenvou = JsonSerializer.Serialize(openVouDto);
                    _userTrackService.AddUserActivity(openVouDto.VoucherNo, transId, actionId, reason, "FiTransactions", "Opening Balance", amount ?? 0, jsonOpenvou);

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

        public CommonResponse UpdateOpeningVoucher(OpeningVoucherDto openVouDto, int PageId, int VoucherId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!_authService.IsPageValid(PageId))
                    {
                        return PageNotValid(PageId);
                    }
                    if (!_authService.UserPermCheck(PageId, 3))
                    {
                        return PermissionDenied("Save OpeningVoucher");
                    }
                    string Status = "Approved";

                    var openingVouDto = _mapper.Map<OpeningVoucherDto, FinanceTransactionDto>(openVouDto);

                    int TransId = (int)_transactionService.SaveTransaction(openingVouDto, PageId, VoucherId, Status).Data;
                    if (openingVouDto != null)
                    {
                        int TransEntId = (int)_paymentService.UpdateTransactionEntries(openingVouDto, PageId, TransId, TransId).Data;

                    }
                    if (openingVouDto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    string? reason = "";
                    int actionId = 1;
                    int transId = TransId;
                    decimal? amount = null;
                    foreach (var accountDetail in openVouDto.AccountDetails)
                    {
                        if (accountDetail.Debit.HasValue && accountDetail.Debit != 0)
                        {
                            amount = accountDetail.Debit.Value;
                        }
                       
                    }
                    var jsonOpen = JsonSerializer.Serialize(openVouDto);
                    _userTrackService.AddUserActivity(openVouDto.VoucherNo, transId, actionId, reason, "FiTransactions", "Opening Balance", amount ?? 0, jsonOpen);
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

        public CommonResponse DeleteOpeningVou(OpeningVoucherDto openVouDto, int pageId)
        {
            try
            {
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 5))
                {
                    return PermissionDenied("Delete Opening Voucher");
                }
                var transid = _context.FiTransaction.Any(x => x.Id == openVouDto.Id);
                if (!transid)
                {
                    return CommonResponse.NotFound("Transaction Not Found");


                }
                string criteria = "DeleteTransactions";

                _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @ID={1}",
                                       criteria, openVouDto.Id);
                string? reason = "delete";
                int actionId = 2;
            //    int transId = openVouDto.Id;
                decimal? amount = 0;
               
                var jsonOpen = JsonSerializer.Serialize(openVouDto);
                _userTrackService.AddUserActivity(openVouDto.VoucherNo, openVouDto.Id ?? 0, actionId, reason, "FiTransactions", "Opening Balance", 0 , jsonOpen);

                _logger.LogInformation("Opening Voucher deleted successfully");
                return CommonResponse.Ok("Opening Voucher deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }










    }
}
