using Dfiance.Hr.Employees;
using Dfiance.Hr.Employees.Interface;
using Dfinance.Application;
using Dfinance.Application.Services;
using Dfinance.Application.Services.Finance;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.Application.Services.General;
using Dfinance.Application.Services.General.Interface;
using Dfinance.Application.Services.Interface;
using Dfinance.Application.Services.Inventory;
using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.AuthApplication.Services;
using Dfinance.AuthApplication.Services.Interface;
using Dfinance.AuthAppllication.Services;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.ChartOfAccount.Services.Finance;
using Dfinance.ChartOfAccount.Services.Finance.Interface;
using Dfinance.Item.Services.Inventory;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Shared.Configuration.Service;
using Dfinance.Shared.Deserialize;
using Dfinance.Stakeholder.Services;
using Dfinance.Stakeholder.Services.Interface;
using Serilog.Formatting;
using Dfinance.Warehouse.Services.Interface;
using Dfinance.Warehouse.Services;


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
        service.AddScoped<DataRederToObj>();

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
        service.AddScoped<IFinanceYearService, FinanceYearService>();
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

        //log
       service.AddScoped <ITextFormatter, LogFormatterService>();

        service.AddScoped<IItemUnitsService, ItemUnitsService>();

        // service.AddScoped<IUnitMasterService, UnitMasterService>();

        // service.AddScoped<ITaxTypeService, TaxTypeService>();

        service.AddScoped<ITransactionAdditionalsService, TransactionAdditionalsService>();

        service.AddScoped<IWarehouseService, WarehouseService>();

    }
}