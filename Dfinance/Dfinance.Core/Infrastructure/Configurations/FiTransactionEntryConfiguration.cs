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
    public class FiTransactionEntryConfiguration : IEntityTypeConfiguration<FiTransactionEntry>
    {
        public void Configure(EntityTypeBuilder<FiTransactionEntry> builder)
        {
            builder.ToTable("FiTransactionEntries");
            builder.HasIndex(e => e.TransactionId, "IX_FiTransactionEntries");

            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.AccountId).HasColumnName("AccountID");

            builder.Property(e => e.Amount).HasColumnType("decimal(18, 4)");

            builder.Property(e => e.BankDate).HasColumnType("datetime");

            builder.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

            builder.Property(e => e.DrCr).HasMaxLength(1);

            builder.Property(e => e.DueDate).HasColumnType("datetime");

            builder.Property(e => e.ExchRate).HasColumnType("decimal(18, 2)");

            builder.Property(e => e.ExchangeRate).HasColumnType("money");

            builder.Property(e => e.Fcamount)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("FCAmount");

            builder.Property(e => e.Nature)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            builder.Property(e => e.RefPageTableId).HasColumnName("RefPageTableID");

            builder.Property(e => e.RefPageTypeId).HasColumnName("RefPageTypeID");

            builder.Property(e => e.RefTransId).HasColumnName("RefTransID");

            builder.Property(e => e.RefTransactionId).HasColumnName("RefTransactionID");

            builder.Property(e => e.ReferenceNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.TaxPerc).HasColumnType("decimal(18, 2)");

            builder.Property(e => e.TranType)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.TransactionId).HasColumnName("TransactionID");

            builder.HasOne(d => d.Account)
                .WithMany(p => p.FiTransactionEntries)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionEntries_FiMaAccounts");

            //builder.HasOne(d => d.Currency)
            //    .WithMany(p => p.FiTransactionEntries)
            //    .HasForeignKey(d => d.CurrencyId)
            //    .HasConstraintName("FK__FiTransac__Curre__78ADC6DC");

            //builder.HasOne(d => d.RefPageType)
            //    .WithMany(p => p.FiTransactionEntries)
            //    .HasForeignKey(d => d.RefPageTypeId)
            //    .HasConstraintName("FK_FiTransactionEntries_MaPageTypes");

            builder.HasOne(d => d.RefTrans)
                .WithMany(p => p.InverseRefTrans)
                .HasForeignKey(d => d.RefTransId)
                .HasConstraintName("FK_FiTransactionEntries_FiTransactionEntries");

            builder.HasOne(d => d.Transaction)
                .WithMany(p => p.FiTransactionEntries)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK_FiTransactionEntries_FiTransactions");
        }
    }
}
