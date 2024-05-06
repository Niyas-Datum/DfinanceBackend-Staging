using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class UserTrackConfiguration : IEntityTypeConfiguration<UserTrack>
    {
        public void Configure(EntityTypeBuilder<UserTrack> builder)
        {
            builder.ToTable("UserTrack");

            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.ActionDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.ActionId).HasColumnName("ActionID");

            builder.Property(e => e.Amount).HasColumnType("money");

            builder.Property(e => e.MachineName).HasMaxLength(50);

            builder.Property(e => e.ModuleName).HasMaxLength(50);

            builder.Property(e => e.Reason)
                .HasColumnType("ntext")
                .HasDefaultValueSql("(N'Deleted')");

            builder.Property(e => e.Reference).HasMaxLength(50);

            builder.Property(e => e.RowId).HasColumnName("RowID");

            builder.Property(e => e.TableName).HasMaxLength(50);

            builder.Property(e => e.UserId).HasColumnName("UserID");

        }
    }
}
