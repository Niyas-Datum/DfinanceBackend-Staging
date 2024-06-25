using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;


namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [Authorize]
    [ApiController]
    public class BudgetRegisterController : BaseController
    {
        private readonly IBudgetRegisterService _budgetRegisterService;
        public BudgetRegisterController(IBudgetRegisterService budgetRegisterService)
        {
            _budgetRegisterService = budgetRegisterService;
        }
        [HttpGet(FinRoute.BudReg.budPopup)]
        public IActionResult FillBudgetReg(int voucherId)
        {
            try
            {
                var data = _budgetRegisterService.FillBudgetPopup(voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(FinRoute.BudReg.fill)]
        public IActionResult FillBudgetReg(int pageId, string criteria)
        {
            try
            {
                var data = _budgetRegisterService.FillBudgetReport(pageId,criteria);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
