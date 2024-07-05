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

        [HttpGet(ApiRoutes.Parties.FillParty)]
        public IActionResult FillParty()
        {
            var result = _custSuppService.FillParty();


            return Ok(result.Data);

        }

        [HttpGet(ApiRoutes.Parties.FillPartyById)]
        public IActionResult FillPartyWithID(int Id,int pageId)
        {
            try
            {
                var result = _custSuppService.FillPartyWithID(Id,pageId);
                return Ok(result.Data);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }


        [HttpPost(ApiRoutes.Parties.SaveCustmsupp)]
        public IActionResult SaveGen([FromBody] GeneralDto generalDto,int pageId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = _custSuppService.SaveGen(generalDto,pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPatch(ApiRoutes.Parties.update)]
        public IActionResult Update([FromBody] GeneralDto generalDto,int pageId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = _custSuppService.SaveGen(generalDto,pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.Parties.supplier)]

        public IActionResult GetSupplier(int locId, int pageId, int voucherId, string criteria)
        {
            try
            {
                var result = _custSuppService.FillSupplier(locId, pageId, voucherId,criteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete(ApiRoutes.Parties.Delete)]
        public IActionResult Delete(int Id,int pageId)
        {
            try
            {
                var result = _custSuppService.Delete(Id,pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
       

}
