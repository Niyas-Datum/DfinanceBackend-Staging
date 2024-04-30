using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.LabelAndGridSettings.Interface;
using Dfinance.Application.Services.General.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Http;
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
    }
}
