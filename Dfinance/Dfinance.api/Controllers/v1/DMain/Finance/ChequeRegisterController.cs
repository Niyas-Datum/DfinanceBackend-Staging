using Dfinance.api.Framework;
using Dfinance.Application.Services.Finance;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [ApiController]
    [Authorize]

    public class ChequeRegisterController : BaseController
    {

        private readonly IChequeRegister _ChequeRegisterService;
        
        public ChequeRegisterController(IChequeRegister ChequeRegisterService)
        {
            _ChequeRegisterService = ChequeRegisterService;
        }
                 
        [HttpGet(FinRoute.ChequeRegister.FillCheqreg)]
        public IActionResult FillChequeRegister()
        {
            try
            {
                var view = _ChequeRegisterService.FillChequeRegister();

                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
