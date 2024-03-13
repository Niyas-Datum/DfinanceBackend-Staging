using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.General.Interface;
using Dfinance.DataModels.Dto;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain
{
    [ApiController]
    [Authorize]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentTypeService _departmentService;
        public DepartmentController(IDepartmentTypeService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpGet(ApiRoutes.Department.DeptPopup)]

        public IActionResult DeptPopup()
        {
            try
            {

                var result = _departmentService.DeptPopup();

                return Ok(result);

            }

            catch (Exception ex)

            {

                return BadRequest(ex.Message);

            }

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

        [HttpPost(ApiRoutes.Department.SaveDepartmentTypes)]

        public IActionResult AddDepartment([FromBody] DepartmentTypeDto DepartmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _departmentService.AddDepartment(DepartmentDto);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(ApiRoutes.Department.UpdateDepartmentType)]

        public IActionResult UpdateDepartment([FromBody] DepartmentTypeDto DepartmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _departmentService.AddDepartment(DepartmentDto);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(ApiRoutes.Department.DeleteDepartmentTypes)]
        public IActionResult DeleteDepartmentTypes(int Id)
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
