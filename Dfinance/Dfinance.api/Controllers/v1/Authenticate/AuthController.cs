using Dfinance.api.Framework;
using Dfinance.AuthAppllication.Dto;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Shared.Configuration.Service;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.Authenticate;

[ApiController]
[Route("auth")]
public class AuthController : BaseController
{

    private readonly IAuthService _authservice;
    private readonly IConnectionServices _connectionServices;
   

    public AuthController(IAuthService authService,
       
       IConnectionServices connectionServices
        )
    {
        _authservice = authService;
        _connectionServices = connectionServices;   
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateRequestDto model)
    {
        if (model == null) return BadRequest();
        var data = _authservice.Authenticate(model);
        return Response(data);
    }

    [HttpGet("setcon")]
    public async Task<IActionResult> Setcon( [FromQuery] DropdownLoginDto model)
    {
        try
        {


            var constng = _authservice.SetCon(model);
            if (constng == null) return BadRequest();
            if(_connectionServices.Setcon(constng))
            return Ok("Connection successfull");
            else return BadRequest();
        }catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
   
    }

    [HttpGet("autologin")]
    public async Task<IActionResult> AutoConnect()
    {
        DropdownLoginDto dropdownLoginDto = new DropdownLoginDto() { Id=46, Value=""};
        var constng =  _authservice.SetCon(dropdownLoginDto);
        if (constng == null) return BadRequest();
       if( _connectionServices.Setcon(constng))
        {
            AuthenticateRequestDto authenticateRequestDto = new AuthenticateRequestDto()
            {
                Username = "user",
                Password = "user",
                Branch = new DropdownLoginDto() { Id = 1 }
            };
            var data =  _authservice.Authenticate(authenticateRequestDto);
            return Response(data);

        }
        else
        {
            return BadRequest();
        }


    }

    }