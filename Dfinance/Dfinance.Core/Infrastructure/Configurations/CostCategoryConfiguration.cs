using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Dfinance.Core.Infrastructure.Configurations
{
    public class CostCategoryConfiguration:IEntityTypeConfiguration<CostCategory>
    {
        public void Configure(EntityTypeBuilder<CostCategory> builder) 
        {
            builder.ToTable("CostCategory");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");

            /* relation:CostCategory->MaEmployee
            connection:one to many */
            builder.HasOne(d => d.CreatedByEmployee).WithMany(p => p.CostCategoryCreatedBy)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_CostCategory_MaEmployees");

            /* relation:CostCategory->MaCompany
              connection:one to many */
            builder.HasOne(d => d.CreatedBranch).WithMany(p => p.BranchCostCategory)
                .HasForeignKey(d => d.CreatedBranchId)
                .HasConstraintName("FK_CostCategory_MaCompanies");

        }
    }
}
