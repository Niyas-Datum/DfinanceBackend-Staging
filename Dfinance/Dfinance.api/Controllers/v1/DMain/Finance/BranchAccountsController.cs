using Dfinance.api.Authorization;
using Dfinance.Application.Services.Finance;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [ApiController]
    [Authorize]

    public class BranchAccountsController : Controller
    {
        private readonly IBranchAccounts _BranchAccountsService;
        public BranchAccountsController(IBranchAccounts branchAccountsService)
        {
            _BranchAccountsService = branchAccountsService;
        }

        [HttpGet(FinRoute.BranchAccounts.FillbrAcc)]
        public IActionResult FillBranchAccounts()
        {
            try
            {
                var view = _BranchAccountsService.FillBranchAccounts();
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(FinRoute.BranchAccounts.update)]
        public IActionResult UpdateBranchAccounts([FromBody] BranchAccountsDto branchAccountsDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _BranchAccountsService.UpdateBranchAccounts(branchAccountsDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
