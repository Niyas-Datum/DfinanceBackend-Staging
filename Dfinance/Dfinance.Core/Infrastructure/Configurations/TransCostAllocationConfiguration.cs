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
    public class TransCostAllocationConfiguration : IEntityTypeConfiguration<TransCostAllocation>
    {
        public void Configure(EntityTypeBuilder<TransCostAllocation> builder)
        {
            builder.ToTable("TransCostAllocation");
            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.Amount).HasColumnType("money");

            builder.Property(e => e.CostCentreId).HasColumnName("CostCentreID");

            builder.Property(e => e.Description).HasMaxLength(200);

            builder.Property(e => e.Veid).HasColumnName("VEID");

            builder.HasOne(d => d.CostCentre)
                .WithMany(p => p.TransCostAllocations)
                .HasForeignKey(d => d.CostCentreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransCostAllocations_CostCentre");

            builder.HasOne(d => d.Ve)
                .WithMany(p => p.TransCostAllocations)
                .HasForeignKey(d => d.Veid)
                .HasConstraintName("FK_TransCostAllocations_FiTransactionEntries");
        }
    }
}
