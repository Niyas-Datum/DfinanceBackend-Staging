using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Dto;
using Dfinance.Application.Dto.General;
using Dfinance.Application.General.Services;
using Dfinance.Application.General.Services.Interface;
using Dfinance.Application.Services.Interface.IGeneral;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.General
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class MaDesignationController : BaseController
    {
        private readonly IMaDesignationsService _maDesignationsService;
        public MaDesignationController(IMaDesignationsService maDesignationsService)
        {
            _maDesignationsService = maDesignationsService;
           
        }
        [HttpGet(ApiRoutes.Designation.GetAllDesignation)]

        public IActionResult GetAllDesignation()
        {
            try
            {
                var data = _maDesignationsService.GetAllDesignation();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.Designation.GetAllDesignationById)]
        public IActionResult GetAllDesignationById(int Id)
        {
            try
            {
                var result = _maDesignationsService.GetAllDesignationById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    
    [HttpPost(ApiRoutes.Designation.AddDesignation)]
        public ActionResult AddDesignation(MaDesignationsDto designationsdto)
        {
            try
            {
                var data = _maDesignationsService.AddDesignations(designationsdto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(ApiRoutes.Designation.UpdateDesignation)]
        public IActionResult UpdateDepartmentTypes(MaDesignationsDto designationsdto, int Id)
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
        public IActionResult DeleteDesignation(int Id)
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
