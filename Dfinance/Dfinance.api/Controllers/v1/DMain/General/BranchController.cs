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
        //[HttpGet(ApiRoutes.Branch.GetAllBranch)]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetALl()
        //{





        //    try
        //    {

        //        return Ok(_branchService.GetBranches());
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex });
        //    }
        //}
        //dropdown Branch
        [HttpGet(ApiRoutes.Branch.FillAllBranch)]

        public IActionResult Fillallbranch()
        {
            try
            {
                var data = _branchService.Fillallbranch();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(ApiRoutes.Branch.SaveBranch)]
        public IActionResult SaveBranch(MaCompanyDto companyDto)
        {
            try
            {                
                object result = _branchService.SaveBranch(companyDto);
                return Ok(result);

            }
            catch (Exception ex)
            {
                //return CommonResponse.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(ApiRoutes.Branch.UpdateBranch)]      
        public IActionResult UpdateBranch(MaCompanyDto companyDto,int Id)
        {
            try
            {
                object result = _branchService.UpdateBranch(companyDto, Id);
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

        [HttpGet(ApiRoutes.Branch.GetAllBranch)]
        public IActionResult GetAllBranches()
        {
            try
            {
                var result = _branchService.GetAllBranch();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiRoutes.Branch.GetAllById)]
        public IActionResult GetAllBranchByID(int Id)
        {
            try
            {
                var result=_branchService.GetBranchByID(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
