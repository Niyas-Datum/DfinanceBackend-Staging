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
    public class FimaUniqueAccountConfiguration : IEntityTypeConfiguration<FimaUniqueAccount>
    {
        public void Configure(EntityTypeBuilder<FimaUniqueAccount> builder)
        {

            builder.HasKey(e => e.Keyword)
                    .HasName("PK_FiMaUniqueAccounts");

            builder.ToTable("FIMaUniqueAccounts");

            builder.Property(e => e.Keyword).HasMaxLength(30);

            builder.Property(e => e.AccId).HasColumnName("AccID");

            builder.HasOne(d => d.Acc)
                            .WithMany(p => p.FimaUniqueAccounts)
                            .HasForeignKey(d => d.AccId)
                            .OnDelete(DeleteBehavior.SetNull)
                            .HasConstraintName("FK_FiMaUniqueAccounts_FiMaAccounts");

        }
    }
}
