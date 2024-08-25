using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class FiMaVouchersConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {

            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.PrimaryVoucherId).HasColumnName("PrimaryVoucherID");
            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");
            builder.Property(e => e.CashAccountId).HasColumnName("CashAccountID");
            builder.Property(e => e.CardAccountId).HasColumnName("CardAccountID");
            builder.Property(e => e.PostAccountId).HasColumnName("PostAccountID");
            builder.Property(e => e.BankAccountId).HasColumnName("BankAccountID");
            builder.Property(e => e.FormId).HasColumnName("FormID");


            builder.HasOne(d => d.CashAccount).WithMany(p => p.FiMaVoucherCashAccounts)
                .HasForeignKey(d => d.CashAccountId)
                .HasConstraintName("FK__FiMaVouch__CashA__092E4768");

            builder.HasOne(d => d.CreatedBranch).WithMany(p => p.FiMaVouchers)
                .HasForeignKey(d => d.CreatedBranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FiMaVouchers_MaCompanies");

            builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FiMaVouchers)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FiMaVouchers_MaEmployees");

            builder.HasOne(d => d.BankAccount).WithMany(p => p.FiMaBankAcount)
                .HasForeignKey(d => d.BankAccountId)
                .HasConstraintName("FK__FiMaVouch__BankA__0C0AB413");

            builder.HasOne(d => d.CardAccount).WithMany(p => p.FiMaCardAccount)
                .HasForeignKey(d => d.CardAccountId)
                .HasConstraintName("FK__FiMaVouch__CardA__0A226BA1");

            builder.HasOne(d => d.PostAccount).WithMany(p => p.FiMaPostAccount)
              .HasForeignKey(d => d.PostAccountId)
              .HasConstraintName("FK__FiMaVouch__PostA__0B168FDA");

            builder.HasOne(d => d.NumberingNavigation).WithMany(p => p.FiMaVouchers)
              .HasForeignKey(d => d.Numbering)
              .HasConstraintName("FK_FiMaVouchers_MaNumbering");

            builder.HasOne(d => d.Form).WithMany(p => p.FiMaVouchers)
                .HasForeignKey(d => d.Id)
                .HasConstraintName("FK_FiMaVouchers_TaxFormMaster");

            builder.HasOne(d => d.PrimaryVoucher)
                     .WithMany(p => p.FiMaVouchers)
                     .HasForeignKey(d => d.PrimaryVoucherId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("FK_FiMaVouchers_FiPrimaryVouchers");





        }
    }

}
