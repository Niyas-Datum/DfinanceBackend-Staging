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
        [HttpGet(ApiRoutes.Department.DropDown)]
        public IActionResult DepartmentDropdown()
        {
            try
            {
                var data = _departmentService.DepartmentDropdown();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.Department.FillAllDepartment)]
        public IActionResult FillDepartment()
        {
            try
            {
                var data = _departmentService.FillDepartment();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.Department.FillDepartmentById)]
        public IActionResult FillDepartmentById(int Id)
        {
            try
            {
                var result = _departmentService.FillDepartmentById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.DepartmentType.FillAll)]
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
        [HttpPost(ApiRoutes.DepartmentType.SaveDepartmentTypes)]
  
        public IActionResult SaveDepartmentTypes([FromBody] DepartmentTypeDto DepartmentDto)
        {
            try
            {
                object result = _departmentService.SaveDepartmentTypes(DepartmentDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
              
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(ApiRoutes.DepartmentType.UpdateDepartmentTypes)]
        public IActionResult UpdateDepartmentTypes([FromBody] DepartmentTypeDto DepartmentTypeDto,int Id)
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
        public IActionResult DeleteDepartmentTypes( int Id)
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
