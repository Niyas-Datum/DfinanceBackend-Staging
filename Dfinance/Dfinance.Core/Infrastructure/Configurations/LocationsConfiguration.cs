using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Dfinance.Core.Infrastructure.Configurations
{
    public class LocationsConfiguration : IEntityTypeConfiguration<Locations>
    {
        public void Configure(EntityTypeBuilder<Locations> builder)
        {
            builder.ToTable("Locations");
            builder.ToTable(tb => tb.HasTrigger("PreventLocationDelete"));
            builder.HasIndex(e => new
            {
                e.Id,
                e.Name,
                e.Code
            }, "IX_Locations").IsUnique();
            builder.HasIndex(e => e.LocationTypeId, "IX_Locations_1");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.Address).HasMaxLength(100);
            builder.Property(e => e.ClearingPerCft)
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("ClearingPerCFT");
            builder.Property(e => e.Code).HasMaxLength(20);
            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");
            builder.Property(e => e.CreatedOn).HasColumnType("datetime");
            builder.Property(e => e.DevCode).HasComment("3- Transit, Special handling items");
            builder.Property(e => e.GroundRentPerCft)
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("GroundRentPerCFT");
            builder.Property(e => e.LocationTypeId).HasColumnName("LocationTypeID");
            builder.Property(e => e.LorryHirePerCft)
                        .HasColumnType("decimal(10, 2)")
                     .HasColumnName("LorryHirePerCFT");
            builder.Property(e => e.LottingPerPiece).HasColumnType("decimal(10, 2)");
            builder.Property(e => e.Name).HasMaxLength(50);
            builder.Property(e => e.Remarks).HasMaxLength(200);
            builder.HasOne(d => d.CreatedBranch).WithMany(p => p.Locations)
                        .HasForeignKey(d => d.CreatedBranchId)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Locations_MaCompanies");
            builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Locations)
                        .HasForeignKey(d => d.CreatedBy)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Locations_MaEmployees");
            builder.HasOne(d => d.LocationTypes).WithMany(p => p.Locations)
                        .HasForeignKey(d => d.LocationTypeId)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Locations_LocationTypes");
        }
    }
}
