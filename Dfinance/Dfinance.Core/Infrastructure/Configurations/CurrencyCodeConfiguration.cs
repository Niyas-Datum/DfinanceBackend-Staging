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
    public class CurrencyCodeConfigurations : IEntityTypeConfiguration<CurrencyCode>
    {
        public void Configure(EntityTypeBuilder<CurrencyCode> builder)
        {
            builder.ToTable("CurrencyCode");

            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.Code).HasMaxLength(10);
            builder.Property(e => e.Name).HasMaxLength(100);
        }

    }
 }

