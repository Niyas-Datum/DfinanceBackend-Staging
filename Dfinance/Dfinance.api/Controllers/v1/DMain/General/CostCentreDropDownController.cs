using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.General.Services.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.General
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CostCentreDropDownController : BaseController
    {
        private readonly ICostCentreDropDownService _service;
        public CostCentreDropDownController(ICostCentreDropDownService service)
        {
                _service = service;
        }

        [HttpGet(ApiRoutes.CostCentreDropDown.GetAll)]
        public IActionResult FillCostCentre() 
        {
            try
            {
                var result = _service.FillCostCentre();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
