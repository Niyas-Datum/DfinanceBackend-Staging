using Microsoft.EntityFrameworkCore;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure.Configurations;
using Dfinance.Core.views;

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
    public DbSet<MaCompany> MaBranches { get; set; }

    public DbSet<FiMaAccount> FiMaAccounts { get; set; }
    public DbSet<MaDepartment> MaDepartments { get; set; }
    public DbSet<ReDepartmentType> ReDepartmentTypes { get; set; }

    public DbSet<SpMacompanyFillallbranch> SpMacompanyFillallbranch { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      =>   optionsBuilder.UseSqlServer(@"Data Source=ip.datuminnovation.com,9600;TrustServerCertificate=true;Initial Catalog=DatumSystemMain;User ID=sa;pwd=Datum123!");

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.ApplyConfiguration(new MaEmployeeConfiguration());
        mb.ApplyConfiguration(new MaCompanyConfiguration());
        mb.ApplyConfiguration(new MaDepartmentConfiguration());

        //views
        mb.Entity<SpMacompanyFillallbranch>().HasNoKey().ToView(null);

    }
}