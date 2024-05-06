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

        public async Task<IActionResult> GetLabel()
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
        public async Task<IActionResult> GetGrid()
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

        [HttpPatch(ApiRoutes.LabelGrid.UpdateLabel)]
        public IActionResult UpdateLabel(List<LabelDto> labelDto, string password)
        {
            try
            {
                var result = _labelgrid.UpdateLabel(labelDto, password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch(ApiRoutes.LabelGrid.Updategrid)]
        public IActionResult UpdateGrid(List<GridDto> gridDto, string password)
        {
            try
            {
                var result = _labelgrid.UpdateGrid(gridDto, password);
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
    }
}
