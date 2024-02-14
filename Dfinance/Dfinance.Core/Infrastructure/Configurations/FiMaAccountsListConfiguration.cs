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
    public class FiMaAccountsListConfiguration:IEntityTypeConfiguration<FiMaAccountsList>
    {
        public void Configure(EntityTypeBuilder<FiMaAccountsList> builder)
        {
            builder.ToTable("FiMaAccountsList");

            builder.HasIndex(e => e.Description, "IX_FiMaAccountsList").IsUnique();

            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.Description).HasMaxLength(50);
        }
    }
}
