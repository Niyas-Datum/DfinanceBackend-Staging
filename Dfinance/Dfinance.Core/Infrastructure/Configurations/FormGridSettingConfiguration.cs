using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class FormGridSettingConfiguration : IEntityTypeConfiguration<FormGridLabelView>
    {
        public void Configure(EntityTypeBuilder<FormGridLabelView> builder)
        {
            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.ArabicCaption).HasMaxLength(100);

            builder.Property(e => e.BranchId).HasColumnName("BranchID");

            builder.Property(e => e.ColumnName).HasMaxLength(100);

            builder.Property(e => e.FormName).HasMaxLength(30);

            builder.Property(e => e.GridName).HasMaxLength(100);

            builder.Property(e => e.NewCaption).HasMaxLength(50);

            builder.Property(e => e.OriginalCaption).HasMaxLength(50);

            builder.Property(e => e.PageId).HasColumnName("PageID");
        }
    }
}