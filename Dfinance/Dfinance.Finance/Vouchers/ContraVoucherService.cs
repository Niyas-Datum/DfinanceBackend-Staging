using AutoMapper;
using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dfinance.Finance.Vouchers
{
    public class ContraVoucherService : IContraVoucherService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<ContraVoucherService> _logger;
        private readonly IFinanceTransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly ISettingsService _settings;
        private readonly IFinanceAdditional _additionalService;
        private readonly IFinancePaymentService _paymentService;
        private readonly IUserTrackService _userTrackService;

        public ContraVoucherService(DFCoreContext context, IAuthService authService, ILogger<ContraVoucherService> logger, IFinanceTransactionService transactionService, IMapper mapper, ISettingsService settings,
                                      IFinanceAdditional additionalService, IFinancePaymentService paymentService, IUserTrackService userTrackService)
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
        public CommonResponse FillAccCode()
        {
            try
            {
                int? branchId = _authService.GetBranchId();
                string criteria = "AccountsContra";

                object PrimaryVoucherID = null, ItemID = null, ModeID = null, TransactionID = null, partyId = null, locId = null, voucherId = null, PageID = null;
                bool IsSizeItem = false, IsMargin = false, ISTransitLoc = false, IsFinishedGood = false, IsRawMaterial = false;

                DateTime? VoucherDate = null;
                int? userId = _authService.GetId();
                var result = _context.CommandTextView.FromSqlRaw($"select dbo.GetCommandText('{criteria}','{PrimaryVoucherID}','{branchId}','{partyId}','{locId}','{IsSizeItem}','{IsMargin}','{voucherId}','{ItemID}','{ISTransitLoc}','{IsFinishedGood}','{IsRawMaterial}','{ModeID}','{PageID}','{VoucherDate}','{TransactionID}','{userId}')")
                             .ToList();

                var res = result.FirstOrDefault();

                var data = _context.ContraAccCode.FromSqlRaw(res.commandText).ToList();

                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        
        public CommonResponse SaveContraVou(ContraDto contraDto, int PageId, int voucherId)
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
                        return PermissionDenied("Save ContraVoucher");
                    }
                    string Status = "Approved";

                    var contraVouDto = _mapper.Map<ContraDto, FinanceTransactionDto>(contraDto);

                    var TotalDebit = Convert.ToDecimal(contraVouDto.AccountDetails.Sum(x => x.Debit) ?? 0);
                    var TotalCredit = Convert.ToDecimal(contraVouDto.AccountDetails.Sum(x => x.Credit) ?? 0);
                    if (TotalDebit != TotalCredit) { return CommonResponse.Error("Debit and Credit must be tally!!"); }


                    int TransId = (int)_transactionService.SaveTransaction(contraVouDto, PageId, voucherId, Status).Data;
                    if (contraVouDto != null)
                    {
                        int TransEntId = (int)_paymentService.SaveTransactionEntries(contraVouDto, PageId, TransId, TransId).Data;
                    }
                    if (contraVouDto != null)
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

        public CommonResponse UpdateContraVou(ContraDto contraDto, int PageId, int voucherId)
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
                        return PermissionDenied("Save ContraVoucher");
                    }
                    string Status = "Approved";

                    var contraVouDto = _mapper.Map<ContraDto, FinanceTransactionDto>(contraDto);

                    var TotalDebit = Convert.ToDecimal(contraVouDto.AccountDetails.Sum(x => x.Debit) ?? 0);
                    var TotalCredit = Convert.ToDecimal(contraVouDto.AccountDetails.Sum(x => x.Credit) ?? 0);
                    if (TotalDebit != TotalCredit) { return CommonResponse.Error("Debit and Credit must be tally!!"); }


                    int TransId = (int)_transactionService.SaveTransaction(contraVouDto, PageId, voucherId, Status).Data;
                    if (contraVouDto != null)
                    {
                        int TransEntId = (int)_paymentService.UpdateTransactionEntries(contraVouDto, PageId, TransId, TransId).Data;
                    }
                    if (contraVouDto != null)
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

        public CommonResponse DeleteContraVou(int TransId, int pageId)
        {
            try
            {
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 3))
                {
                    return PermissionDenied("Delete Contra Voucher");
                }
                var transid = _context.FiTransaction.Any(x => x.Id == TransId);
                if (!transid)
                {
                    return CommonResponse.NotFound("Transaction Not Found");


                }
                string criteria = "DeleteTransactions";

                _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @ID={1}",
                                       criteria, TransId);

                _logger.LogInformation("Contra Voucher deleted successfully");
                return CommonResponse.Ok("Contra Voucher deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }




    }
}
