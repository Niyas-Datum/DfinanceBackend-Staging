using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class MaNumberingConfiguration : IEntityTypeConfiguration<MaNumbering>
    {
        public void Configure(EntityTypeBuilder<MaNumbering> builder)
        {
            builder.HasKey(e => e.Id).HasName ("PK_MasterNumbering_1");
            builder.ToTable("MaNumbering");

            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.Prefillwithzero).HasDefaultValueSql("((1))");
            builder.Property(e => e.Prefix).HasDefaultValueSql("((3))");
            builder.Property(e => e.Renewedby).HasDefaultValueSql("((0))");
            builder.Property(e => e.StartingNumber).HasDefaultValueSql("((1))");
            builder.Property(e => e.Suffix).HasDefaultValueSql("((0))");
            builder.Property(e => e.Editable).HasDefaultValueSql("((0))");
            builder.Property(e => e.MaximumDegits).HasDefaultValueSql("((4))");
            builder.Property(e => e.PrefixValue).HasMaxLength(5);
            builder.Property(e => e.SuffixValue).HasMaxLength(5);

            //builder.HasOne(d => d.RenewedbyNavigation).WithMany(p => p.MaNumberings)
            //    .HasForeignKey(d => d.Renewedby)
            //    .HasConstraintName("FK_MasterNumbering_MasterPeriods");
        }
    }
}
