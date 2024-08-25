using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Inventory
{
    [ApiController]
    [Authorize]
    public class QualityTypesController : BaseController
    {
        private readonly IQualityTypeService _qualityTypeService;
        public QualityTypesController(IQualityTypeService qualityTypeService)
        {
            _qualityTypeService = qualityTypeService;
        }
        [HttpGet(InvRoute.QualityTypes.Popup)]
        public IActionResult QualityinCaretPopup()
        {
            try
            {
                var result = _qualityTypeService.QualityinCaretPopup();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.QualityTypes.Fill)]
        public IActionResult Fill()
        {
            try
            {
                var result = _qualityTypeService.Fill();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.QualityTypes.SaveAndUpdate)]
        public IActionResult SaveEditQualityType(List<QualityTypeDto> qualityTypeDto)
        {
            try
            {
                var result = _qualityTypeService.SaveEditQualityType(qualityTypeDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
