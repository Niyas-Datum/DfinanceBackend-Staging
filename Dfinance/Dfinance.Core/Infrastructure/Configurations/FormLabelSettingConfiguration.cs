using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Dfinance.Core.Domain;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class FormlabelSettingConfiguration : IEntityTypeConfiguration<FormLabelSetting>
    {
        public void Configure(EntityTypeBuilder<FormLabelSetting> builder)
        {
            builder.HasKey(e => new { e.FormName, e.LabelName })
                   .HasName("PK_FormLabelSetting");

            builder.Property(e => e.FormName).HasMaxLength(30);

            builder.Property(e => e.LabelName).HasMaxLength(30);

            builder.Property(e => e.ArabicCaption).HasMaxLength(100);

            builder.Property(e => e.BranchId).HasColumnName("BranchID");

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");

            builder.Property(e => e.NewCaption).HasMaxLength(50);

            builder.Property(e => e.OriginalCaption).HasMaxLength(50);

            builder.Property(e => e.PageId).HasColumnName("PageID");
        }
    }
}