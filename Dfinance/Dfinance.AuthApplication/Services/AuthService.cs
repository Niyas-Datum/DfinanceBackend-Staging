using Dfinance.AuthAppllication.Authorization;
using Dfinance.AuthAppllication.Dto;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.AuthCore.Infrastructure;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Shared.Domain;

namespace Dfinance.AuthAppllication.Services;

public class AuthService : IAuthService
{
    private readonly DFCoreContext _dfCoreContext;

    private static MaEmployee _User = null!;
   

    private readonly IJwtSecret _jwtsecret;
    public AuthService(IJwtSecret jwtSecret, DFCoreContext dFCoreContext)
    {
        _jwtsecret = jwtSecret;
        _dfCoreContext = dFCoreContext;
    }


    public CommonResponse Authenticate(AuthenticateRequestDto model)
    {

        _User = _dfCoreContext.MaEmployees.SingleOrDefault(u => u.UserName == model.Username)!;

        if(_User == null)
        return CommonResponse.Error("User Account not found");

       // if ( user.Password == user.Password)

        //var user = _users.SingleOrDefault(x => x.UserName == model.Username && x.Password == model.Password);
      
        var token = _jwtsecret.GenerateJwtToken(_User);

        return CommonResponse.Ok(new AuthResponseDto(_User, token));

    }

    public MaEmployee GetUserById(int? id )
    {
        if (_User == null) return null!;
        if (_User.Id == id) return _User;

        return null!;

    }

    public int? GetId()
    {
        return _User.Id;
    }

    public string GetUserName()
    {
        return _User.UserName;
    }

}
