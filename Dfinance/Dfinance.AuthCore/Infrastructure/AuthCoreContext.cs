using Dfinance.AuthCore.Domain;
using Dfinance.AuthCore.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.AuthCore.Infrastructure
{
    /**
    # Created On: Firday 18 Aug 2023
    # Updated: Niyas
    **/
    public class AuthCoreContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        // --not-in-use public DbSet<Ipaddress> Ipaddresses { get; set; }
        public DbSet<CompanySchedule> CompanySchedules { get; set; }
        // --not-in-use  public DbSet<Licence> Licences { get; set; }

        public AuthCoreContext(DbContextOptions<AuthCoreContext> options) : base(options)
        {
        }
        public AuthCoreContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.ApplyConfiguration(new UserConfiguration());
            mb.ApplyConfiguration(new CompanyConfiguration());
            mb.ApplyConfiguration(new UserCompanyConfiguration());
            mb.ApplyConfiguration(new CompanyScheduleConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
            //// if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer(@"Data Source=ip.datuminnovation.com,9600;TrustServerCertificate=true;Initial Catalog=DatumSystemMaster;User ID=sa;pwd=Datum123!");
            //}
        }
    }
}