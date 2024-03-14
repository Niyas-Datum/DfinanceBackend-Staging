using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dfinance.Core.Infrastructure.Configurations
{
    public class LocationTypesConfiguration : IEntityTypeConfiguration<LocationTypes>
    {
        public void Configure(EntityTypeBuilder<LocationTypes> builder)
        {
            builder.ToTable("LocationTypes");
            builder.Property(e => e.Id)
              .ValueGeneratedNever()
              .HasColumnName("ID");
            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");
            builder.Property(e => e.CreatedOn).HasColumnType("datetime");
            builder.Property(e => e.LocationType)
                .HasMaxLength(20)
                .HasColumnName("LocationType");
            builder.Property(e => e.SaleSite).HasMaxLength(5);
            builder.HasOne(d => d.CreatedBranch).WithMany(p => p.LocationTypes)
                .HasForeignKey(d => d.CreatedBranchId)
                .HasConstraintName("FK_LocationTypes_MaCompanies");
            builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LocationTypes)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_LocationTypes_MaEmployees");
        }
    }
}
