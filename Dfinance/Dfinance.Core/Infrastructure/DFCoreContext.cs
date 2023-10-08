using Microsoft.EntityFrameworkCore;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure.Configurations;
using Dfinance.Core.Views;
using Dfinance.Core.Views.General;
using Dfinance.Core.Domain.Roles;
using Dfinance.Core.Infrastructure.Configurations.Employee;
using Dfinance.Core.Infrastructure.Configurations.Roles;

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


    public DbSet<MaCompany> MaBranches { get; set; }

    public DbSet<FiMaAccount> FiMaAccounts { get; set; }
    public DbSet<MaDepartment> MaDepartments { get; set; }
    public DbSet<ReDepartmentType> ReDepartmentTypes { get; set; }
    public DbSet<MaCompany> MaMisc { get; set; }
    public DbSet<MaMiscKeys> MaMiscKeys { get; set; }
   
  

    //view init
    public DbSet<SpMaCompanyC> SpMaCompanyC {  get; set; }
    public DbSet<SpFillEmployees> SpFillEmployees { get; set; }
    public DbSet<SpMacompanyFillallbranch> SpMacompanyFillallbranch { get; set; }
    public DbSet<SpCountryDropDown> SpCountryDropDown { get; set; }
    public DbSet<spDepartmentTypesC> spDepartmentTypesC { get; set; }
    public DbSet<SpReDepartmentTypeFillAllDepartment> SpReDepartmentTypeFillAllDepartment { get; set; }
    public DbSet<SpMaEmployeesC> SpMaEmployeesC {  get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      =>   optionsBuilder.UseSqlServer(@"Data Source=ip.datuminnovation.com,9600;TrustServerCertificate=true;Initial Catalog=DatumSystemMain;User ID=sa;pwd=Datum123!");

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.ApplyConfiguration(new MaEmployeeConfiguration());
        mb.ApplyConfiguration(new MaEmployeeDetailConfiguration());

        mb.ApplyConfiguration(new MaCompanyConfiguration());
        mb.ApplyConfiguration(new MaDepartmentConfiguration());
        mb.ApplyConfiguration(new MaMiscConfiguration());

        mb.ApplyConfiguration(new MaRolesConfiguration());
        mb.ApplyConfiguration(new MaUserRightConfiguration());

        //views configuring
        mb.Entity<SpMacompanyFillallbranch>().HasNoKey().ToView(null);
        mb.Entity<SpCountryDropDown>().HasNoKey().ToView(null);
        mb.Entity<SpFillEmployees>().HasNoKey().ToView(null); 
        mb.Entity<SpMaCompanyC>().HasNoKey().ToView(null);
        mb.Entity<spDepartmentTypesC>().HasNoKey().ToView(null);
        mb.Entity<SpReDepartmentTypeFillAllDepartment>().HasNoKey().ToView(null);
        mb.Entity<SpMaEmployeesC>().HasNoKey().ToView(null);

    }
}