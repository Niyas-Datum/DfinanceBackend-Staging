using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.General;
using Dfinance.Application.Services.General.Interface;
using Dfinance.ChartOfAccount.Services.Finance.Interface;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace Dfinance.api.Controllers.v1.DMain.General
{
    [Authorize]
    [ApiController]
    public class PageMenuController : BaseController
    {
        private readonly IPageMenuService _chartOfMenu;
        public PageMenuController(IPageMenuService chartOfMenu)
        {
            _chartOfMenu = chartOfMenu;
        }


        [HttpGet(ApiRoutes.PageMenu.fillMenu)]
        [SwaggerOperation(Summary = "Fill the Menu. If AllPages is True, Fills All the pages")]      
        public IActionResult FillMenu(bool AllPages, int? parentId = null)
        {
            try
            {
                var result = _chartOfMenu.FillMenu(AllPages,parentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
        [HttpGet(ApiRoutes.PageMenu.grpModules)]
        [SwaggerOperation(Summary = "Fill the Group and Modules Dropdowns")]
        public IActionResult FillGroupAndModules()
        {
            try
            {
                var result = _chartOfMenu.FillGroupAndModules();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
        [HttpPost(ApiRoutes.PageMenu.save)]
        [SwaggerOperation(Summary = "Save or Update the Page")]
        public IActionResult SavePageMenu(PageMenuDto pageMenuDto, int? parentId = null)
        {
            var result = _chartOfMenu.SavePageMenu(pageMenuDto, parentId);
            return Ok(result);
        }
        [HttpDelete(ApiRoutes.PageMenu.delete)]
        [SwaggerOperation(Summary = "Delete the Page")]
        public IActionResult DeletePageMenu(int Id)
        {
            var result = _chartOfMenu.DeletePageMenu(Id);
            return Ok(result);
        }
        [HttpPatch(ApiRoutes.PageMenu.update)]
        [SwaggerOperation(Summary = "Update Active and FrequentlyUsed in PageMenu")]
        public IActionResult UpdateActive(List<PageActiveDto> pageActiveDto)
        {
            var result=_chartOfMenu.UpdateActive(pageActiveDto);
            return Ok(result);
        }
    }
}
