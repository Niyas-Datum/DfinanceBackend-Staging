using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Shared.Routes.v1;
using Dfinance.Stakeholder.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.CustomerSupplier
{
    [ApiController]
    [Authorize]

    public class CustomerController : BaseController
    {
        private readonly ICustomerService _custSuppService;

        public CustomerController(ICustomerService custSuppService)
        {
            _custSuppService = custSuppService;
        }
        [HttpGet(ApiRoutes.CustomerDetails.GetCommodity)]

        public IActionResult GetCommodity()
        {
            try
            {
                var result = _custSuppService.FillCommodity();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiRoutes.CustomerDetails.GetPriceCategory)]

        public IActionResult GetPriceCategory()
        {
            try
            {
                var result = _custSuppService.FillPriceCategory();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.CustomerDetails.CustomerCategories)]

        public IActionResult GetCustomerCategories()
        {
            try
            {
                var result = _custSuppService.FillCustomerCategories();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
