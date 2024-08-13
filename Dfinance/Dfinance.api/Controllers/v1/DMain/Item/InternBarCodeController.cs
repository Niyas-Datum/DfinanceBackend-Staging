using Dfinance.api.Authorization;
using Dfinance.DataModels.Dto.Item;
using Dfinance.Item.Services.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Item
{
    [Authorize]
    [ApiController]
    public class InternBarCodeController : Controller
    {
        private readonly IInternationalBarCodeService _internBarCode;
        public InternBarCodeController(IInternationalBarCodeService internBarCode)
        {
            _internBarCode= internBarCode;
        }
        [HttpGet(InvRoute.InternBarCode.FillIntnBarcode)]
        public IActionResult FillInternBarCode() 
        {
            try
            {
                var result = _internBarCode.FillInternBarCode();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.InternBarCode.SaveandUpdate)]
        public IActionResult SaveandUpdate(List<IntnBarCodeDto> intnBarCodeDto)
        {
            try
            {
                var result = _internBarCode.SaveUpdateIntBarcCode(intnBarCodeDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
