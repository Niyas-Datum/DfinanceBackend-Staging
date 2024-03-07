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
    public class PriceCategoryConfiguration : IEntityTypeConfiguration<MaPriceCategory>
    {
        public void Configure(EntityTypeBuilder<MaPriceCategory> builder)
        {
            builder.ToTable("MaPriceCategory");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");

            //relationship with MaCompany
            builder.HasOne(d => d.CreatedBranch).WithMany(p => p.PriceCategories)
               .HasForeignKey(d => d.CreatedBranchId)
               .HasConstraintName("FK_MaPriceCategory_MaCompanies");

            //relationship with MaEmployee
            builder.HasOne(d => d.CreatedEmployee).WithMany(p => p.PriceCategories)
               .HasForeignKey(d => d.CreatedBranchId)
               .HasConstraintName("FK_MaPriceCategory_MaEmployees");
        }
    }
}
