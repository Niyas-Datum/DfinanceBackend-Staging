using Dfiance.Hr.Employees;
using Dfiance.Hr.Employees.Interface;
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
using Dfinance.Inventory.Service.Interface;
using Dfinance.Inventory.Service;
//using Dfinance.Application.LabelAndGridSettings.Interface;
//using Dfinance.Application.LabelAndGridSettings;
using Dfinance.Application.LabelAndGridSettings.Interface;
using Dfinance.Application.LabelAndGridSettings;

using Dfinance.Inventory.Interface;
using Dfinance.Inventory;
using Dfinance.Sales;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Purchase.Services;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Finance.Vouchers;
using Dfinance.Finance.Services.Interface;
using Dfinance.Finance.Services;
using Dfinance.WareHouse.Services.Interface;
using Dfinance.WareHouse.Services;
using Dfinance.Finance.Statements.Interface;
using Dfinance.Finance.Statements;


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
		service.AddScoped<ICardMaster, CardMasterService>();

        service.AddScoped<IChartOfAccountsService, ChartOfAccountsService>();
        service.AddScoped<IVoucherService, VoucherService>();
        //PASSWORD
        service.AddScoped<IPasswordService, PasswordService>();

        //AccountList
        service.AddScoped<IAccountListService, AccountListService>();

        //Customer supplier
        service.AddScoped<ICustomerSupplierService, CustomerSupplierService>();
        service.AddScoped<ICustomerService, CustomerService>();
        service.AddScoped<ICsDeliveryService, CsDeliveryService>();
        //HR

        service.AddScoped<IHrEmployeeService, HrEmployeeService>();

        //log
       service.AddScoped <ITextFormatter, LogFormatterService>();

        //itemmaster

        service.AddScoped<IItemMasterService, ItemMasterService>();
        service.AddScoped<IItemUnitsService, ItemUnitsService>();

        service.AddScoped<IUnitMasterService, UnitMasterService>();
        service.AddScoped<IUserTrackService, UserTrackService>();

        // service.AddScoped<ITaxTypeService, TaxTypeService>();

        //service.AddScoped<ITransactionAdditionalsService, TransactionAdditionalsService>();

        service.AddScoped<IWarehouseService, WarehouseService>();

        //Roles
        service.AddScoped<IRoleService, RoleService>();
 //label&Grid
        service.AddScoped<ILabelAndGridSettings, LabelAndGridSettings>();
		
		 //commonservice in inventory
        service.AddScoped<Dfinance.Inventory.CommonService>();
        	 //purchase
      
        service.AddScoped<IInventoryTransactionService, InventoryTransactionService>();
        service.AddScoped<IInventoryAdditional, InventoryAdditional>();
        service.AddScoped<IInventoryItemService, InventoryItemservice>();
        service.AddScoped<IInventoryPaymentService, InventoryPaymentService>();       
        service.AddScoped<IPurchaseService, PurchaseService>();
        service.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
		 service.AddScoped<ISalesInvoiceService, SalesInvoiceService>();
		 service.AddScoped<IPurchaseEnquiryService, PurchaseEnquiryService>();
        service.AddScoped<IPurchaseRequestService, PurchaseRequestService>();
        service.AddScoped<IPurchaseQuotationService, PurchaseQuotationService>();
        service.AddScoped<IInternationalPurchaseService, InternationalPurchaseService>();
        service.AddScoped<IGoodsInTransitService, GoodsInTransitService>();
		service.AddScoped<ISalesReturnService, SalesReturnService>();

        service.AddScoped<IPurchaseReturnService, PurchaseReturnService>();

        service.AddScoped<IFinanceAdditional, FinanceAdditional>();
        service.AddScoped<IFinancePaymentService, FinancePaymentService>();
        service.AddScoped<IFinanceTransactionService, FinanceTransactionService>();
        service.AddScoped<IPaymentVoucherService,PaymentVoucherService>();
        service.AddScoped<IReceiptVoucherService, ReceiptVoucherService>();
        
        //contravou
        service.AddScoped<IContraVoucherService, ContraVoucherService>();
	service.AddScoped<IMaterialRequestService, MaterialRequestService>();
        service.AddScoped<IDeliveryInService, DeliveryInService>();
        service.AddScoped<IMaterialReceiptService, MaterialReceiptService>();
        service.AddScoped<IInvMatTransService, InvMatTransService>();

        service.AddScoped<IBudgetingService, BudgetingService>();
        service.AddScoped<IBudgetRegisterService, BudgetRegisterService>();
        service.AddScoped<IBudgetMonthwiseService, BudgetMonthwiseService>();
        service.AddScoped<IDayBookService, DayBookService>();
        //BranchAccounts
        service.AddScoped<IBranchAccounts, BranchAccountsService>();

        //AccountReconciliation
        service.AddScoped<IAccountReconciliationService, AccountReconciliationService>();
        //openingvoucher
        service.AddScoped<IOpeningVoucherService, OpeningVoucherService>();
        //PDC
        service.AddScoped<IPdcClearingService,PdcClearingService>();
        //creditdebitnote
        service.AddScoped<ICreditDebitNoteService, CreditDebitNoteService>();
    }
}
