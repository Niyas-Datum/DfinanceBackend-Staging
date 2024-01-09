using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class FiMaAccountConfiguration : IEntityTypeConfiguration<FiMaAccount>
    {
        public void Configure(EntityTypeBuilder<FiMaAccount> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_MaLedgers");

            builder.ToTable(tb =>
{
    tb.HasTrigger("GroupParentUpdate");
    tb.HasTrigger("PreventFirstLevelUpdation");
    tb.HasTrigger("PreventParentNULL");
    tb.HasTrigger("PreventSystemAccountDeletion");
});

            builder.HasIndex(e => e.Parent, "IX_FiMaAccounts");

            builder.HasIndex(e => e.Alias, "IX_FiMaAccounts_Alias").IsUnique();

            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.AccountTypeId).HasColumnName("AccountTypeID");
            builder.Property(e => e.Alias).HasMaxLength(50);
            builder.Property(e => e.AlternateName).HasMaxLength(100);
            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");
            builder.Property(e => e.CreatedOn).HasColumnType("datetime");
            builder.Property(e => e.Date).HasColumnType("datetime");
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.Narration).HasMaxLength(200);
            builder.Property(e => e.PreventExtraPay).HasDefaultValueSql("((0))");

            builder.HasOne(d => d.AccountCategoryNavigation).WithMany(p => p.FiMaAccounts)
    .HasForeignKey(d => d.AccountCategory)
    .HasConstraintName("FK_FiMaAccounts_FiMaAccountCategory");

            builder.HasOne(d => d.ParentNavigation).WithMany(p => p.InverseParentNavigation)
    .HasForeignKey(d => d.Parent)
    .HasConstraintName("FK_FiMaAccounts_FiMaAccounts");
        }
    }
}