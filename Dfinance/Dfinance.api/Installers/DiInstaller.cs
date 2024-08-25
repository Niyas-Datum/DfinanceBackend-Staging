using Dfiance.Hr.Employees;
using Dfiance.Hr.Employees.Interface;
using Dfinance.Application.LabelAndGridSettings;
//using Dfinance.Application.LabelAndGridSettings.Interface;
//using Dfinance.Application.LabelAndGridSettings;
using Dfinance.Application.LabelAndGridSettings.Interface;
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
using Dfinance.Finance.Masters;
using Dfinance.Finance.Masters.Interface;
using Dfinance.Finance.Services;
using Dfinance.Finance.Services.Interface;
using Dfinance.Finance.Statements;
using Dfinance.Finance.Statements.Interface;
using Dfinance.Finance.Vouchers;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Inventory;
using Dfinance.Inventory.Interface;
using Dfinance.Inventory.Reports;
using Dfinance.Inventory.Reports.Interface;
using Dfinance.Inventory.Service;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Item.Services;
using Dfinance.Item.Services.Interface;
using Dfinance.Item.Services.Inventory;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Purchase.Services;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Restaurant;
using Dfinance.Restaurant.Interface;
using Dfinance.Sales;
using Dfinance.Sales.Service;
using Dfinance.Sales.Service.Interface;
using Dfinance.Shared.Configuration.Service;
using Dfinance.Shared.Deserialize;
using Dfinance.Stakeholder.Services;
using Dfinance.Stakeholder.Services.Interface;
using Dfinance.Stock.Services;
using Dfinance.Warehouse.Services;
using Dfinance.Warehouse.Services.Interface;
using Dfinance.WareHouse.Services;
using Dfinance.WareHouse.Services.Interface;
using Dfinance.WareHouse.Services.Stock;
using Serilog.Formatting;


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
        service.AddScoped<ITextFormatter, LogFormatterService>();

        //itemmaster

        service.AddScoped<IItemMasterService, ItemMasterService>();
        service.AddScoped<IItemUnitsService, ItemUnitsService>();

        service.AddScoped<IUnitMasterService, UnitMasterService>();
        service.AddScoped<IUserTrackService, UserTrackService>();

        // service.AddScoped<ITaxTypeService, TaxTypeService>();

        //service.AddScoped<ITransactionAdditionalsService, TransactionAdditionalsService>();

        service.AddScoped<IWarehouseService, WarehouseService>();
        service.AddScoped<IStockReportService, StockReportService>();

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
        service.AddScoped<ISalesOrder, SalesOrderService>();
        service.AddScoped<IPurchaseReturnService, PurchaseReturnService>();

        service.AddScoped<IFinanceAdditional, FinanceAdditional>();
        service.AddScoped<IFinancePaymentService, FinancePaymentService>();
        service.AddScoped<IFinanceTransactionService, FinanceTransactionService>();
        service.AddScoped<IPaymentVoucherService, PaymentVoucherService>();
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

        //BranchAccounts
        service.AddScoped<IBranchAccounts, BranchAccountsService>();


        //AccountReconciliation
        service.AddScoped<IAccountReconciliationService, AccountReconciliationService>();
        //openingvoucher
        service.AddScoped<IOpeningVoucherService, OpeningVoucherService>();
        //PDC
        service.AddScoped<IPdcClearingService, PdcClearingService>();
        //creditdebitnote
        service.AddScoped<ICreditDebitNoteService, CreditDebitNoteService>();

        //Restaurant
        service.AddScoped<IRestaurantInvoice, RestaurantInvoice>();
        //finance-statements

        //RecallVoucher
        service.AddScoped<IRecallVoucherService, RecallVoucherService>();

        //finance-statements
        service.AddScoped<IFinStatementService, FinStatementService>();
        //AccountConfiguration
        service.AddScoped<IAccountConfigurationService, AccountConfigurationService>();

        service.AddScoped<IPageMenuService, PageMenuService>();

        //counters
        service.AddScoped<ICountersService, CountersService>();

        //ChequeTemplate
        service.AddScoped<IChequeTemplateService, ChequeTemplateService>();

        //JournalVoucher
        service.AddScoped<IJournalVoucherService, JournalVoucherService>();

        //submasters
        service.AddScoped<ISubMastersService, SubMastersService>();

        //ChequeRegister
        service.AddScoped<IChequeRegister, ChequeRegisterService>();
        //CustomerRegister
        service.AddScoped<ICustomerRegister, CustomerRegisterService>();
        //AccountRegister
        service.AddScoped<IAccountRegister, AccountRegisterService>();

        //PhysicalOpeningStock
        service.AddScoped<IPhyOpenStockService, PhyOpenStockService>();

        //StockReturn and Adjustment
        service.AddScoped<IStockRtnAdjustService, StockRtnAdjustService>();

        service.AddScoped<IStockTransactionService, StockTransactionService>();
        //AccountSortOrder
        service.AddScoped<IAccountSortOrder, AccountSortOrderService>();
        //InventoryRegister
        service.AddScoped<IInventoryRegister,InventoryRegisterService>();
        //InventoryApproval
        service.AddScoped<IInventoryApproval, InventoryApprovalService>();

        //InternationalBarCode
        service.AddScoped<IInternationalBarCodeService, InternationalBarCodeService>();


        //BatchEdit
        service.AddScoped<IBatchEditService, BatchEditService>();
        service.AddScoped<IItemReservationService, ItemReservationService>();

        service.AddScoped<ISizeMasterService, SizeMasterService>();


        service.AddScoped<ISizeMasterService, SizeMasterService>();

        service.AddScoped<IItemMappingService, ItemMappingService>();


        //PriceCategory
        service.AddScoped<IPriceCategoryService, PriceCategoryService>();
        //CloseVoucher
        service.AddScoped<ICloseVoucherService, CloseVoucherService>();
        //SalesPos
        service.AddScoped<ISalesPosService, SalesPosService>();

        service.AddScoped<ISalesEnquiryService, SalesEnquiryService>();

        service.AddScoped<ISalesEstimateService, SalesEstimateService>();
        service.AddScoped<ISalesQuotationService, SalesQuotationService>();
        service.AddScoped<IDeliveryOutService, DeliveryOutService>();


        //DosageMaster
        service.AddScoped<IDosageMasterService, DosageMasterService>();


        service.AddScoped<IPurchaseWithoutTaxService, PurchaseWithoutTaxService>();
        service.AddScoped<ISalesB2BService, SalesB2BService>();
        service.AddScoped<ISalesB2CService, SalesB2CService>();
        service.AddScoped<IDocumentTypeService, DocumentTypeService>();
        //FinanceRegister
        service.AddScoped<IFinanceRegister,FinanceRegisterService>();


        //QualityType
        service.AddScoped<IQualityTypeService, QualityTypeService>();


        service.AddScoped<ITaxTypeService, TaxTypeService>();
        service.AddScoped<ITaxCategoryService, TaxCategoryService>();
    }
}
