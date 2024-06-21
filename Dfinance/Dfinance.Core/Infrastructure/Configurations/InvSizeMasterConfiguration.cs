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
    public class InvSizeMasterConfiguration : IEntityTypeConfiguration<InvSizeMaster>
    {
        public void Configure(EntityTypeBuilder<InvSizeMaster> builder)
        {
            builder.ToTable("InvSizeMaster");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

            builder.Property(e => e.Code).HasMaxLength(50);

            builder.Property(e => e.Description).HasMaxLength(200);

            builder.Property(e => e.Name).HasMaxLength(50);
        }
    }
}
