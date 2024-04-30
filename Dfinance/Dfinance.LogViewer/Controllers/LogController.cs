using Dfinance.LogViewer.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.LogViewer.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        private readonly IViewLogService _viewLogService;
        public LogController(IViewLogService viewLogService)

        {
            _viewLogService = viewLogService;

        }
        [HttpGet]
        public IActionResult ViewLogs(DateOnly date, string method = null)
        {
            try
            {
                var result = _viewLogService.ViewLogs(date,method);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
