using Dfinance.AuthCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.AuthCore.Infrastructure.Configurations
{
    /**
   # Created On: Thursday 18 Aug 2023
   # Updated: Niyas
   **/
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasIndex(e => e.Username, "IX_Users").IsUnique();
            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.Company).HasMaxLength(50);
            builder.Property(x => x.City).HasMaxLength(50);
            builder.Property(x => x.BussinessType).HasMaxLength(50);
            builder.Property(x => x.Country).HasMaxLength(50);
            builder.Property(x => x.Email).HasMaxLength(50);
            builder.Property(x => x.Mobile).HasMaxLength(20);
            builder.Property(x => x.Password).HasMaxLength(15);
            builder.Property(x => x.Email).HasMaxLength(50);
        }
    }
}