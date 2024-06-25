using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [ApiController]
    [Authorize]
    public class BudgetingController : BaseController
    {
        private readonly IBudgetingService _budgeting;
        public BudgetingController(IBudgetingService budgeting)
        {
            _budgeting = budgeting;
        }

        [HttpGet(FinRoute.Budgeting.fillMaster)]
        public IActionResult FillMaster(int? TransId,int? PageId, int? voucherId)
        {
            try
            {
                var data = _budgeting.FillMaster(TransId,PageId,voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }        
        
        [HttpGet(FinRoute.Budgeting.plBalsheet)]
        public IActionResult FillProfitLossBalsheet(bool? profitLoss,bool? balSheet)
        {
            try
            {
                var data = _budgeting.FillProfitLossBalsheet(profitLoss,balSheet);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(FinRoute.Budgeting.Save)]
        public IActionResult SaveBudget(BudgetingDto budgetDto, int pageId, int voucherId)
        {
            try
            {
                var data = _budgeting.SaveBudget(budgetDto,pageId,voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(FinRoute.Budgeting.Delete)]
        public IActionResult Delete(BudgetingDto budgetDto, int pageId, int voucherId, bool? cancel = false)
        {
            try
            {
                var data = _budgeting.DeleteBudget(budgetDto, pageId, voucherId,cancel);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
