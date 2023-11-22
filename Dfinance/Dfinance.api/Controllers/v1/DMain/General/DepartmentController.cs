using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Dto;
using Dfinance.Application.Services;
using Dfinance.Application.Services.General;
using Dfinance.Application.Services.Interface;
using Dfinance.Application.Services.Interface.IGeneral;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentTypeService _departmentService;
        public DepartmentController(IDepartmentTypeService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpGet(ApiRoutes.DepartmentType.GetAll)]
        public IActionResult FillDepartmentTypes()
        {
            try
            {
                var data = _departmentService.FillDepartmentTypes();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.DepartmentType.FillDepartmentTypesById)]
        public IActionResult FillDepartmentTypesById(int Id)
        {
            try
            {
                var result = _departmentService.FillDepartmentTypesById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(ApiRoutes.DepartmentType.AddDepartmentTypes)]
  
        public IActionResult AddDepartmentTypes(DepartmentTypeDto DepartmentDto)
        {
            try
            {
                object result = _departmentService.AddDepartmentTypes(DepartmentDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
              
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(ApiRoutes.DepartmentType.UpdateDepartmentTypes)]
        public IActionResult UpdateDepartmentTypes(DepartmentTypeDto DepartmentTypeDto,int Id)
        {
            try
            {
                object result =
                    _departmentService.UpdateDepartmentTypes(DepartmentTypeDto, Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }
        [HttpDelete(ApiRoutes.DepartmentType.DeleteDepartmentTypes)]
        public IActionResult DeleteBranch(int Id)
        {
            try
            {
                var result = _departmentService.DeleteDepartmentTypes(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
