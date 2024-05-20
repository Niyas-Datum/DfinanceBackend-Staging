using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class TransExpensesConfiguration : IEntityTypeConfiguration<TransExpense>
    {
        public void Configure(EntityTypeBuilder<TransExpense> builder)
        {
            builder.ToTable("TransExpenses");
            builder.HasIndex(e => new { e.TransactionId, e.AccountId }, "IX_TransactionExpenses");

            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.AccountId).HasColumnName("AccountID");

            builder.Property(e => e.Amount).HasColumnType("money");

            builder.Property(e => e.ChargeTypeId).HasColumnName("ChargeTypeID");

            builder.Property(e => e.Description).HasMaxLength(200);

            builder.Property(e => e.PayableAccountId).HasColumnName("PayableAccountID");

            builder.Property(e => e.PreCalculatedAmt).HasColumnType("money");

            builder.Property(e => e.TranType).HasMaxLength(20);

            builder.Property(e => e.TransactionId).HasColumnName("TransactionID");

            builder.Property(e => e.Veid).HasColumnName("VEID");

            builder.HasOne(d => d.Account)
                .WithMany(p => p.TransExpenseAccounts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_TransactionExpenses_FiMaAccounts");

            builder.HasOne(d => d.ChargeType)
                .WithMany(p => p.TransExpenses)
                .HasForeignKey(d => d.ChargeTypeId)
                .HasConstraintName("FK_TransExpenses_MaChargeTypes");

            builder.HasOne(d => d.PayableAccount)
                .WithMany(p => p.TransExpensePayableAccounts)
                .HasForeignKey(d => d.PayableAccountId)
                .HasConstraintName("FK_TransExpenses_FiMaAccounts");

            builder.HasOne(d => d.Transaction)
                .WithMany(p => p.TransExpenses)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK_TransactionExpenses_FiTransactions");

            builder.HasOne(d => d.Ve)
                .WithMany(p => p.TransExpenses)
                .HasForeignKey(d => d.Veid)
                .HasConstraintName("FK_TransExpenses_FiTransactionEntries");
        }
    }
}
