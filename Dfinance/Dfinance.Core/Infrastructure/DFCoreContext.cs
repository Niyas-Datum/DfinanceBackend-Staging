using Microsoft.EntityFrameworkCore;
using Dfinance.Core.Domain;
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


}