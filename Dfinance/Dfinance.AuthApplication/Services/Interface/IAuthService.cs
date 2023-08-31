
using Dfinance.AuthAppllication.Dto;
using Dfinance.Core.Domain;
using Dfinance.Shared.Domain;

namespace Dfinance.AuthAppllication.Services.Interface;

public interface IAuthService
{
    CommonResponse Authenticate(AuthenticateRequestDto model);
    MaEmployee GetUserById(int? id);
    int? GetId();
    string GetUserName();

}