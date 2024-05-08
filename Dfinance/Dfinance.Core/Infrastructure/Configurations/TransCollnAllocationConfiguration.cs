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
    public class TransCollnAllocationConfiguration : IEntityTypeConfiguration<TransCollnAllocation>
    {
        public void Configure(EntityTypeBuilder<TransCollnAllocation> builder)
        {
            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasComment("Amount allocated to this collection");

            builder.Property(e => e.Description).HasMaxLength(250);

            builder.Property(e => e.TransCollectionId).HasColumnName("TransCollectionID");

            builder.Property(e => e.VallocationId)
                .HasColumnName("VAllocationID")
                .HasComment("VoucherAllocation ID");

            builder.HasOne(d => d.TransCollection)
                .WithMany(p => p.TransCollnAllocations)
                .HasForeignKey(d => d.TransCollectionId)
                .HasConstraintName("FK_TransCollnAllocations_TransCollections");

            builder.HasOne(d => d.Vallocation)
                .WithMany(p => p.TransCollnAllocations)
                .HasForeignKey(d => d.VallocationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TransCollnAllocations_FiVoucherAllocation");
        }
    }
}
