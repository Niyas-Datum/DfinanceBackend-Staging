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

namespace Dfinance.Core.Infrastructure;


public partial class DFCoreContext : DbContext
{
    public DFCoreContext()
    {

    }

    public DFCoreContext(DbContextOptions<DFCoreContext> options) : base(options)
    {
    }

    public DbSet<MaEmployee> MaEmployees { get; set; }
    public DbSet<MaEmployeeDetail> MaEmployeeBranchDet { get; set; }
    public DbSet<MaRoles> UserRoles { get; set; }
    public DbSet<MaUserRight> UserRolePermission { get; set; }
    public DbSet<MaPageMenu> MaPageMenus { get; set; }

    public DbSet<LogInfo> LogInfo { get; set; }
    public DbSet<MaCompany> MaBranches { get; set; }

    public DbSet<FiMaAccount> FiMaAccounts { get; set; }
    public DbSet<MaDepartment> MaDepartments { get; set; }
    public DbSet<ReDepartmentType> ReDepartmentTypes { get; set; }
    public DbSet<MaCompany> MaMisc { get; set; }
    public DbSet<MaMiscKeys> MaMiscKeys { get; set; }
    public DbSet<MaDesignation> MaDesignations { get; set; }
    public DbSet<CostCategory> CostCategory { get; set; }
    public DbSet<CostCentre> CostCentre { get; set; }
    public DbSet<Categories> Commodity { get; set; }
    public DbSet<MaArea> MaArea { get; set; }


    //view init


    public DbSet<SpFillAreaMasterByIdG> SpFillAreaMasterByIdG { get; set; }
    public DbSet<SpFillAreaMasterG> SpFillAreaMasterG { get; set; }
    public DbSet<SpFillCategoryByIdG> SpFillCategoryByIdG { get; set; }
    public DbSet<SpFillCategoryG> SpFillCategoryG { get; set; }
    public DbSet<SpCostCentreDropDown> SpCostCentreDropDown { get; set; }
    public DbSet<SpCostCategoryDropDown> SpFillCostCategoryDropDown { get; set; }
    public DbSet<SpFillCostCentreByIdG> SpFillCostCentreByIdG { get; set; }
    public DbSet<SpFillCostCentreG> SpFillCostCentreG { get; set; }
    public DbSet<SpFillCostCategoryByIdG> SpFillCostCategoryByIdG { get; set; }
    public DbSet<SpFillCostCategoryG> SpFillCostCategoryG { get; set; }
    public DbSet<SpFillAllBranchByIdG> SpFillAllBranchByIdG { get; set; }
    public DbSet<SpFillAllBranchG> SpFillAllBranch { get; set; }
    public DbSet<SpFillEmployees> SpFillEmployees { get; set; }
    public DbSet<SpMacompanyFillallbranch> SpMacompanyFillallbranch { get; set; }
    public DbSet<SpFillMaMisc> SpFillMaMisc { get; set; }
    public DbSet<spDepartmentTypesC> spDepartmentTypesC { get; set; }
    public DbSet<SpReDepartmentTypeFillAllDepartment> SpReDepartmentTypeFillAllDepartment { get; set; }
    public DbSet<SpUserGById> SpUserGById { get; set; }
    //public DbSet<spMaDesignationsC> spMaDesignationsC {  get; set; }
    public DbSet<SpDesignationMasterG> SpFillDesignationMaster { get; set; }
    public DbSet<SpDesignationMasterByIdG> SpDesignationMasterByIdG { get; set; }
    public DbSet<spDepartmentTypesGetById> spDepartmentTypesGetById { get; set; }
    //public DbSet<AuthResponseDtoView> AuthResponseDtoView { get; set; }
    public DbSet<UserInfo> UserInfo { get; set; }
    public DbSet<UserPageListView> UserPageListView { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      => optionsBuilder.UseSqlServer(@"Data Source=ip.datuminnovation.com,9600;TrustServerCertificate=true;Initial Catalog=DatumSystemMain;User ID=sa;pwd=Datum123!");

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


        //views configuring
        mb.Entity<UserPageListView>().HasNoKey().ToView(null);
        mb.Entity<UserInfo>().HasNoKey().ToView(null);
        //mb.Entity<AuthResponseDtoView>().HasNoKey().ToView(null);
        mb.Entity<SpMacompanyFillallbranch>().HasNoKey().ToView(null);
        mb.Entity<SpFillMaMisc>().HasNoKey().ToView(null);
        mb.Entity<SpFillEmployees>().HasNoKey().ToView(null);
        mb.Entity<spDepartmentTypesC>().HasNoKey().ToView(null);
        mb.Entity<SpReDepartmentTypeFillAllDepartment>().HasNoKey().ToView(null);
        mb.Entity<SpUserGById>().HasNoKey().ToView(null);
        mb.Entity<spDepartmentTypesGetById>().HasNoKey().ToView(null);

        mb.Entity<SpFillAllBranchByIdG>().HasNoKey().ToView(null);
        mb.Entity<SpFillAllBranchG>().HasNoKey().ToView(null);
        mb.Entity<SpFillCostCategoryG>().HasNoKey().ToView(null);
        mb.Entity<SpFillCostCategoryByIdG>().HasNoKey().ToView(null);
        mb.Entity<SpFillCostCentreG>().HasNoKey().ToView(null);
        mb.Entity<SpFillCostCentreByIdG>().HasNoKey().ToView(null);
        mb.Entity<SpCostCategoryDropDown>().HasNoKey().ToView(null);
        mb.Entity<SpCostCentreDropDown>().HasNoKey().ToView(null);
        mb.Entity<SpFillCategoryG>().HasNoKey().ToView(null);
        mb.Entity<SpFillCategoryByIdG>().HasNoKey().ToView(null);
        mb.Entity<SpFillAreaMasterG>().HasNoKey().ToView(null);
        mb.Entity<SpFillAreaMasterByIdG>().HasNoKey().ToView(null);
    }
}