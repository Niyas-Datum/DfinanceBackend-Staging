using Dfinance.AuthApplication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.AuthApplication.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly DFCoreContext _dfCoreContext;
        public PasswordService(DFCoreContext dfCoreContext)
        {
            _dfCoreContext = dfCoreContext;
        }
 public CommonResponse IsPasswordOk(string password)
   {
    try
    {
        const string criteria = "Password";

        var isValidPassword = _dfCoreContext.PasswordCheckResult
            .FromSqlRaw("EXEC SettingsSP @Criteria = {0}, @Password = {1}", criteria, password)
            .AsEnumerable()
            .FirstOrDefault();

        return new CommonResponse
        {
            Data = isValidPassword != null && isValidPassword.Result == 1,
        };
    }
    catch (Exception ex)
    {
        return CommonResponse.Error(ex);
    }
}

    }
}
