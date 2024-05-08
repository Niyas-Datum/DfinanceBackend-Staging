using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.Core.Infrastructure.Configurations
{

    public class FiVoucherAllocationsConfiguration : IEntityTypeConfiguration<FiVoucherAllocation>
    {
        public void Configure(EntityTypeBuilder<FiVoucherAllocation> builder)
        {
            builder.ToTable("FiVoucherAllocation");

            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.AccountId).HasColumnName("AccountID");

            builder.Property(e => e.Amount).HasColumnType("money");

            builder.Property(e => e.RefTransId).HasColumnName("RefTransID");

            builder.Property(e => e.Veid).HasColumnName("VEID");

            builder.Property(e => e.Vid).HasColumnName("VID");

            builder.HasOne(d => d.Account)
                .WithMany(p => p.FiVoucherAllocations)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_FiVoucherAllocation_FiMaAccounts");

            builder.HasOne(d => d.RefTrans)
                .WithMany(p => p.FiVoucherAllocationRefTrans)
                .HasForeignKey(d => d.RefTransId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__FiVoucher__RefTr__79A1EB15");

            builder.HasOne(d => d.Ve)
                .WithMany(p => p.FiVoucherAllocations)
                .HasForeignKey(d => d.Veid)
                .HasConstraintName("FK_FiVoucherAllocation_FiTransactionEntries");

            builder.HasOne(d => d.VidNavigation)
                .WithMany(p => p.FiVoucherAllocationVidNavigations)
                .HasForeignKey(d => d.Vid)
                .HasConstraintName("FK_FiMaTransaction_FiVoucherAllocation");
        }
    }
}