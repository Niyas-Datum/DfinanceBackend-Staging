using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Dfinance.Core.Domain;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class TaxFormMasterConfiguration : IEntityTypeConfiguration<TaxFormMaster>
    {
        public void Configure(EntityTypeBuilder<TaxFormMaster> builder)
        {
            builder.ToTable("TaxFormMaster");

            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.CrystalPath).HasMaxLength(200);
            builder.Property(e => e.Name).HasMaxLength(50);

        }
    }
}
