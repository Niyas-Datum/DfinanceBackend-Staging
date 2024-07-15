using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class ChequeTemplateConfiguration : IEntityTypeConfiguration<ChequeTemplate>
    {
        public void Configure(EntityTypeBuilder<ChequeTemplate> builder)
        {
            builder.ToTable("ChequeTemplate");

            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.AccountId)
                .HasColumnName("AccountID")
                .HasComment("Bank Account from FiMaAccounts");

            builder.Property(e => e.Code).HasMaxLength(50);

            builder.Property(e => e.DateFormat).HasMaxLength(15);

            builder.Property(e => e.DateSeperator)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            builder.Property(e => e.Name).HasMaxLength(100);

            //builder.HasOne(d => d.Account)
            //    .WithMany(p => p.ChequeTemplates)
            //    .HasForeignKey(d => d.AccountId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_ChequeTemplate_FiMaAccounts");

        }
    }
}
