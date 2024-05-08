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
    public class MaChargeTypeConfiguration : IEntityTypeConfiguration<MaChargeType>
    {
        public void Configure(EntityTypeBuilder<MaChargeType> builder)
        {
            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.AccountId).HasColumnName("AccountID");          
                


            builder.Property(e => e.ChargeType)
                        .HasMaxLength(50)
                        .IsUnicode(false);

            builder.Property(e => e.CostingType)
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .IsFixedLength();

            builder.Property(e => e.CreatedBranchId).HasColumnName("CreatedBranchID");

            builder.Property(e => e.CreatedOn).HasColumnType("datetime");

            builder.Property(e => e.DueDaysRemarks)
                        .HasMaxLength(200)
                        .IsUnicode(false);

            builder.Property(e => e.IsLcrelatedExp).HasColumnName("IsLCRelatedExp");

            builder.Property(e => e.Mode)
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .IsFixedLength();

            builder.Property(e => e.PayableAccountId).HasColumnName("PayableAccountID");

            builder.Property(e => e.Type)
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .IsFixedLength();

            builder.HasOne(d => d.Account)
                        .WithMany(p => p.MaChargeTypeAccounts)
                        .HasForeignKey(d => d.AccountId)
                        .HasConstraintName("FK_MaChargeTypes_FiMaAccounts1");

            builder.HasOne(d => d.CreatedBranch)
                        .WithMany(p => p.MaChargeTypes)
                        .HasForeignKey(d => d.CreatedBranchId)
                        .HasConstraintName("FK_MaChargeTypes_MaCompanies");

            builder.HasOne(d => d.CreatedByNavigation)
                        .WithMany(p => p.MaChargeTypes)
                        .HasForeignKey(d => d.CreatedBy)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MaChargeTypes_MaEmployees");

            builder.HasOne(d => d.PayableAccount)
                        .WithMany(p => p.MaChargeTypePayableAccounts)
                        .HasForeignKey(d => d.PayableAccountId)
                        .HasConstraintName("FK_MaChargeTypes_FiMaAccounts");
        }
    }
}
