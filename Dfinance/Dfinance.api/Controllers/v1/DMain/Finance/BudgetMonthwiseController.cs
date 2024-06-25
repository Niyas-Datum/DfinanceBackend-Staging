using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Finance.Services;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [Authorize]
    [ApiController]
    public class BudgetMonthwiseController : BaseController
    {
        private readonly IBudgetMonthwiseService _budgetMonthwiseService;
        public BudgetMonthwiseController(IBudgetMonthwiseService budgetMonthwiseService)
        {            
            _budgetMonthwiseService = budgetMonthwiseService;
        }
        [HttpGet(FinRoute.BudMonthwise.fill)]
        public IActionResult FillBudgetMonthwise(int pageId,string criteria)
        {
            try
            {
                var data = _budgetMonthwiseService.FillBudgetMonthwise(pageId,criteria);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
