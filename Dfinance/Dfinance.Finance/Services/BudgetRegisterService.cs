using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Services
{
    public class BudgetRegisterService : IBudgetRegisterService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<BudgetRegisterService> _logger;
        public BudgetRegisterService(DFCoreContext context, IAuthService authService, ILogger<BudgetRegisterService> logger)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
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
        public CommonResponse FillBudgetPopup(int voucherId)
        {
            try
            {
                var result = _context.FiTransaction
                     .Where(ft => ft.Active == true && ft.VoucherId == 70)
                     .Select(ft => new
                     {
                         ft.Id,
                         ft.Description,
                         ft.TransactionNo,
                         ft.Date,
                         ft.ApprovedDate,
                         ft.InstrumentDate,
                         ft.CommonNarration
                     })
                     .ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error();
            }
        }
        public CommonResponse FillBudgetReport(int pageId,string criteria)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 1))
            {
                return PermissionDenied("Fill BudgetRegister");
            }
            try
            {
                object budgetReport=null;
                if (criteria== "ProfitAndLoss")               
                    budgetReport = _context.BudRegProfitAndLoss.FromSqlRaw("Exec BudgetingNewSP @Criteria={0}", criteria).ToList();
                else if(criteria == "BalanceSheet")
                    budgetReport = _context.BalanceSheetView.FromSqlRaw("Exec BudgetingNewSP @Criteria={0}", criteria).ToList();
                return CommonResponse.Ok(budgetReport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error();
            }
        }
    }
}
