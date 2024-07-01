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
    public class BudgetMonthwiseService:IBudgetMonthwiseService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<BudgetMonthwiseService> _logger;
        public BudgetMonthwiseService(DFCoreContext context, IAuthService authService, ILogger<BudgetMonthwiseService> logger)
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
        public CommonResponse FillBudgetMonthwise(int pageId, string criteria)
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
                object budgetReport = null;
                if (criteria == "ProfitAndLoss")
                {
                    criteria = "MonthWisePandL";
                    budgetReport = _context.MonthwisePandLView.FromSqlRaw("Exec BudgetingNewSP @Criteria={0}", criteria).ToList();
                }                   
                else if (criteria == "BalanceSheet")
                {
                  criteria = "MonthWiseBalanceSheet";
                  budgetReport = _context.MonthwiseBalSheetView.FromSqlRaw("Exec BudgetingNewSP @Criteria={0}", criteria).ToList();
                }
                    
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
