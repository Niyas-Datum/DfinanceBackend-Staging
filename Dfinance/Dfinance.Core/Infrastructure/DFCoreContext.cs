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

    public DbSet<MaEmployee> MaEmployees { get; set; }
    public DbSet<MaEmployeeDetail> MaEmployeeBranchDet { get; set; }
    public DbSet<MaRoles> UserRoles { get; set; }
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
    public DbSet<Categories> Commodity { get; set; }
    public DbSet<MaArea> MaArea { get; set; }
    public DbSet<FiMaAccountCategory> FiMaAccountCategory { get; set; }
    public DbSet<FiMaBranchAccounts> FiMaBranchAccounts { get; set; }
    public DbSet<CategoryType> CategoryType { get; set; }
    //MaSettings
    public DbSet<MaSettings> MaSettings { get; set; }
   
    //Fin=>Currency
    public DbSet<Currency> Currency { get; set; }
    public DbSet<CurrencyCode> CurrencyCode { get; set; }
   //Fin=>AccountsList
   public DbSet<FiAccountsList> FiAccountsList { get; set; }
    public DbSet<FiMaAccountsList> FiMaAccountsLists { get; set; }
    public DbSet<TblMaFinYear> TblMaFinYear { get; set; }

    //view init

    // read- id, code name
    public DbSet<ReadView> ReadView { get; set; }
    // read- id, code description
    public DbSet<ReadViewDesc> ReadViewDesc { get; set; } 
    //nextCodeView id,descripition
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
    
    public DbSet<UserPageListView> UserPageListView { get; set; }
	//Currency 
    public DbSet<FillcurrencyCode> FillcurrencyCode { get; set; }
    public DbSet<FillCurrencyCodeById> FillCurrencyCodeById { get; set; }
    public DbSet<FillCurrency> FillCurrency { get; set; }
    public DbSet<FillCurrencyById> FillCurrencyById { get; set; }
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
    public DbSet<Voucher> FiMaVouchers {  get; set; }
    public DbSet<NameView> NameView {  get; set; }  //  retun only name

    //MaNumbering
    public DbSet<MaNumbering> MaNumbering { get; set; }
    //password 
    public DbSet<PasswordCheckResult> PasswordCheckResult { get; set; }
    //SettingView 
    public DbSet<FillSettingById> FillSettingById { get; set; }
    public DbSet<FillSetting> FillSetting { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //  => optionsBuilder.UseSqlServer(@"Data Source=ip.datuminnovation.com,9600;TrustServerCertificate=true;Initial Catalog=DatumSystemMain;User ID=sa;pwd=Datum123!");
   // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

    => optionsBuilder.UseSqlServer(con);

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.ApplyConfiguration(new MaEmployeeConfiguration());
        mb.ApplyConfiguration(new MaCompanyConfiguration());
        mb.ApplyConfiguration(new MaDepartmentConfiguration());
        mb.ApplyConfiguration(new MaMiscConfiguration());
        mb.ApplyConfiguration(new CostCategoryConfiguration());
        mb.ApplyConfiguration(new MaEmployeeDetailConfiguration());
        mb.ApplyConfiguration(new MaRolesConfiguration());
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
        //settings
        mb.ApplyConfiguration(new MaSettingsConfiguration());
		//currency 
		 mb.ApplyConfiguration(new CurrencyConfigurations());
        //Financeyear
        mb.ApplyConfiguration(new TblMaFinYearConfigurations());
    //AccountList
        mb.ApplyConfiguration(new FiAccountsListConfiguration());
        mb.ApplyConfiguration(new FiMaAccountsListConfiguration());
        mb.Entity<CurrencyCode>().HasNoKey().ToView(null);
        mb.Entity<UserPageListView>().HasNoKey().ToView(null);
        mb.Entity<UserInfo>().HasNoKey().ToView(null);
        mb.Entity<SpUser>().HasNoKey().ToView(null);
        
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
    }
}