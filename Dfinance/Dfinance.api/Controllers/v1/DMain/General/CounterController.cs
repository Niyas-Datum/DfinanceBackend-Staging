using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.General.Interface;
using Dfinance.Core.Views.General;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.General
{
    [Authorize]
    [ApiController]
    public class CounterController : BaseController
    {
       private readonly ICountersService _countersService;
        public CounterController(ICountersService countersService)
        {
            _countersService = countersService;
        }
        [HttpGet(ApiRoutes.Counters.Fillcounters)]
        public IActionResult FillMaster()
        {
            try
            {
                var view = _countersService.FillMaster();
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.Counters.FillcountersById)]
        public IActionResult FillCountersById(int Id)
        {
            try
            {
                var view = _countersService.FillCountersById(Id);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.Counters.GetMachineNameandIp)]
        public IActionResult GetNameandIp()
        {
            try
            {
                var view = _countersService.GetNameandIp();
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(ApiRoutes.Counters.Save)]
        public IActionResult SaveCounters(CounterDto counterDto)
        {
            try
            {
                var view = _countersService.SaveCounters(counterDto);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(ApiRoutes.Counters.Update)]
        public IActionResult UpdateCounters(CounterDto counterDto, int PageId)
        {
            try
            {
                var view = _countersService.UpdateCounters(counterDto, PageId);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(ApiRoutes.Counters.Delete)]
        public IActionResult DeleteCounter(int Id, int PageId)
        {
            try
            {
                var view = _countersService.DeleteCounter(Id, PageId);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
