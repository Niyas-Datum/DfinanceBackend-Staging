using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class InvBarcodeMasterConfiguuration : IEntityTypeConfiguration<InvBarcodeMaster>
    {
        public void Configure(EntityTypeBuilder<InvBarcodeMaster> builder)
        {
            builder.ToTable("InvBarcodeMaster");
            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            builder.Property(e => e.Barcode).HasMaxLength(50);

        }
    }
}
