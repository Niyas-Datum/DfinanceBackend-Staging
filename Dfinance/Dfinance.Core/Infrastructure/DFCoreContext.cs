using Microsoft.EntityFrameworkCore;
using Dfinance.Core.Domain;
using Dfinance.AuthCore.Infrastructure.Configurations;

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
    public DbSet<FiMaAccount> FiMaAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      =>   optionsBuilder.UseSqlServer(@"Data Source=ip.datuminnovation.com,9600;TrustServerCertificate=true;Initial Catalog=DatumSystemMain;User ID=sa;pwd=Datum123!");

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.ApplyConfiguration(new MaEmployeeConfiguration());
    }
    }