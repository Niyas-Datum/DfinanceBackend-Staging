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
    public class FimaPrimaryVoucherConfiguraton : IEntityTypeConfiguration<FiPrimaryVoucher>
    {
        public void Configure(EntityTypeBuilder<FiPrimaryVoucher> builder)
        {
           
                builder.HasIndex(e => e.Name, "IX_FiPrimaryVouchers")
                    .IsUnique();

                builder.Property(e => e.Id)
                            .ValueGeneratedNever()
                            .HasColumnName("ID");

                builder.Property(e => e.DayBookDrCr)
                            .HasMaxLength(1)
                            .IsUnicode(false)
                            .IsFixedLength();

                builder.Property(e => e.Name)
                            .HasMaxLength(50)
                            .IsUnicode(false);
           

        }
    }
}
