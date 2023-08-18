using Dfinance.AuthCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.AuthCore.Infrastructure.Configurations
{
    /**
   # Created On: Thursday 18 Aug 2023
   # Not use in this project
   # Updated: Niyas
   **/
    public class IpaddressConfiguration : IEntityTypeConfiguration<Ipaddress>
    {
        public void Configure(EntityTypeBuilder<Ipaddress> builder)
        {
            builder.ToTable("IPAddress");
            builder.HasIndex(d => d.Ipaddress1, "IX_Address").IsUnique();
            builder.Property(d => d.Id).HasColumnName("ID").ValueGeneratedOnAdd();
            builder.Property(d => d.Ipaddress1).HasMaxLength(50).HasColumnName("IPAddress");
        }
    }
}