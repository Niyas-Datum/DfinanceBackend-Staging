using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class MaMiscConfiguration: IEntityTypeConfiguration<MaMisc>
    {
        public void Configure(EntityTypeBuilder<MaMisc> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_MaMisc");           
            builder.Property(e => e.Id).HasColumnName("ID");
            
        }
    }
}
