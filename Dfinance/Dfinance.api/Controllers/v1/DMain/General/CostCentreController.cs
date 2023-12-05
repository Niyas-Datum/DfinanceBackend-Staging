using Dfinance.api.Authorization;
using Dfinance.api.Framework;

using Microsoft.AspNetCore.Mvc;
using Dfinance.Shared.Routes.v1;
using Dfinance.Application.Dto.General;
using Dfinance.Application.Services.General.Interface;

namespace Dfinance.api.Controllers.v1.DMain.General
{
    
    [ApiController]
    [Authorize]
    public class CostCentreController : BaseController
    {
        private readonly ICostCentreService _costCentreService;
        public CostCentreController(ICostCentreService costCentreService)
        {
                _costCentreService = costCentreService;
        }

        [HttpGet(ApiRoutes.CostCentre.FillCostCentre)]
        public IActionResult FillCostCentre()
        {
            try
            {
                var result = _costCentreService.FillCostCentre();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiRoutes.CostCentre.FillCostCentreById)]
        public IActionResult FillCostCentreById(int Id)
        {
            try
            {
                var result = _costCentreService.FillCostCentreById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(ApiRoutes.CostCentre.SaveCostCentre)]
        public IActionResult SaveCostCentre([FromBody] CostCentreDto costCentreDto)
        {
            try
            {
                var result = _costCentreService.SaveCostCentre(costCentreDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                 return BadRequest(ex.Message);
            }
        }

        [HttpPatch(ApiRoutes.CostCentre.UpdateCostCentre)]
        public IActionResult UpdateCostCentre([FromBody] CostCentreDto costCentreDto,int Id)
        {
            try
            {
                var result = _costCentreService.UpdateCostCentre(costCentreDto, Id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(ApiRoutes.CostCentre.DeleteCostCentre)]
        public IActionResult DeleteCostCentre(int id)
        {
            try
            {
                var result = _costCentreService.DeleteCostCentre(id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.CostCentre.DropDown)]
        public IActionResult FillCostCentreDropDown()
        {
            try
            {
                var result = _costCentreService.FillCostCentreDropDown();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiRoutes.CostCentre.FillPopUp)]
        public IActionResult FillPopUp(string Description)
        {
            try
            {
                var result = _costCentreService.FillPopUp(Description);
                return Ok(result);
            }
            catch( Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
