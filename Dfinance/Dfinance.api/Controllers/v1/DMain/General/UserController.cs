using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.General.Interface;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using static Dfinance.Shared.Routes.v1.ApiRoutes;

namespace Dfinance.api.Controllers.v1.DMain
{
    [ApiController]
    [Authorize]
   
    public class UserController : BaseController
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
		
        [HttpGet(ApiRoutes.User.UserDropDown)]
        public IActionResult UserDropDown()
        {
            try
            {
                var user = _userService.UserDropDown();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.User.FillUser)]
        public IActionResult GetAllUser()
        {
            try
            {
                var result = _userService.FillUser();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.User.FillUserById)]
        public IActionResult GetAllUserByID([FromQuery] int Id)
        {
            try
            {
                var result = _userService.FillUserById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(ApiRoutes.User.SaveUser)]
        public IActionResult SaveUser([FromBody] UserDto employeeDetailsDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _userService.SaveUser(employeeDetailsDto);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(ApiRoutes.User.UpdateUser)]
        public IActionResult UpdateUser([FromBody]UserDto employeeDetailsDto, int Id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
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
