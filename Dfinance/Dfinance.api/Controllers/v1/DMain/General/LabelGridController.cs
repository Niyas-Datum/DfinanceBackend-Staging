using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.LabelAndGridSettings.Interface;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.General
{
    [ApiController]
    [Authorize]
    public class LabelGridController : BaseController
    {
        private readonly ILabelAndGridSettings _labelgrid;
        public LabelGridController(ILabelAndGridSettings labelgrid)
        {
            _labelgrid = labelgrid;
        }
        [HttpGet(ApiRoutes.LabelGrid.Getlabel)]

        public IActionResult GetLabel()
        {
            try
            {

                return Ok(_labelgrid.FillFormLabelSettings());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
        [HttpGet(ApiRoutes.LabelGrid.Getgrid)]
        public IActionResult GetGrid()
        {
            try
            {

                return Ok(_labelgrid.FillGridSettings());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }

        [HttpGet(ApiRoutes.LabelGrid.FormName)]
        public IActionResult GetFormNamer()
        {
            try
            {

                return Ok(_labelgrid.FormNamePopup());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }

        [HttpGet(ApiRoutes.LabelGrid.PagePopUp)]
        public IActionResult GetPage()
        {
            try
            {

                return Ok(_labelgrid.PagePopUp());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }



        [HttpPost(ApiRoutes.LabelGrid.SaveUpdateLabel)]
        public IActionResult SaveAndUpdateLabel(List<LabelDto> labelDto, string password)
        {
            try
            {
                var result = _labelgrid.SaveAndUpdateLabel(labelDto, password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(ApiRoutes.LabelGrid.SaveUpdategrid)]
        public IActionResult  SaveAndUpdateGrid(List<GridDto> gridDto, string password)
        {
            try
            {
                var result = _labelgrid.SaveAndUpdateGrid(gridDto, password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet(ApiRoutes.LabelGrid.labelGridpopup)]
        public IActionResult labelGridpopup()
        {
            try
            {
                var result = _labelgrid.labelGridpopup();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.LabelGrid.getgridbyId)]
        public IActionResult GetGridByPageId(int pageId)
        {
            try
            {
                var result = _labelgrid.GetGridByPageId(pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
