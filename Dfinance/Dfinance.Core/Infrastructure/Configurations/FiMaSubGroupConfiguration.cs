using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{

    public class FiMaSubGroupConfiguration : IEntityTypeConfiguration<FiMaSubGroup>
    {
        public void Configure(EntityTypeBuilder<FiMaSubGroup> builder)
        {
            builder.ToTable("FiMaSubGroup");

            builder.HasIndex(e => e.Description, "IX_FiMaSubGroup")
                .IsUnique();

            builder.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

            builder.Property(e => e.Description).HasMaxLength(30);

            builder.Property(e => e.GroupType)
                .HasMaxLength(20)
                .HasDefaultValueSql("((0))");

            builder.Property(e => e.MajorGroup).HasMaxLength(30);

            builder.Property(e => e.OrderNo).HasDefaultValueSql("((0))");
        }

        }
    }

