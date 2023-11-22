using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.General.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dfinance.Shared.Routes;
using Dfinance.Shared.Routes.v1;

namespace Dfinance.api.Controllers.v1.DMain.General
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CostCategoryDropDownController : BaseController
    {
        private readonly ICostCategoryDropDownService _service;
        public CostCategoryDropDownController(ICostCategoryDropDownService service)
        {
                _service = service;
        }
        [HttpGet(ApiRoutes.CostCategoryDropDown.GetAll)]
        public IActionResult FillCostCategory() 
        {
            try
            {
                var categories = _service.FillCostCategory();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
