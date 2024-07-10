using Dfinance.api.Authorization;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Dfinance.Shared.Routes.v1.FinRoute;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [Authorize]
    [ApiController]
    public class AccountConfigController : ControllerBase
    {
        private readonly IAccountConfigurationService _account;
        public AccountConfigController(IAccountConfigurationService account)
        {
            _account = account;
        }
        [HttpPost(FinRoute.AccConfig.fill)]
        public IActionResult FillAccConfig()
        {
            try
            {
                var result =_account.FillAccConfig();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(FinRoute.AccConfig.update)]
        public IActionResult SaveAccConfig(List<AccConfigDto> accConfig)
        {
            var result = _account.SaveAccConfig(accConfig);
            return Ok(result);
        }
    }
}
