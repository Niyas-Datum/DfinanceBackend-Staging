using Microsoft.AspNetCore.Mvc;
using Dfinance.Application.General.Services.Interface;
using Dfinance.api.Authorization;
using Dfinance.Shared.Routes.v1;
using Dfinance.Application.Dto;
using Dfinance.api.Framework;

namespace Dfinance.api.Controllers.v1.DMain.General
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchController : BaseController
    {
        private readonly IBranchService _branchService;
        public BranchController(IBranchService branchService)
        {
                _branchService = branchService;
        }
        [HttpGet(ApiRoutes.Branch.GetBranchDropDown)]
        [AllowAnonymous]
        public async Task<IActionResult> GetBranchDropDown()
        {
            try
            {

                return Ok(_branchService.GetBranchesDropDown());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
        [HttpPost(ApiRoutes.Branch.SaveBranch)]
        public IActionResult SaveBranch([FromBody] BranchDto branch)
        {
            try
            {                
                var result = _branchService.SaveBranch(branch);
                return Ok(result);

            }
            catch (Exception ex)
            {
                //return CommonResponse.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(ApiRoutes.Branch.UpdateBranch)]      
        public IActionResult UpdateBranch([FromBody]  BranchDto branch,int Id)
        {
            try
            {
                var result = _branchService.UpdateBranch(branch, Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(ApiRoutes.Branch.Delete)]
        public IActionResult DeleteBranch(int Id)
        {
            try
            {
                var result = _branchService.DeleteBranch(Id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      [HttpGet(ApiRoutes.Branch.FillAllBranch)]
        public IActionResult FillAllBranch()
        {
            try
            {
                var result = _branchService.FillAllBranch();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiRoutes.Branch.FillBranchById)]
        public IActionResult FillBranchByID(int Id)
        {
            try
            {
                var result=_branchService.FillBranchByID(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
