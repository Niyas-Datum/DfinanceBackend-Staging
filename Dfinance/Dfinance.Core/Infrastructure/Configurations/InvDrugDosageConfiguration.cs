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
    public class InvDrugDosageConfiguration : IEntityTypeConfiguration<InvDrugDosage>
    {
        public void Configure(EntityTypeBuilder<InvDrugDosage> builder)
        {
            builder.ToTable("InvDrugDosage");

            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.Dosage).HasMaxLength(100);

            builder.Property(e => e.Remarks).HasMaxLength(100);

        }
    }
}
