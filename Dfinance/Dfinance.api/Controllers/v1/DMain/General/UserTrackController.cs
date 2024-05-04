using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.General.Interface;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.General
{
    [Authorize]
    [ApiController]
    public class UserTrackController : BaseController
    {
        private readonly IUserTrackService _UserTrackService;


        public UserTrackController(IUserTrackService userTrackService)
        {
            _UserTrackService = userTrackService;
        }

        [HttpGet(ApiRoutes.UserTrack.FillUserTrack)]
     
        public IActionResult FillUserTrack([FromQuery] UserTrackDto userTrackDto)
        {
            try
            {
                var view = _UserTrackService.FillUserTrack( userTrackDto);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}