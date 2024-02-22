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
    public class MaCustomerDetailsConfiguration : IEntityTypeConfiguration<MaCustomerDetails>
    {
        public void Configure(EntityTypeBuilder<MaCustomerDetails> builder)
        {

        
            builder.ToTable("MaCustomerDetails");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.AddressOwned)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            builder.Property(e => e.BandByHoid).HasColumnName("BandByHOID");
            builder.Property(e => e.BandByImportId).HasColumnName("BandByImportID");
            builder.Property(e => e.BusPrimaryType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            builder.Property(e => e.BusRetailType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            builder.Property(e => e.BusYearTurnover).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.CashCreditType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            builder.Property(e => e.CreditCollnThru)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("('D')")
                .IsFixedLength();
            builder.Property(e => e.CreditPeriod).HasDefaultValueSql("((0))");
            builder.Property(e => e.CreditPeriodByHo).HasColumnName("CreditPeriodByHO");
            builder.Property(e => e.IsLoanAvailed)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            builder.Property(e => e.MainMerchants).HasMaxLength(100);
            builder.Property(e => e.MarketReputation)
                .HasMaxLength(1)
                 .IsUnicode(false)
                .IsFixedLength();
            builder.Property(e => e.OverdueLimitPerc).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.PartyId).HasColumnName("PartyID");
            builder.Property(e => e.PlannedCft)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("PlannedCFT");
            builder.Property(e => e.SalesLimitByHo)
                .HasColumnType("money")
                .HasColumnName("SalesLimitByHO");
            builder.Property(e => e.SalesLimitByImport).HasColumnType("money");
            builder.Property(e => e.SalesPriceLowVarPerc).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.SalesPriceUpVarPerc).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.ValueofProperty).HasColumnType("money");
            builder.HasOne(d => d.BandByHo).WithMany(p => p.MaCustomerDetailBandByHos)
                          .HasForeignKey(d => d.BandByHoid)
                          .HasConstraintName("FK_MaCustomerDetails_MaCustomerCategories");

        builder.HasOne(d => d.BandByImport).WithMany(p => p.MaCustomerDetailBandByImports)
              .HasForeignKey(d => d.BandByImportId)
              .HasConstraintName("FK_MaCustomerDetails_MaCustomerCategories1");

            builder.HasOne(d => d.Party).WithMany(p => p.MaCustomerDetails)
                .HasForeignKey(d => d.PartyId)
                .HasConstraintName("FK_MaCustomerDetails_Parties");
        }
        }
    }

