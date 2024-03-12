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
using Dfinance.Stakeholder.Services.Interface;
using Dfinance.Stakeholder.Services;
using Dfinance.ChartOfAccount.Services.Finance.Interface;
using Dfinance.ChartOfAccount.Services.Finance;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.Application.Services.Finance;
using Dfiance.Hr.Employees.Interface;
using Dfiance.Hr.Employees;
using Dfinance.Item.Services.Inventory; 
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Application;

namespace Dfinance.api.Installers;

public class DiInstaller : IInstaller
{
    public void InstallService(IServiceCollection service, IConfiguration configuration)
    {
		//Master DB 
		service.AddScoped<ICompanyService, CompanyService>();
        //authentication 
        service.AddScoped<IAuthService, AuthService>();
        service.AddScoped<IEncryptService, EncryptService>();
        service.AddScoped<IDecryptService, DecryptService>();
        service.AddSingleton<IConnectionServices, ConnectionServices>();

        //GENERAL
        service.AddScoped<IBranchService, BranchService>();
       
        service.AddScoped<IDepartmentTypeService, DepartmentTypeService>();
        service.AddScoped<IUserService, UserService>();
        service.AddScoped<IDesignationsService, DesignationsService>();
        service.AddScoped<ICostCategoryService, CostCategoryService>();
        service.AddScoped<ICostCentreService, CostCentreService>();
        service.AddScoped<ISettingsService, SettingsService>();

        //MAMISC        
        service.AddScoped<IMiscellaneousService, MiscellaneousService>();

        //Inventory
        service.AddScoped<ICategoryService, CategoryService>();
        service.AddScoped<ICategoryTypeService, CategoryTypeService>();
        service.AddScoped<IAreaMasterService, AreaMasterService>();

        //FINANCE
	    service.AddScoped<IFinanceYearService,FinanceYearService>();
        service.AddScoped<ICurrencyService, CurrencyService>();

        service.AddScoped<IChartOfAccountsService, ChartOfAccountsService>();
        service.AddScoped<IVoucherService, VoucherService>();
        //PASSWORD
        service.AddScoped<IPasswordService, PasswordService>();
		
		//AccountList
		 service.AddScoped<IAccountList, AccountListService>();

        //Customer supplier
        service.AddScoped<ICustomerSupplierService, CustomerSupplierService>();
        service.AddScoped<ICustomerService, CustomerService>();
        service.AddScoped<ICsDeliveryService, CsDeliveryService>();
        //HR

        service.AddScoped<IHrEmployeeService, HrEmployeeService>();

        //Item
        service.AddScoped<IItemMasterService, ItemMasterService>();

        service.AddScoped<IItemUnitsService, ItemUnitsService>();

       // service.AddScoped<IUnitMasterService, UnitMasterService>();

       // service.AddScoped<ITaxTypeService, TaxTypeService>();

       //TransAdditinaals
       service.AddScoped<ITransactionAdditionalsService, TransactionAdditionalsService>();
    }
}
