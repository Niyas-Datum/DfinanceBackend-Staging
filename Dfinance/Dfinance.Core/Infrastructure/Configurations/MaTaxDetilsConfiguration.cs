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


    public class MaTaxDetilsConfiguration : IEntityTypeConfiguration<MaTaxDetail>
    {
        public void Configure(EntityTypeBuilder<MaTaxDetail> builder)
        {
            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.CategoryId).HasColumnName("CategoryID");

            builder.Property(e => e.PurchasePerc).HasColumnType("decimal(18, 2)");

            builder.Property(e => e.SalesPerc).HasColumnType("decimal(18, 2)");

            builder.Property(e => e.TaxTypeId).HasColumnName("TaxTypeID");

            builder.HasOne(d => d.Category)
                .WithMany(p => p.MaTaxDetails)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaTaxDetails_MaTax");
        }
    }
}