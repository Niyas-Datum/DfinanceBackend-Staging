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
    public class FiMaAccountCategoryConfiguration : IEntityTypeConfiguration<FiMaAccountCategory>
    {
        public void Configure(EntityTypeBuilder<FiMaAccountCategory> builder)
        {
            builder.ToTable("FiMaAccountCategory");
            builder.Property(e => e.Id).HasColumnName("ID");         
        }
    }
}
