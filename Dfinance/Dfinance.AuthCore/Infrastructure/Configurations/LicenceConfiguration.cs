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
    class LicenceConfiguration : IEntityTypeConfiguration<Licence>
    {
        public void Configure(EntityTypeBuilder<Licence> builder)
        {
            builder.ToTable("Licence");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.ActivatedKey).HasMaxLength(20);
            builder.Property(e => e.LicenceKey).HasMaxLength(30);
            builder.Property(e => e.ActivatedOn).HasColumnType("datetime");
            builder.Property(e => e.CreatedOn).HasColumnType("datetime");


        }
    }
}