using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class ChequeTemplateFieldConfiguration : IEntityTypeConfiguration<ChequeTemplateField>
    {
        public void Configure(EntityTypeBuilder<ChequeTemplateField> builder)
        {
            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.Casing).HasMaxLength(20);

            builder.Property(e => e.ChequeTemplateId).HasColumnName("ChequeTemplateID");

            builder.Property(e => e.FieldId)
                .HasMaxLength(50)
                .HasColumnName("FieldID");

            builder.Property(e => e.Font).HasMaxLength(50);

            builder.Property(e => e.FontStyle).HasMaxLength(10);

            builder.HasOne(d => d.ChequeTemplate)
                .WithMany(p => p.ChequeTemplateFields)
                .HasForeignKey(d => d.ChequeTemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChequeTemplateFields_ChequeTemplate");
        }
    }
}
