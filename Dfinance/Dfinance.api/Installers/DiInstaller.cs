using Dfinance.AuthAppllication.Services;
using Dfinance.AuthAppllication.Services.Interface;

namespace Dfinance.api.Installers;

public class DiInstaller : IInstaller
{
    public void InstallService(IServiceCollection service, IConfiguration configuration)
    {
        //authentication 
        service.AddScoped<IAuthService, AuthService>();

        service.AddScoped<ICompanyService, CompanyService>();
    }
}
