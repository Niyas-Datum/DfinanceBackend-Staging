using Dfinance.AuthAppllication.Services;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Application.Services.General;
using Dfinance.Application.Services.Interface;
using Dfinance.Application.Services;
using Dfinance.AuthApplication.Services.Interface;
using Dfinance.AuthApplication.Services;
using Dfinance.Shared.Configuration.Service;
using Dfinance.Application.Services.General.Interface;
using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.Application.Services.Inventory;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.Application.Services.Finance;

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

        service.AddScoped<IDecryptService,DecryptService>();    

        service.AddScoped<IDepartmentTypeService, DepartmentTypeService>();

        service.AddScoped<IUserService, UserService>();

        service.AddScoped<IDesignationsService, DesignationsService>();

        service.AddScoped<ICostCategoryService, CostCategoryService>();

        service.AddScoped<ICostCentreService, CostCentreService>();
        service.AddScoped<IStatusDropDownService, StatusDropDownService>();
        

        service.AddScoped<ICategoryService, CategoryService>();
        service.AddScoped<ICategoryTypeService, CategoryTypeService>();
		service.AddScoped<ICurrencyService, CurrencyService>();

        service.AddScoped<IAreaMasterService, AreaMasterService>();
        service.AddSingleton<IConnectionServices, ConnectionServices>();
    }
}
