using Dfinance.AuthAppllication.Services;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Application.General.Services;
using Dfinance.Application.General.Services.Interface;
using Dfinance.Application.Services.General;
using Dfinance.Application.Services.Interface.IGeneral;
using Dfinance.Application.Services.Interface;
using Dfinance.Application.Services;
using Dfinance.Application.Inventory.Services.Interface;
using Dfinance.Application.Inventory.Services;
using Dfinance.AuthApplication.Services.Interface;
using Dfinance.AuthApplication.Services;

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

        service.AddScoped<IMaDesignationsService, MaDesignationsService>();

        service.AddScoped<ICostCategoryService, CostCategoryService>();

        service.AddScoped<ICostCentreService, CostCentreService>();

        service.AddScoped<ICostCategoryDropDownService, CostCategoryDropDownService>();

        service.AddScoped<ICostCentreDropDownService, CostCentreDropDownService>();

        service.AddScoped<IStatusDropDownService, StatusDropDownService>();

        service.AddScoped<ICategoryService, CategoryService>();

        service.AddScoped<IAreaMasterService, AreaMasterService>();
    }
}
