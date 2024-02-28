using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class CategoryConfiguration:IEntityTypeConfiguration<Categories>
    {
        public void Configure(EntityTypeBuilder<Categories> builder) 
        {
            builder.ToTable("Commodity");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");
            builder.Property(e => e.CategoryTypeId).HasColumnName("TypeofWoodID");
            builder.Property(e => e.CategoryCode).HasColumnName("Code");
            builder.Property(e => e.Category).HasColumnName("Category");
        }
    }
}
