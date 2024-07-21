using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Finance.Services;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics.Eventing.Reader;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [ApiController]
    [Authorize]
    public class CustomerRegisterController : BaseController
    {
        private readonly ICustomerRegister _CustomerRegisterService;
        public CustomerRegisterController(ICustomerRegister CustomerRegisterService)
        {
            _CustomerRegisterService = CustomerRegisterService;
        }
        
        [HttpGet(FinRoute.CustomerRegister.FillCusreg)]
        public IActionResult FillCustomerRegister(string PartyType, int pageId, int? AccountID  , int? PartyCategory )
        {
            try
            {
                var view = _CustomerRegisterService.FillCustomerRegister(PartyType, AccountID, PartyCategory,pageId);

                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
