using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.General;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Dfinance.Application.Services.General
{
    public class RecallVoucherService : IRecallVoucherService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<RecallVoucherService> _logger;
        private readonly IHostEnvironment _environment;
        public RecallVoucherService(DFCoreContext context, IAuthService authService, ILogger<RecallVoucherService> logger, IHostEnvironment environment)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
            _environment = environment;
        }
        public CommonResponse GetData()
        {
            try
            {
                int branchid = _authService.GetBranchId().Value;
                var account = (from ac in _context.FiMaAccounts
                               join ba in _context.FiMaBranchAccounts on ac.Id equals ba.AccountId
                               where ac.IsGroup == false && ba.BranchId == branchid
                               select new
                               {
                                   AccountCode = ac.Alias,
                                   AccountName = ac.Name,
                                   Id = ac.Id
                               }).ToList();
                var voucherType = _context.FiMaVouchers.Where(v => v.Active == true).ToList();
                return CommonResponse.Ok(new { account, voucherType });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse FillCancelledVouchers(int? accountId, int? vTypeId, DateTime? dateFrom, DateTime? dateUpTo, string? transactionNo)
        {
            try
            {
                var account = accountId.ToString();
                if(accountId == null) 
                    account= "NULL ";
                var vType=vTypeId.ToString();
                if (vTypeId == null)
                    vType = "NULL ";
                var dateFrom1 = dateFrom.ToString();
                if (dateFrom == null)
                    dateFrom1 = "NULL ";
                var dateUpTo1 = dateUpTo.ToString();
                if (dateUpTo == null)
                    dateUpTo1 = "NULL ";
                var transaction = transactionNo;
                if (transactionNo == null)
                    transaction = "NULL ";

                int branchid = _authService.GetBranchId().Value;
                var data = _context.RecallVoucherViews.FromSqlRaw($"Exec RecallVoucherSP @Criteria='Fill',@BranchID={branchid},@AccountID={account}, @VTypeID={vType}, @DateFrom = {dateFrom1}, @DateUpto = {dateUpTo1}, @TransactionNo={transaction}").ToList();
                _logger.LogInformation("FillCancelledVouchers successfully");
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        private CommonResponse PermissionDenied(string msg)
        {
            _logger.LogInformation(msg);
            return CommonResponse.Error("No Permission ");
        }
        private CommonResponse PageNotValid(int pageId)
        {
            _logger.LogInformation("Page not Exists :" + pageId);
            return CommonResponse.Error("Page not Exists");
        }
        public CommonResponse ApplyUpdateVoucher(string? Reason,int[] voucherID)
        {
            try
            {
                //if (!_authService.IsPageValid(PageId))
                //{
                //    return PageNotValid(PageId);
                //}
                //if (!_authService.UserPermCheck(PageId, 2))
                //{
                //    return PermissionDenied("Sorry, you do not have permission!!");
                //}

                int branchid = _authService.GetBranchId().Value;
                var machineName = _environment.EnvironmentName;
                var userId=_authService.GetId();                
                foreach (var voucher in voucherID)
                {
                     _context.Database.ExecuteSqlRaw($"Exec RecallVoucherSP @Criteria='Update',@BranchID={branchid},@UserID ={userId}, @Reason ={Reason}, @MachineName ={machineName}, @ID ={voucher}");
                }
                _logger.LogInformation("FillCancelledVouchers successfully");
                return CommonResponse.Ok("Update Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }

        }
    }
}
