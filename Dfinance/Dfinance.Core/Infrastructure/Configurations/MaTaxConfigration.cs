using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfinance.Core.Domain;

namespace Dfinance.Core.Infrastructure.Configurations
{

    public class MaTaxConfigration : IEntityTypeConfiguration<MaTax>
    {
        public void Configure(EntityTypeBuilder<MaTax> builder)
        {
            builder.ToTable("MaTax");

            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");

            builder.Property(e => e.CreatedOn).HasColumnType("datetime");

            builder.Property(e => e.Name).HasMaxLength(100);

            builder.Property(e => e.Note).HasMaxLength(200);

            builder.HasOne(d => d.CreatedBranch)
                .WithMany(p => p.MaTaxes)
                .HasForeignKey(d => d.CreatedBranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaTax_MaCompanies");

            builder.HasOne(d => d.CreatedByNavigation)
                .WithMany(p => p.MaTaxes)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaTax_MaEmployees");
        }
    }
}
