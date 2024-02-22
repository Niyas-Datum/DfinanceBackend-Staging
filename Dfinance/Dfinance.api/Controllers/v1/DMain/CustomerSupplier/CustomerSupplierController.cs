using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.CustSupp;
using Dfinance.Shared.Routes.v1;
using Dfinance.Stakeholder.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.CustomerSupplier
{
    [ApiController]
    [Authorize]

    public class CustomerSupplierController : BaseController
    {
            private readonly ICustomerSupplierService _custSuppService;

            public CustomerSupplierController(ICustomerSupplierService custSuppService)
            {
                _custSuppService = custSuppService;
            }

        [HttpGet(ApiRoutes.Parties.GetCode)]

        public IActionResult GetCode()
        {
            try
            {
                var result = _custSuppService.GetCode();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.Parties.GetType)]
        public IActionResult FillType()
        {
            var result = _custSuppService.FillPartyType();


            return Ok(result.Data);

        }
        [HttpGet(ApiRoutes.Parties.GetCategory)]
        public IActionResult FillCategory()
        {
            var result = _custSuppService.FillCategory();


            return Ok(result.Data);

        }


        [AllowAnonymous]
        [HttpPost(ApiRoutes.Parties.SaveCustmsupp)]
        public IActionResult SaveGen([FromBody] GeneralDto generalDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = _custSuppService.SaveGen(generalDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPatch(ApiRoutes.Parties.update)]
        public IActionResult Update([FromBody] GeneralDto generalDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = _custSuppService.SaveGen(generalDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
        }
