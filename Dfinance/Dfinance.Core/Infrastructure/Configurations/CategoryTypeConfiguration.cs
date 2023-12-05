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
    public class CategoryTypeConfiguration: IEntityTypeConfiguration<CategoryType>
    {
        public void Configure(EntityTypeBuilder<CategoryType> builder)
        {
            builder.ToTable("TypeofWood");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");          
           
        }
    }
}
