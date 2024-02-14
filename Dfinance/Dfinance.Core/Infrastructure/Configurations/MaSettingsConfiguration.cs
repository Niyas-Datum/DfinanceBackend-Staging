using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class MaSettingsConfiguration : IEntityTypeConfiguration<MaSettings>
    {
        public void Configure(EntityTypeBuilder<MaSettings> builder) 
        {
            builder.HasIndex(e => new { e.Key, e.Value }, "IX_MaSettings").IsUnique();
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.ModuleId).HasColumnName("ModuleID");
            builder.Property(e => e.Description).HasMaxLength(200);
            builder.Property(e => e.Key).HasMaxLength(50);
            builder.Property(e => e.Value).HasMaxLength(150);

        }
    }
}
