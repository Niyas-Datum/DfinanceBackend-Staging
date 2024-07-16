using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [ApiController]
    [Authorize]
    public class AccountRegisterController : BaseController
    {
        private readonly ICustomerRegister _CustomerRegisterService;
        private IAccountRegister _AccountRegisterService;

        public AccountRegisterController(IAccountRegister AccountRegisterService)
        {
            _AccountRegisterService = AccountRegisterService;
        }
        [HttpGet(FinRoute.AccountRegister.FillAccreg)]
        public IActionResult FillAccountRegister( int pageId)
        {
            try
            {
                var view = _AccountRegisterService.FillAccountRegister(pageId);

                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
