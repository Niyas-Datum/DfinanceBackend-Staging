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
        [HttpGet(FinRoute.AccountRegister.PopupAccgr)]
        public IActionResult AccountGroupPopup()
        {
            try
            {
                var view = _AccountRegisterService.AccountGroupPopup();

                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(FinRoute.AccountRegister.PopupSubgr)]
        public IActionResult SubGroupPopup()
        {
            try
            {
                var view = _AccountRegisterService.SubGroupPopup();

                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(FinRoute.AccountRegister.PopupPar)]
        public IActionResult ParentPopup()
        {
            try
            {
                var view = _AccountRegisterService.ParentPopup();

                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
