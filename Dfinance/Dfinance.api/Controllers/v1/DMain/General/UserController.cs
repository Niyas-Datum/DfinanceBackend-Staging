using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Dto;
using Dfinance.Application.Dto.General;
using Dfinance.Application.Services.Interface.IGeneral;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using static Dfinance.Shared.Routes.v1.ApiRoutes;

namespace Dfinance.api.Controllers.v1.DMain
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet(ApiRoutes.User.GetAllUser)]
        public IActionResult FillUser()
        {
            try
            {
                var user = _userService.FillUser();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.User.GetUserById)]
        public IActionResult GetAllUserByID(int Id)
        {
            try
            {
                var result = _userService.GetUserById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(ApiRoutes.User.AddUser)]
        public IActionResult AddUser(UserDto employeeDetailsDto)
        {
            try
            {
                object result = _userService.AddUser(employeeDetailsDto);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(ApiRoutes.User.UpdateUser)]
        public IActionResult UpdateUser(UserDto employeeDetailsDto, int Id)
        {
            try
            {
                object result = _userService.UpdateUser(employeeDetailsDto, Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(ApiRoutes.User.DeleteUserRight)]
        public IActionResult DeleteUserRight(int Id)
        {
            try
            {
                var result = _userService.DeleteUserRight(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(ApiRoutes.User.DeleteBranchDetails)]
        public IActionResult DeleteBranchDetails(int Id)
        {
            try
            {
                var result = _userService.DeleteBranchdetails(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(ApiRoutes.User.DeleteUser)]
        public IActionResult DeleteUser(int Id)
        {
            try
            {
                var result = _userService.Deleteuser(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
