using Dfinance.AuthCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.AuthCore.Infrastructure.Configurations
{
    /**
   # Created On: Thursday 18 Aug 2023
   # Updated: Niyas
   **/
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company");
            builder.HasIndex(x => x.Code, "IX_Company").IsUnique();
            builder.HasIndex(e => e.Name, "IX_Company_1").IsUnique();
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.Code).HasMaxLength(50);
            builder.Property(x => x.DatabaseName).HasMaxLength(50);
            builder.Property(x => x.Name).HasMaxLength(150);
            builder.Property(x => x.ServerIp).HasMaxLength(50).HasColumnName("ServerIP");
            builder.Property(x => x.ServerName).HasMaxLength(50);
            builder.Property(x => x.Active).IsRequired().HasDefaultValueSql("1");
        }

    }
}