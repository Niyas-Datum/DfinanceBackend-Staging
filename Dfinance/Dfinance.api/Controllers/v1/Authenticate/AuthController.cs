using Dfinance.api.Framework;
using Dfinance.AuthAppllication.Dto;
using Dfinance.AuthAppllication.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.Authenticate;

[ApiController]
[Route("auth")]
public class AuthController : BaseController
{

    private readonly IAuthService _authservice;
    public AuthController(IAuthService authService)
    {
        _authservice = authService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateRequestDto model)
    {
        if (model == null) return BadRequest();
        var data = _authservice.Authenticate(model);
        return Response(data);
    }

}