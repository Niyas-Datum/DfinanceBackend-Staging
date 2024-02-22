using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.General.Interface;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.General
{
    [ApiController]
    [Authorize]
   
    public class DesignationController : BaseController
    {
        private readonly IDesignationsService _maDesignationsService;
        public DesignationController(IDesignationsService maDesignationsService)
        {
            _maDesignationsService = maDesignationsService;
           
        }
        [HttpGet(ApiRoutes.Designation.FillAllDesignation)]

        public IActionResult GetAllDesignation()
        {
            try
            {
                var data = _maDesignationsService.FillAllDesignation();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.Designation.FillDesignationById)]
        public IActionResult GetAllDesignationById(int Id)
        {
            try
            {
                var result = _maDesignationsService.FillDesignationById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    
    [HttpPost(ApiRoutes.Designation.SaveDesignation)]
        public ActionResult AddDesignation([FromBody] DesignationsDto designationsdto)
        {
            try
            {
                var data = _maDesignationsService.SaveDesignations(designationsdto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(ApiRoutes.Designation.UpdateDesignation)]
        public IActionResult UpdateDepartmentTypes([FromBody] DesignationsDto designationsdto, int Id)
        {
            try
            {
                object result = _maDesignationsService.UpdateDesignation(designationsdto, Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(ApiRoutes.Designation.DeleteDesignation)]
        public IActionResult DeleteDesignation( int Id)
        {
            try
            {
                var result = _maDesignationsService.DeleteDesignation(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
