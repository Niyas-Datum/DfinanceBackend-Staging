using AutoMapper;
using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Finance.Services.Interface;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Vouchers
{
    public class CloseVoucherService : ICloseVoucherService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<CloseVoucherService> _logger;
        private readonly IFinanceTransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly ISettingsService _settings;
        private readonly IFinanceAdditional _additionalService;
        private readonly IFinancePaymentService _paymentService;
        private readonly IHostEnvironment _environment;
        public CloseVoucherService(DFCoreContext context, IAuthService authService, ILogger<CloseVoucherService> logger, IFinanceTransactionService transactionService, IMapper mapper, ISettingsService settings,
                                      IFinanceAdditional additionalService, IFinancePaymentService paymentService,IHostEnvironment environment)
        {

            _authService = authService;
            _context = context;
            _logger = logger;
            _transactionService = transactionService;
            _mapper = mapper;
            _settings = settings;
            _additionalService = additionalService;
            _paymentService = paymentService;
            _environment = environment;
        }
        private CommonResponse GetAccounts()
        {
            int? branchId = _authService.GetBranchId().Value;
            var accounts = (from A in _context.FiMaAccounts
                            join B in _context.FiMaBranchAccounts on A.Id equals B.AccountId
                            where A.IsGroup == false && B.BranchId == branchId
                            select new
                            {
                                AccountCode = A.Alias,
                                AccountName = A.Name,
                                A.Id
                            });
            return CommonResponse.Ok(accounts);
        }
        private CommonResponse GetVouchers()
        {
            var vouchers=_context.FiMaVouchers.Where(v=>v.Active==true).Select(v=>new { v.Id,v.Name}).ToList();
            return CommonResponse.Ok(vouchers);
        }
        public CommonResponse GetLoadData()
        {
            try
            {
                var accounts = GetAccounts().Data;
                var vouchers=GetVouchers().Data;
                return CommonResponse.Ok(new { Accounts = accounts,Vouchers=vouchers });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse Fill(CloseVoucherDto closeVoucher)
        {
            try
            {
                var cmd = _context.Database.GetDbConnection().CreateCommand();
                cmd.CommandType = CommandType.Text;
                var accountId = closeVoucher.AccountId.Id != null ? closeVoucher.AccountId.Id.ToString() : "NULL";
                var vType = closeVoucher.VTypeId.Id != null ? closeVoucher.VTypeId.Id.ToString() : "NULL";
                string startDate = closeVoucher.DateFrom.HasValue ? $"'{closeVoucher.DateFrom.Value.ToString("yyyy-MM-dd")}'" : "NULL";
                string endDate = closeVoucher.DateUpto.HasValue ? $"'{closeVoucher.DateUpto.Value.ToString("yyyy-MM-dd")}'" : "NULL";
                var vno = closeVoucher.VNo != null ? $"'{closeVoucher.VNo}'" : "NULL";
                var branchId = _authService.GetBranchId().Value;
                cmd.CommandText = $"Exec CloseVoucherSP @Criteria='Fill',@BranchID={branchId},@AccountID={accountId},@VTypeID={vType},@DateFrom={startDate},@DateUpto={endDate},@TransactionNo={vno}";               
                _context.Database.GetDbConnection().Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var tb = new DataTable();
                    tb.Load(reader);
                    if (tb.Rows.Count > 0)
                    {
                        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                        Dictionary<string, object> row;
                        foreach (DataRow dr in tb.Rows)
                        {
                            row = new Dictionary<string, object>();
                            foreach (DataColumn col in tb.Columns)
                            {
                                row.Add(col.ColumnName.Replace(" ", ""), dr[col].ToString().Trim());
                            }
                            rows.Add(row);
                        }
                        return CommonResponse.Ok(rows);
                    }
                    return CommonResponse.NoContent();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
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

        //UserTrack added in sp
        public CommonResponse CloseVoucherUpdate(int PageId, List<int> Ids)
        {
            try
            {
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 2))
                {
                    return PermissionDenied("CloseVoucher");
                }
                int branchId = _authService.GetBranchId().Value;
                int createdBy = _authService.GetId().Value;
                var machineName = _environment.EnvironmentName;
                var criteria = "Update";
                var reason = "Reason to close the selected voucher(s)";
                foreach (var id in Ids)
                {
                    _context.Database.ExecuteSqlRaw("EXEC CloseVoucherSP " +
                       "@Criteria={0}, @BranchID={1},@UserID={2},@Reason={3},@MachineName={4},@ID={5}",
                       criteria, branchId, createdBy, reason, machineName, id);
                }
                _logger.LogInformation("Sucessfully Applied");
                return CommonResponse.Ok("Sucessfully Applied");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
    }
}
