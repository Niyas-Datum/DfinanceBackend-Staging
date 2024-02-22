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
    public class MaCustomerCategoresConfiguration:IEntityTypeConfiguration<MaCustomerCategories>
    {
        public void Configure(EntityTypeBuilder<MaCustomerCategories> builder)
        { 
        builder.ToTable("MaCustomerCategories");
            builder.Property(e => e.ID).HasColumnName("ID");
            builder.Property(e => e.CreatedBranchID).HasColumnName("CreatedBranchID");
            builder.Property(e => e.CreatedOn).HasColumnType("datetime");
            builder.Property(e => e.Name)
                .HasMaxLength(15)
                .IsUnicode(false);
            builder.Property(e => e.OverdueLimitPerc).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.SalesLimit).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.SalesPriceLowVarPerc).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.SalesPriceUpVarPerc).HasColumnType("decimal(18, 2)");
        }
    }
}
