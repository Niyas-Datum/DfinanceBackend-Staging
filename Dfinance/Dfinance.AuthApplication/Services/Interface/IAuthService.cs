
using Dfinance.AuthAppllication.Dto;
using Dfinance.Core.Domain;
using Dfinance.Shared.Domain;

namespace Dfinance.AuthAppllication.Services.Interface;

public interface IAuthService
{
    CommonResponse Authenticate(AuthenticateRequestDto model);
    AuthResponseDto GetUserById(int? id);

    bool UserPermCheck(int pageid, int method);
    int? GetId();
    string GetUserName();
    int? GetBranchId();
    string? GetUserRole();
    string SetCon(DropdownLoginDto company);
    bool IsPageValid(int pageId);

}