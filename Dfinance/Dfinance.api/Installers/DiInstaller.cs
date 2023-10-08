using Dfinance.AuthAppllication.Services;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Application.General.Services;
using Dfinance.Application.General.Services.Interface;
using Dfinance.Application.Services.General;
using Dfinance.Application.Services.Interface.IGeneral;
using Dfinance.Application.Services.Interface;
using Dfinance.Application.Services;

namespace Dfinance.api.Installers;

public class DiInstaller : IInstaller
{
    public void InstallService(IServiceCollection service, IConfiguration configuration)
    {
        //authentication 
        service.AddScoped<IAuthService, AuthService>();

        service.AddScoped<ICompanyService, CompanyService>();

        service.AddScoped<IBranchService, BranchService>();

        service.AddScoped<ICountryDropDownService,CountryDropDownService>();

        service.AddScoped<IEncryptService, EncryptService>();
        service.AddScoped<IDepartmentTypeService, DepartmentTypeService>();
        service.AddScoped<IEmployeeService, EmployeeService>();

    }
}
