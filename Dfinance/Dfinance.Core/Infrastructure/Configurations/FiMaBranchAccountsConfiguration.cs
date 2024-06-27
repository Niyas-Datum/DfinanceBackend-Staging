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
    public class FiMaBranchAccountsConfiguration : IEntityTypeConfiguration<FiMaBranchAccounts>
    {
        public void Configure(EntityTypeBuilder<FiMaBranchAccounts> builder)
        {
            builder.ToTable("FiMaBranchAccounts");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.AccountId).HasColumnName("AccountID");
            builder.Property(e => e.BranchId).HasColumnName("BranchID");
            builder.HasOne(d => d.Account)
                             .WithMany(p => p.FiMaBranchAccounts)
                             .HasForeignKey(d => d.AccountId)
                             .HasConstraintName("FK_FiMaBranchAccounts_FiMaAccounts");

            builder.HasOne(d => d.Branch)
                .WithMany(p => p.BranchAccounts)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_FiMaBranchAccounts_MaCompanies");

        }
    }
}
