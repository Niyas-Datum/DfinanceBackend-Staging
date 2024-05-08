using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.Core.Infrastructure.Configurations
{

    public class FiTransactionConfiguration : IEntityTypeConfiguration<FiTransaction>
    {
        public void Configure(EntityTypeBuilder<FiTransaction> builder)
        {
            builder.ToTable("FiTransactions");
            builder.HasIndex(e => new { e.CompanyId, e.VoucherId, e.TransactionNo }, "IX_FiTransactions");

            builder.HasIndex(e => new { e.VoucherId, e.CompanyId }, "IX_FiTransactions_1");

            builder.HasIndex(e => new { e.VoucherId, e.TransactionNo, e.CompanyId }, "uq_FiTransactions")
                .IsUnique();
           
            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.AccountId).HasColumnName("AccountID");

            builder.Property(e => e.Action)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            builder.Property(e => e.Active).HasDefaultValueSql("((1))");

            builder.Property(e => e.AddedDate).HasColumnType("datetime");

            builder.Property(e => e.ApprovalStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            builder.Property(e => e.ApproveNote).HasMaxLength(200);

            builder.Property(e => e.ApprovedDate).HasColumnType("datetime");

            builder.Property(e => e.Cancelled).HasDefaultValueSql("((0))");

            builder.Property(e => e.CompanyId).HasColumnName("CompanyID");

            builder.Property(e => e.CostCentreId).HasColumnName("CostCentreID");

            builder.Property(e => e.CurrencyId).HasColumnName("CurrencyID");

            builder.Property(e => e.Date).HasColumnType("datetime");

            builder.Property(e => e.EditedDate).HasColumnType("datetime");

            builder.Property(e => e.EffectiveDate).HasColumnType("datetime");

            builder.Property(e => e.ExchangeRate).HasColumnType("decimal(18, 2)");

            builder.Property(e => e.FinYearId).HasColumnName("FinYearID");

            builder.Property(e => e.InstrumentBank).HasMaxLength(50);

            builder.Property(e => e.InstrumentDate).HasColumnType("datetime");

            builder.Property(e => e.InstrumentNo).HasMaxLength(20);

            builder.Property(e => e.InstrumentType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            builder.Property(e => e.MachineName).HasMaxLength(50);

            builder.Property(e => e.PageId).HasColumnName("PageID");

            builder.Property(e => e.Posted).HasDefaultValueSql("((0))");

            builder.Property(e => e.RefPageTableId).HasColumnName("RefPageTableID");

            builder.Property(e => e.RefPageTypeId).HasColumnName("RefPageTypeID");

            builder.Property(e => e.RefTransId).HasColumnName("RefTransID");

            builder.Property(e => e.ReferenceNo).HasMaxLength(50);

            builder.Property(e => e.StatusId).HasColumnName("StatusID");

            builder.Property(e => e.TransactionNo).HasMaxLength(50);

            builder.Property(e => e.VoucherId).HasColumnName("VoucherID");

            builder.HasOne(d => d.Account)
                .WithMany(p => p.FiTransactions)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_FiTransactions_FiMaAccounts");

            builder.HasOne(d => d.AddedByNavigation)
                .WithMany(p => p.FiTransactionAddedByNavigations)
                .HasForeignKey(d => d.AddedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FiTransactions_MaEmployees");

            builder.HasOne(d => d.ApprovedByNavigation)
                .WithMany(p => p.FiTransactionApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK_FiTransactions_MaEmployees1");

            builder.HasOne(d => d.Company)
                .WithMany(p => p.FiTransactions)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FiTransactions_MaCompanies");

            builder.HasOne(d => d.CostCentre)
                .WithMany(p => p.FiTransactions)
                .HasForeignKey(d => d.CostCentreId)
                .HasConstraintName("FK_FiTransactions_CostCentre");

            builder.HasOne(d => d.Currency)
                .WithMany(p => p.FiTransactions)
                .HasForeignKey(d => d.CurrencyId)
                .HasConstraintName("FK_FiTransactions_Currency");

            builder.HasOne(d => d.FinYear)
                .WithMany(p => p.FiTransactions)
                .HasForeignKey(d => d.FinYearId)
                .HasConstraintName("FK_FiTransactions_tblMaFinYear");

            builder.HasOne(d => d.RefTrans)
                .WithMany(p => p.InverseRefTrans)
                .HasForeignKey(d => d.RefTransId)
                .HasConstraintName("FK_FiTransactions_FiTransactions");

            builder.HasOne(d => d.Status)
                .WithMany(p => p.FiTransactions)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_FiTransactions_MaMisc");

            builder.HasOne(d => d.Voucher)
                .WithMany(p => p.FiTransactions)
                .HasForeignKey(d => d.VoucherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FiTransactions_FiMaVouchers");
        }
    }
}