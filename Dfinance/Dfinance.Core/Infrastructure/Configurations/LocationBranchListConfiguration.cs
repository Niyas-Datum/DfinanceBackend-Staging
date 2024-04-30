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
    public class LocationBranchListConfiguration : IEntityTypeConfiguration<LocationBranchList>
    {
        public void Configure(EntityTypeBuilder<LocationBranchList> builder)
        {
            builder.ToTable("LocationBranchList");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.BranchId).HasColumnName("BranchID");
            builder.Property(e => e.LocationId).HasColumnName("LocationID");
            builder.HasOne(d => d.Branch).WithMany(p => p.LocationBranchList)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_LocationBranchList_MaCompanies");
            builder.HasOne(d => d.Locations).WithMany(p => p.LocationBranchLists)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_LocationBranchList_Locations");
        }
    }
}