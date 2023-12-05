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
    public class CurrencyConfigurations : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasIndex(e => e.DefaultCurrency, "IX_Currency");

            builder.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
            builder.Property(e => e.Abbreviation).HasMaxLength(3).IsUnicode(false).IsFixedLength();
            builder.Property(e => e.Coin).HasMaxLength(20).IsUnicode(false);
            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");
            builder.Property(e => e.CreatedOn).HasColumnType("datetime");
            builder.Property(e => e.Culture).HasMaxLength(15);
            builder.Property(e => e.Currency1).HasMaxLength(20).HasColumnName("Currency");
            builder.Property(e => e.FormatString).HasMaxLength(10);
            builder.HasOne(d => d.CreatedBranch).WithMany(p => p.Currencies)
                .HasForeignKey(d => d.CreatedBranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Currency_MaCompanies");

            builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Currencies)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Currency_MaEmployees");
        }
        }
    }

