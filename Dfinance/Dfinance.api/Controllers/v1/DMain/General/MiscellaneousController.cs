using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.General.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.General
{

    [ApiController]
    //[Authorize]
    public class MiscellaneousController : BaseController
    {
        private readonly IMiscellaneousService _miscService;

        public MiscellaneousController(IMiscellaneousService miscService)
        {
            this._miscService = miscService;
        }

        //dropdown for quality,country,
        [HttpGet(ApiRoutes.Miscellaneous.GetDropdown)]
        public IActionResult FillDropDown([FromQuery] string[] keys)
        {
            try
            {              
                    
                var result = _miscService.GetDropDown(keys);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
           }
        }

        //popup for color brand,country of origin
        [HttpGet(ApiRoutes.Miscellaneous.GetPopup)]
       public IActionResult FillPopup([FromQuery]string[] keys)
        {
            try
            {
                //string criteria = "";
                //if (m == "color")
                //    criteria = "Item Color";
                //else if (m == "brand")
                //    criteria = "Item Brand";
                //else if (m == "origin")
                //    criteria = "Item Origin";
                // var result = _miscService.MiscPopUp(criteria);
                var result = _miscService.GetPopup(keys);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
