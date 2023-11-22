using Microsoft.AspNetCore.Mvc;
using Dfinance.AuthAppllication.Services;
using Dfinance.api.Framework;
using Dfinance.Shared.Routes.v1;
using Dfinance.api.Authorization;

namespace Dfinance.api.Controllers.v1.dm;

/**
# @use: F
**/
[ApiController]
[Authorize]
[Route("[controller]")]
public class CompanyController : BaseController
{
    private readonly ICompanyService _companyService;
    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet(ApiRoutes.Company.GetAll)]
    [AllowAnonymous]
    public async Task<IActionResult> GetALl()
    {
        try
        {

            return Ok(_companyService.GetCompanies());
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex });
        }
    }
}
