using Microsoft.EntityFrameworkCore;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure.Configurations;
using Dfinance.Core.Views;
using Dfinance.Core.Views.General;
using Dfinance.Core.Domain.Roles;
using Dfinance.Core.Infrastructure.Configurations.Employee;
using Dfinance.Core.Infrastructure.Configurations.Roles;
using Dfinance.Core.Views.Inventory;
using Dfinance.Core.Views.PagePermission;
using Dfinance.Core.Views.Common;
using Dfinance.Shared.Configuration.Service;
using Dfinance.Core.Views.Finance;
using Dfinance.Core.Views.Inventory.Purchase;
using Dfinance.Core.Views.Item;
using System.Reflection.Emit;
using System;

namespace Dfinance.Core.Infrastructure;


public partial class DFCoreContext : DbContext
{
    private string con;
    public DFCoreContext()
    {


    }

    private IConnectionServices _connectionServices;
    public DFCoreContext(DbContextOptions<DFCoreContext> options,
        IConnectionServices connectionServices) : base(options)
    {
        _connectionServices = connectionServices;
        con = _connectionServices.getcon();
    }

    public DbSet<ItemMultiRate> ItemMultiRate { get; set; }
    public DbSet<BranchItems> BranchItems { get; set; }
    public DbSet<TaxType> TaxType { get; set; }
    public DbSet<UnitMaster> UnitMaster { get; set; }
    public DbSet<ItemUnits> ItemUnits { get; set; }
    public DbSet<ItemMaster> ItemMaster { get; set; }
    public DbSet<MaEmployee> MaEmployees { get; set; }
    public DbSet<MaEmployeeDetail> MaEmployeeBranchDet { get; set; }
    //public DbSet<MaRoles> UserRoles { get; set; }
    public DbSet<MaUserRight> UserRolePermission { get; set; }
    public DbSet<MaPageMenu> MaPageMenus { get; set; }

    public DbSet<LogInfo> LogInfo { get; set; }
    public DbSet<MaCompany> MaBranches { get; set; }

    public DbSet<FiMaAccount> FiMaAccounts { get; set; }
    public DbSet<FiMaSubGroup> FiMaSubGroups { get; set; }
    public DbSet<FiMaAccountGroup> FiMaAccountGroup { get; set; }

    public DbSet<MaDepartment> MaDepartments { get; set; }
    public DbSet<ReDepartmentType> ReDepartmentTypes { get; set; }
    public DbSet<MaMisc> MaMisc { get; set; }
    public DbSet<MaMiscKeys> MaMiscKeys { get; set; }
    public DbSet<MaDesignation> MaDesignations { get; set; }
    public DbSet<CostCategory> CostCategory { get; set; }
    public DbSet<CostCentre> CostCentre { get; set; }
    public DbSet<Categories> Category { get; set; }
    public DbSet<MaArea> MaArea { get; set; }
    public DbSet<FiMaAccountCategory> FiMaAccountCategory { get; set; }
    public DbSet<FiMaBranchAccounts> FiMaBranchAccounts { get; set; }
    public DbSet<CategoryType> CategoryType { get; set; }
    //General(Perties(C&S)
    public DbSet<Parties> Parties { get; set; }
    public DbSet<FillPartyView> FillPartyView { get; set; }
    public DbSet<FillSideBar> FillSideBars { get; set; }
    public DbSet<FillPartyById> FillPartyById { get; set; }
    public DbSet<FillCustdetails> FillCustdetails { get; set; }
    public DbSet<FillDeldetails> FillDeldetails { get; set; }


    //MaSettings
    public DbSet<MaSettings> MaSettings { get; set; }

    //Fin=>Currency
    public DbSet<Currency> Currency { get; set; }
    public DbSet<CurrencyCode> CurrencyCode { get; set; }
    //Fin=>AccountsList
    public DbSet<FiAccountsList> FiAccountsList { get; set; }
    public DbSet<FiMaAccountsList> FiMaAccountsLists { get; set; }
    public DbSet<TblMaFinYear> TblMaFinYear { get; set; }
    public DbSet<MaCustomerDetails> MaCustomerDetails { get; set; }
    public DbSet<MaCustomerCategories> MaCustomerCategories { get; set; }
    public DbSet<MaCustomerItems> MaCustomerItems { get; set; }
    public DbSet<FiMaCards> FiMaCards { get; set; }
    public DbSet<DeliveryDetails> DeliveryDetails { get; set; }
    public DbSet<MaPriceCategory> MaPriceCategory { get; set; }

    //Inv=>Warehouse
    public DbSet<Locations> Locations { get; set; }
    public DbSet<LocationTypes> LocationTypes { get; set; }
    public DbSet<LocationBranchList> LocationBranchList { get; set; }

    //Transaction
    public DbSet<FiTransaction> FiTransaction { get; set; }
    public DbSet<FiTransactionAdditionals> FiTransactionAdditionals { get; set; }
    public DbSet<TransReference> TransReference { get; set; }
    //InvTransItems
    public DbSet<InvTransItems> InvTransItems { get; set; }
    public DbSet<InvAvgCost> InvAvgCost { get; set; }
    public DbSet<InvUniqueItems> InvUniqueItems { get; set; }
    // public DbSet<InvBatchWiseItem> InvBatchWiseItem { get; set; }

    //MaVehicle
    public DbSet<MaVehicles> MaVehicles { get; set; }

    public DbSet<FiTransactionEntry> FiTransactionEntries { get; set; }
    public DbSet<FiVoucherAllocation> FiVoucherAllocation { get; set; }
    public DbSet<TransExpense> TransExpense { get; set; }
    public DbSet<InvSizeMaster> InvSizeMaster { get; set; }
    public DbSet<TransItemExpense> TransItemExpenses { get; set; }
    public DbSet<BudgetMonth> BudgetMonth { get; set; }






    //view init

    // read- id, code name
    public DbSet<ReadView> ReadView { get; set; }
    // read- id, code description
    public DbSet<ReadViewDesc> ReadViewDesc { get; set; }
    //nextCodeView id,descripition
    //read category id,code,category,categorytype
    public DbSet<CategoryPopupView> CategoryPopupView { get; set; }

    //nextcode in cat type

    public DbSet<NextCodeCat> NextCodeCat { get; set; }
    public DbSet<NextCategoryCode> NextCategoryCode { get; set; }
    public DbSet<DropDownViewDesc> DropDownViewDesc { get; set; }

    public DbSet<SpFillCategoryTypeById> SpFillCategoryTypeById { get; set; }
    public DbSet<NextCodeView> NextCodeView { get; set; }
    public DbSet<DropDownViewValue> DropDownViewValue { get; set; }
    public DbSet<DropDownViewName> DropDownViewName { get; set; }
    public DbSet<SpFillAreaMasterByIdG> SpFillAreaMasterByIdG { get; set; }
    public DbSet<SpFillAreaMasterG> SpFillAreaMasterG { get; set; }
    public DbSet<SpFillCategoryByIdG> SpFillCategoryByIdG { get; set; }
    public DbSet<SpFillCategoryG> SpFillCategoryG { get; set; }
    public DbSet<SpFillCostCentreByIdG> SpFillCostCentreByIdG { get; set; }
    public DbSet<SpFillCostCentreG> SpFillCostCentreG { get; set; }
    public DbSet<SpFillCostCategoryByIdG> SpFillCostCategoryByIdG { get; set; }
    public DbSet<SpFillCostCategoryG> SpFillCostCategoryG { get; set; }
    public DbSet<SpFillAllBranchByIdG> SpFillAllBranchByIdG { get; set; }
    public DbSet<SpFillAllBranchG> SpFillAllBranch { get; set; }
    public DbSet<SpFillEmployees> SpFillEmployees { get; set; }
    public DbSet<SpMacompanyFillallbranch> SpMacompanyFillallbranch { get; set; }
    public DbSet<SpReDepartmentTypeFillAllDepartment> SpReDepartmentTypeFillAllDepartment { get; set; }
    public DbSet<spMaDepartmentsFillDepartmentById> spMaDepartmentsFillDepartmentById { get; set; }
    public DbSet<spDepartmentTypesFillAllDepartmentTypes> spDepartmentTypesFillAllDepartmentTypes { get; set; }
    public DbSet<SpUserGById> SpUserGById { get; set; }

    public DbSet<SpDesignationMasterG> SpFillDesignationMaster { get; set; }
    public DbSet<SpDesignationMasterByIdG> SpDesignationMasterByIdG { get; set; }
    public DbSet<spDepartmentTypesGetById> spDepartmentTypesGetById { get; set; }
    public DbSet<spMaDepartmentsFillAllDepartment> spMaDepartmentsFillAllDepartment { get; set; }
    public DbSet<UserInfo> UserInfo { get; set; }
    public DbSet<SpUser> SpUser { get; set; }
    public DbSet<RoleRightsModel> RoleRightsModel { get; set; }
    public DbSet<GetRole> GetRole { get; set; }
    public DbSet<UserPageListView> UserPageListView { get; set; }
    //Currency 
    public DbSet<FillcurrencyCode> FillcurrencyCode { get; set; }
    public DbSet<FillCurrencyCodeById> FillCurrencyCodeById { get; set; }
    public DbSet<FillCurrency> FillCurrency { get; set; }
    public DbSet<FillCurrencyById> FillCurrencyById { get; set; }
    public DbSet<FillCardMaster> FillCardMaster { get; set; }
    public DbSet<FillMaster> FillMaster { get; set; }
    //-------------ItemMaster  ------------------------------------------ 

    public DbSet<TaxDropDownView> TaxDropDownView { get; set; }
    public DbSet<SpFillItemMasterById> SpFillItemMasterById { get; set; }
    public DbSet<SpFillItemMaster> SpFillItemMaster { get; set; }
    public DbSet<ItemNextCode> ItemNextCode { get; set; }
    public DbSet<ParentItemPoupView> ParentItemPoupView { get; set; }
    public DbSet<BarcodeView> BarcodeView { get; set; }
    public DbSet<ItemHistoryView> ItemHistoryView { get; set; }
    public DbSet<CurrentStockView> CurrentStockView { get; set; }
    //FinanceYear
    public DbSet<FinanceYearView> FinanceYearView { get; set; }
    public DbSet<FinanceYearViewByID> FinanceYearViewByID { get; set; }
    //FiMaAccounts
    public DbSet<ChartofAccView> ChartofAccView { get; set; }
    public DbSet<FillLedgers> FillLedgers { get; set; }
    public DbSet<ChartofAccViewById> ChartofAccViewById { get; set; }
    public DbSet<AccountCodeView> AccountCodeView { get; set; }
    //AccountList

    public DbSet<FillAccountList> FillAccountList { get; set; }
    public DbSet<ReadViewAlias> ReadViewAlias { get; set; }

    //Vouchers
    public DbSet<FillVoucherView> FillVoucherView { get; set; }
    public DbSet<FillMaVouchersUsingPageIDView> FillMaVouchersUsingPageIDView { get; set; }
    public DbSet<FillVoucherWithTrnasId> FillVoucherWithTrnasId { get; set; }


    public DbSet<Voucher> FiMaVouchers { get; set; }
    public DbSet<NameView> NameView { get; set; }  //  retun only name

    //MaNumbering
    public DbSet<MaNumbering> MaNumbering { get; set; }
    //UserTrack
    public DbSet<UserTrack> UserTrack { get; set; }
    public DbSet<UserTrackView> UserTrackView { get; set; }
    //password 
    public DbSet<PasswordCheckResult> PasswordCheckResult { get; set; }
    //SettingView 
    public DbSet<FillSettingById> FillSettingById { get; set; }
    //----------------ItemUnits------------------------------------------------     
    public DbSet<FillItemUnitsView> FillItemUnitsView { get; set; }
    public DbSet<TransItemUnits> TransItemUnits { get; set; }


    //--------------UnitMaster----------------------------------------
    public DbSet<UnitPopupView> UnitPopupView { get; set; }
    //fillcustomeritem=>Customer&supplier
    public DbSet<FillCustomeritem> FillCustomeritem { get; set; }
    public DbSet<CrdtCollView> CrdtCollView { get; set; }
    //UnitMaster
    //public DbSet<UnitMaster> UnitMaster { get; set; }
    public DbSet<SpFillUnitMaster> SpFillUnitMaster { get; set; }
    public DbSet<SpFillByUnit> SpFillByUnit { get; set; }
    public DbSet<DropDownView> DropDownView { get; set; }
    //------------------------------------------------------------------
    public DbSet<FillSetting> FillSetting { get; set; }



    //Fillwarehouse=>Warehousemaster
    public DbSet<WareHouseView> WareHouseView { get; set; }
    //public DbSet<WarehouseBranchView> WarehouseBranchView { get; set; }
    public DbSet<Warehousebranchfill> Warehousebranchfill { get; set; }


    //FitransactionAdditonals
    public DbSet<SpGetTransactionAdditionals> SpGetTransactionAdditionals { get; set; }
    public DbSet<ReferenceView> ReferenceView { get; set; }
    public DbSet<RefItemsView> RefItemsView { get; set; }
    public DbSet<ImportItemListView> ImportItemListView { get; set; }

    public DbSet<NextBatchNoView> NextBatchNoView { get; set; }
    public DbSet<TransItemsView> TransItemsView { get; set; }
    public DbSet<CommandTextView> CommandTextView { get; set; }
    public DbSet<ItemTransaction> ItemTransaction { get; set; }
    public DbSet<DropDownViewIsdeft> DropDownViewIsdeft { get; set; }

    //purchase
    public DbSet<FillAdvanceView> FillAdvanceView { get; set; }
    public DbSet<Fillvoucherview> Fillvoucherview { get; set; }
    public DbSet<PriceCategoryPopUp> PriceCategoryPopUp { get; set; }

    //Roles

    public DbSet<MaRoles> UserRoles { get; set; }
    public DbSet<MaRoleRight> MaRoleRights { get; set; }

    //Cheque
    public DbSet<FiCheques> fiCheques { get; set; }
    //FimaUniqueAccounts
    public DbSet<FimaUniqueAccount> FimaUniqueAccount { get; set; }
    //ChargeType
    public DbSet<MaChargeType> MaChargeType { get; set; }
    public DbSet<TransCollection> TransCollections { get; set; }
    public DbSet<TransCollnAllocation> TransCollnAllocations { get; set; }
    public DbSet<FillRole> FillRole { get; set; }
    public DbSet<FillRoleRight> FillRoleRight { get; set; }
    public DbSet<SpFillRoles> SpFillRoles { get; set; }
    //Grid and Label
    public DbSet<FormGridSetting> FormGridSettings { get; set; }
    public DbSet<FormLabelSetting> FormLabelSettings { get; set; }
    //Label&Grid
    public DbSet<FormLabelView> FormLabelView { get; set; }
    public DbSet<FormGridView> FormGridView { get; set; }
    //voucher in finance
    public DbSet<AccountCodesView> AccountCodesView { get; set; }
    public DbSet<ContraAccCode> ContraAccCode { get; set; }
    
    //ReportsView
    public DbSet<PurchaseReportView> PurchaseReportView { get; set; }
    public DbSet<PurchaseReportViews> PurchaseReportViews { get; set; }
    public DbSet<ItemSearchView> ItemSearchView { get; set; }
    public DbSet<QtyView> QtyView { get; set; }

    //Receiptvoucher
    public DbSet<VoucherView> VoucherView { get; set; }
    public DbSet<FillVoucher> FillVoucher { get; set; }
    public DbSet<FillVouTranId> FillVouTranId { get; set; }


    //budgeting

    public DbSet<BudgetRegPandLView> BudRegProfitAndLoss { get; set; }
    public DbSet<BalanceSheetView> BalanceSheetView { get; set; }
    public DbSet<MonthwisePandLView> MonthwisePandLView { get; set; }
    public DbSet<MonthwiseBalSheetView> MonthwiseBalSheetView { get; set; }

    //daybook
    public DbSet<DayBookView> DayBookView { get; set; }
    //finance-statements
    public DbSet<TrialBalanceView> TrialBalanceView { get; set; }
    public DbSet<CashBankBookView> CashBankBookView { get; set; }
    public DbSet<AccStatementView> AccStatementView { get; set; }
    public DbSet<GroupStatementView> GroupStatementView { get; set; }
    public DbSet<BillwiseStatementView> BillwiseStatementView { get; set; }
    public DbSet<BalSheetView3> BalSheetView3 { get; set; }
    public DbSet<ConsolMonthwiseView> ConsolMonthwiseView { get; set; }
    public DbSet<PaymentAnalysisView> PaymentAnalysisView { get; set; }
    public DbSet<PartyOutstandingView> PartyOutstandingView { get; set; }
    public DbSet<DebitCreditView> DebitCreditView { get; set; }
    public DbSet<ProfitAndLossView3> ProfitAndLossView3 { get; set; }
    public DbSet<eReturnView> eReturnView { get; set; }
   


    //itemregister report
    public DbSet<ItemCatalogueView> ItemCatalogueView { get; set; }
    public DbSet<ItemCatalogueViews> ItemCatalogueViews { get; set; }


    //inventoryAgen
    public DbSet<InventoryAgeingView> InventoryAgeingView {  get; set; }    
    public DbSet<InventoryAgeingViews> InventoryAgeingViews {  get; set; }
    public DbSet<ItemExpiryReportView> ItemExpiryReportView {  get; set; }      

    //HR
    public DbSet<HREmployee> Hremployees { get; set; }

    public DbSet<InventoryTransactionsView> InventoryTransactionsView { get; set; }
    public DbSet<MonthlyInvSummaryView> MonthlyInvSummaryView { get; set; }

    //Restaurent
    public DbSet<CommodityView> CommodityViews { get; set; }
    public DbSet<TableView> TableViews { get; set; }
    public DbSet<KitchenCategoryView> KitchenCategoryViews { get; set; }
    public DbSet<PrintKotView> PrintKotViews { get; set; }
    public DbSet<ProductVew> ProductVews { get; set; }

    //RecallVoucher
    public DbSet<RecallVoucherView> RecallVoucherViews { get; set; }
    public DbSet<InventoryProfitItemView> InventoryProfitItemView { get; set; }
    public DbSet<InventoryProfitVoucherView> InventoryProfitVoucherView { get; set; }
    public DbSet<InventoryProfitVoucherViews> InventoryProfitVoucherViews { get; set; }
    public DbSet<InventoryProfitPartyView> InventoryProfitPartyView { get; set; }
    public DbSet<InventoryProfitPartyViews> InventoryProfitPartyViews { get; set; }
    public DbSet<ItemsHistoryReportView> ItemsHistoryReportView { get; set; }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //  => optionsBuilder.UseSqlServer(@"Data Source=ip.datuminnovation.com,9600;TrustServerCertificate=true;Initial Catalog=DatumSystemMain;User ID=sa;pwd=Datum123!");
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

    => optionsBuilder.UseSqlServer(con);

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.ApplyConfiguration(new UnitMasterConfiguration());
        mb.ApplyConfiguration(new TaxTypeConfiguration());
        mb.ApplyConfiguration(new ItemMultiRateConfiguration());
        mb.ApplyConfiguration(new PriceCategoryConfiguration());
        mb.ApplyConfiguration(new BranchItemsConfiguration());
        mb.ApplyConfiguration(new ItemUnitsConfiguration());
        mb.ApplyConfiguration(new ItemMasterConfiguration());
        mb.ApplyConfiguration(new MaEmployeeConfiguration());
        mb.ApplyConfiguration(new MaCompanyConfiguration());
        mb.ApplyConfiguration(new MaDepartmentConfiguration());
        mb.ApplyConfiguration(new MaMiscConfiguration());
        mb.ApplyConfiguration(new CostCategoryConfiguration());
        mb.ApplyConfiguration(new MaEmployeeDetailConfiguration());
        mb.ApplyConfiguration(new MaRolesConfiguration());
        mb.ApplyConfiguration(new CurrencyCodeConfigurations());
        //  mb.ApplyConfiguration(new MaRoleRightConfiguration());
        mb.ApplyConfiguration(new MaUserRightConfiguration());
        mb.ApplyConfiguration(new MaPageMenuConfiguration());
        mb.ApplyConfiguration(new LogInfoConfiguration());
        mb.ApplyConfiguration(new FiMaAccountConfiguration());
        mb.ApplyConfiguration(new FiMaSubGroupConfiguration());
        mb.ApplyConfiguration(new FiMaAccountGroupConfiguration());
        mb.ApplyConfiguration(new FiMaAccountCategoryConfiguration());
        mb.ApplyConfiguration(new FiMaBranchAccountsConfiguration());
        mb.ApplyConfiguration(new CategoryConfiguration());
        mb.ApplyConfiguration(new CategoryTypeConfiguration());
        mb.ApplyConfiguration(new FiMaVouchersConfiguration());
        mb.ApplyConfiguration(new FiVoucherAllocationsConfiguration());
        //settings
        mb.ApplyConfiguration(new MaSettingsConfiguration());
        //currency 
        mb.ApplyConfiguration(new CurrencyConfigurations());
        //Financeyear
        mb.ApplyConfiguration(new TblMaFinYearConfigurations());
        //AccountList
        mb.ApplyConfiguration(new FiAccountsListConfiguration());
        mb.ApplyConfiguration(new FiMaAccountsListConfiguration());

        mb.ApplyConfiguration(new FiMaCardsConfiguration());
        //InvTransItems
        mb.ApplyConfiguration(new InvTransItemConfiguration());
        mb.ApplyConfiguration(new InvUniqueItemConfiguration());
        //  mb.ApplyConfiguration(new InvBatwiseItemConfiguration());
        //Warehouse
        mb.ApplyConfiguration(new LocationsConfiguration());
        mb.ApplyConfiguration(new LocationTypesConfiguration());
        mb.ApplyConfiguration(new LocationBranchListConfiguration());
        //usertrack
        mb.ApplyConfiguration(new UserTrackConfiguration());
        //warehouse
        mb.Entity<Warehousebranchfill>().HasNoKey().ToView(null);
        mb.Entity<WareHouseView>().HasNoKey().ToView(null);

        //TransactionAddition
        mb.ApplyConfiguration(new FiTransactionAdditionalConfiguration());
        mb.ApplyConfiguration(new FiTransactionConfiguration());
        mb.ApplyConfiguration(new TransReferenceConfiguration());

        mb.ApplyConfiguration(new FimaUniqueAccountConfiguration());
        mb.ApplyConfiguration(new MaChargeTypeConfiguration());

        //TransExpense
        mb.ApplyConfiguration(new TransExpensesConfiguration());

        //TransactionEntries
        mb.ApplyConfiguration(new FiTransactionEntryConfiguration());
        mb.ApplyConfiguration(new FiTransactionEntryConfiguration());

        mb.ApplyConfiguration(new InvSizeMasterConfiguration());
        mb.ApplyConfiguration(new BudgetMonthConfiguration());
        //HR
        mb.ApplyConfiguration(new MaEmployeeConfiguration());
        mb.ApplyConfiguration(new MaEmployeeDetailConfiguration());
        mb.ApplyConfiguration(new HREmployeeConfiguration());


        //View

        mb.Entity<CurrencyCode>().HasNoKey().ToView(null);
        mb.Entity<UserPageListView>().HasNoKey().ToView(null);
        mb.Entity<UserInfo>().HasNoKey().ToView(null);
        mb.Entity<SpUser>().HasNoKey().ToView(null);


        mb.Entity<spDepartmentTypesFillAllDepartmentTypes>().HasNoKey().ToView(null);

        mb.Entity<RoleRightsModel>().HasNoKey().ToView(null);
        mb.Entity<spDepartmentTypesFillAllDepartmentTypes>().HasNoKey().ToView(null);

        mb.Entity<spMaDepartmentsFillDepartmentById>().HasNoKey().ToView(null);
        mb.Entity<spMaDepartmentsFillAllDepartment>().HasNoKey().ToView(null);
        mb.Entity<SpMacompanyFillallbranch>().HasNoKey().ToView(null);
        mb.Entity<SpFillEmployees>().HasNoKey().ToView(null);
        mb.Entity<SpReDepartmentTypeFillAllDepartment>().HasNoKey().ToView(null);
        mb.Entity<SpUserGById>().HasNoKey().ToView(null);
        mb.Entity<spDepartmentTypesGetById>().HasNoKey().ToView(null);
        mb.Entity<SpFillAllBranchByIdG>().HasNoKey().ToView(null);
        mb.Entity<SpFillAllBranchG>().HasNoKey().ToView(null);
        mb.Entity<SpFillCostCategoryG>().HasNoKey().ToView(null);
        mb.Entity<SpFillCostCategoryByIdG>().HasNoKey().ToView(null);
        mb.Entity<SpFillCostCentreG>().HasNoKey().ToView(null);
        mb.Entity<SpFillCostCentreByIdG>().HasNoKey().ToView(null);
        mb.Entity<SpFillCategoryG>().HasNoKey().ToView(null);
        mb.Entity<SpFillCategoryByIdG>().HasNoKey().ToView(null);
        mb.Entity<SpFillAreaMasterG>().HasNoKey().ToView(null);
        mb.Entity<SpFillAreaMasterByIdG>().HasNoKey().ToView(null);
        mb.Entity<NextCodeView>().HasNoKey().ToView(null);
        mb.Entity<SpFillCategoryTypeById>().HasNoKey().ToView(null);
        mb.Entity<NextCodeCat>().HasNoKey().ToView(null);
        mb.Entity<NextCategoryCode>().HasNoKey().ToView(null);


        mb.Entity<SpGetTransactionAdditionals>().HasNoKey().ToView(null);

        /*-----------------ItemMaster--------------------*/

        mb.Entity<SpFillItemMasterById>().HasNoKey().ToView(null);
        mb.Entity<SpFillItemMaster>().HasNoKey().ToView(null);
        mb.Entity<ItemNextCode>().HasNoKey().ToView(null);
        mb.Entity<ParentItemPoupView>().HasNoKey().ToView(null);
        mb.Entity<BarcodeView>().HasNoKey().ToView(null);
        mb.Entity<ItemHistoryView>().HasNoKey().ToView(null);
        mb.Entity<CurrentStockView>().HasNoKey().ToView(null);

        //----------------ItemUnits-------------------------
        mb.Entity<FillItemUnitsView>().HasNoKey().ToView(null);


        /*-----------------common------------------------------*/
        //dropdown data:id,value
        mb.Entity<DropDownViewValue>().HasNoKey().ToView(null);
        //dropdown data:id,description
        mb.Entity<DropDownViewDesc>().HasNoKey().ToView(null);
        //dropdown data:id,name
        mb.Entity<DropDownViewName>().HasNoKey().ToView(null);
        //read data : id, code name
        mb.Entity<ReadView>().HasNoKey().ToView(null);
        //read data : id, code description
        mb.Entity<ReadViewDesc>().HasNoKey().ToView(null);
        mb.Entity<DropDownViewDesc>().HasNoKey().ToView(null);
        //Currency 
        mb.Entity<FillcurrencyCode>().HasNoKey().ToView(null);
        mb.Entity<FillCurrencyCodeById>().HasNoKey().ToView(null);
        mb.Entity<FillCurrency>().HasNoKey().ToView(null);
        mb.Entity<FillCurrencyById>().HasNoKey().ToView(null);
        //FinanceYear
        mb.Entity<FinanceYearView>().HasNoKey().ToView(null);
        mb.Entity<FinanceYearViewByID>().HasNoKey().ToView(null);
        mb.Entity<ChartofAccView>().HasNoKey().ToView(null);
        mb.Entity<FillLedgers>().HasNoKey().ToView(null);
        mb.Entity<AccountCodeView>().HasNoKey().ToView(null);
        mb.Entity<ChartofAccViewById>().HasNoKey().ToView(null);
        //Voucher
        mb.Entity<FillVoucherView>().HasNoKey().ToView(null);

        mb.Entity<NameView>().HasNoKey().ToView(null);
        mb.Entity<FillSettingById>().HasNoKey().ToView(null);
        mb.Entity<FillSetting>().HasNoKey().ToView(null);
        //Password
        mb.Entity<PasswordCheckResult>().HasNoKey().ToView(null);
        mb.Entity<FillAccountList>().HasNoKey().ToView(null);
        //Customer Supplier
        mb.ApplyConfiguration(new PartiesConfiguration());
        mb.ApplyConfiguration(new MaCustomerCategoresConfiguration());
        mb.ApplyConfiguration(new MaCustomerDetailsConfiguration());
        mb.ApplyConfiguration(new DeliveryDetailsConfiguration());

        mb.Entity<TransItemsView>().HasNoKey().ToView(null);
        mb.Entity<CommandTextView>().HasNoKey().ToView(null);
        //Role
        mb.Entity<SpFillRoles>().HasNoKey().ToView(null);
        mb.Entity<FillRoleRight>().HasNoKey().ToView(null);
        mb.Entity<SpFillRoles>().HasNoKey().ToView(null);

        //UnitMaster
        mb.Entity<SpFillUnitMaster>().HasNoKey().ToView(null);
        mb.Entity<SpFillByUnit>().HasNoKey().ToView(null);
        mb.Entity<DropDownView>().HasNoKey().ToView(null);
        //UserTrack
        mb.Entity<UserTrackView>().HasNoKey().ToView(null);
        mb.Entity<FillAdvanceView>().HasNoKey().ToView(null);
        mb.Entity<Fillvoucherview>().HasNoKey().ToView(null);
        mb.Entity<FillMaVouchersUsingPageIDView>().HasNoKey().ToView(null);
        mb.Entity<FillVoucherWithTrnasId>().HasNoKey().ToView(null);


        mb.Entity<FillCardMaster>().HasNoKey().ToView(null);
        mb.Entity<FillMaster>().HasNoKey().ToView(null);
        //TransItems


        mb.Entity<TransItemsView>().HasNoKey().ToView(null);
        mb.Entity<CommandTextView>().HasNoKey().ToView(null);
        mb.Entity<NextBatchNoView>().HasNoKey().ToView(null);
        mb.Entity<ItemTransaction>().HasNoKey().ToView(null);
        mb.ApplyConfiguration(new TransCollectionConfiguration());
        mb.ApplyConfiguration(new TransCollnAllocationConfiguration());
        mb.ApplyConfiguration(new MaChargeTypeConfiguration());
        mb.ApplyConfiguration(new FimaUniqueAccountConfiguration());
        mb.Entity<ReferenceView>().HasNoKey().ToView(null);
        mb.Entity<DropDownViewIsdeft>().HasNoKey().ToView(null);
        mb.Entity<FillPartyView>().HasNoKey().ToView(null);//inventory=>Customer/supplier dropdown
        mb.Entity<FillPartyById>().HasNoKey().ToView(null);
        mb.Entity<FillCustdetails>().HasNoKey().ToView(null);
        mb.Entity<FillDeldetails>().HasNoKey().ToView(null);
        mb.Entity<CrdtCollView>().HasNoKey().ToView(null);
        mb.Entity<PriceCategoryPopUp>().HasNoKey().ToView(null);
        mb.Entity<RefItemsView>().HasNoKey().ToView(null);
        mb.Entity<PurchaseReportView>().HasNoKey().ToView(null);
        mb.Entity<PurchaseReportViews>().HasNoKey().ToView(null);
        mb.Entity<ItemSearchView>().HasNoKey().ToView(null);
        mb.Entity<ItemCatalogueView>().HasNoKey().ToView(null);
        mb.Entity<ItemCatalogueViews>().HasNoKey().ToView(null);
        mb.Entity<InventoryAgeingView>().HasNoKey().ToView(null);
        mb.Entity<InventoryAgeingViews>().HasNoKey().ToView(null);
        mb.Entity<ImportItemListView>().HasNoKey().ToView(null);

        mb.Entity <ItemExpiryReportView>().HasNoKey().ToView(null);

        mb.Entity<InventoryTransactionsView>().HasNoKey().ToView(null);

        //receiptvoucher
        mb.Entity<VoucherView>().HasNoKey().ToView(null);
        mb.Entity<FillVoucher>().HasNoKey().ToView(null);
        mb.Entity<FillVouTranId>().HasNoKey().ToView(null);
        mb.Entity<QtyView>().HasNoKey().ToView(null);

        //budgeting

        mb.Entity<BudgetRegPandLView>().HasNoKey().ToView(null);
        mb.Entity<BalanceSheetView>().HasNoKey().ToView(null);
        mb.Entity<MonthwiseBalSheetView>().HasNoKey().ToView(null);
        mb.Entity<MonthwisePandLView>().HasNoKey().ToView(null);

        //daybook
        mb.Entity<DayBookView>().HasNoKey().ToView(null);

        //HR
        mb.Entity<HREmployee>().HasNoKey().ToView(null);

        //Restaurant
        mb.Entity<PrintKotView>().HasNoKey().ToView(null);
        mb.Entity<KitchenCategoryView>().HasNoKey().ToView(null);
        mb.Entity<ProductVew>().HasNoKey().ToView(null);

        //RecallVoucher
        mb.Entity<RecallVoucherView>().HasNoKey().ToView(null);


        //finance-statements
        mb.Entity<TrialBalanceView>().HasNoKey().ToView(null);
        mb.Entity<TrialBalanceView>().HasNoKey().ToView(null);
        mb.Entity<AccStatementView>().HasNoKey().ToView(null);
        mb.Entity<GroupStatementView>().HasNoKey().ToView(null);
        mb.Entity<BillwiseStatementView>().HasNoKey().ToView(null);
        mb.Entity<BalSheetView3>().HasNoKey().ToView(null);
        mb.Entity<ConsolMonthwiseView>().HasNoKey().ToView(null);
        mb.Entity<PaymentAnalysisView>().HasNoKey().ToView(null);
        mb.Entity<PartyOutstandingView>().HasNoKey().ToView(null);
        mb.Entity<DebitCreditView>().HasNoKey().ToView(null);
        mb.Entity<ProfitAndLossView3>().HasNoKey().ToView(null);
        mb.Entity<eReturnView>().HasNoKey().ToView(null);

        mb.Entity<InventoryProfitItemView>().HasNoKey().ToView(null);
        mb.Entity<InventoryProfitVoucherView>().HasNoKey().ToView(null);
        mb.Entity<InventoryProfitVoucherViews>().HasNoKey().ToView(null);
        mb.Entity<InventoryProfitPartyView>().HasNoKey().ToView(null);
        mb.Entity<InventoryProfitPartyViews>().HasNoKey().ToView(null); 
        mb.Entity<ItemsHistoryReportView>().HasNoKey().ToView(null); 
  mb.Entity<MonthlyInvSummaryView>().HasNoKey().ToView(null);


    }

}