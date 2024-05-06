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
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;

        }


        [HttpGet(ApiRoutes.Role.FillRole)]
        public IActionResult FillRoleMaster()
        {
            try
            {
                var data = _roleService.FillRoleMaster();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiRoutes.Role.FillRoleRight)]
        public IActionResult FillRoleRight()
        {
            try
            {
                var data = _roleService.FillRoleRights();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiRoutes.Role.FillRoleAndRight)]
        public IActionResult FillRoleAndRight(int Id)
        {
            try
            {
                var data = _roleService.FillRoleandRoleRights(Id);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(ApiRoutes.Role.Saverole)]
        public IActionResult Saverole(RoleDto roleDto)
        {
            try
            {
                var result = _roleService.AddRole( roleDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpDelete(ApiRoutes.Role.DeleteRole)]
        public IActionResult Deleterole(int Id)
        {
            try
            {
                var result = _roleService.DeleteRole(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPatch(ApiRoutes.Role.UpdateRole)]
        public IActionResult Updaterole(RoleDto roleDto) 
        {
            try
            {
                var result = _roleService.UpdateRole(roleDto);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
