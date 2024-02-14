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
    public class FiAccountsListConfiguration:IEntityTypeConfiguration<FiAccountsList>
    {
        public void Configure(EntityTypeBuilder<FiAccountsList> builder)
        {
            builder.ToTable("FiAccountsList");
            builder.HasIndex(e => new { e.ListId, e.AccountId }, "IX_FiAccountsList").IsUnique();

            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.AccountId).HasColumnName("AccountID");
            builder.Property(e => e.BranchId).HasColumnName("BranchID");
            builder.Property(e => e.ListId).HasColumnName("ListID");
        }


    }
}
