using Dfiance.Hr.Employees.Interface;
using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Hr
{
    [ApiController]
    [Authorize]
    public class HRController:BaseController
    {
        private readonly IHrEmployeeService _employeeService;
        public HRController(IHrEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpGet(ApiRoutes.HR.Salesman)]
        [AllowAnonymous]
        public async Task<IActionResult> GetSalesman()
        {
            try
            {

                return Ok(_employeeService.GetSalesMan());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }
    }
}
