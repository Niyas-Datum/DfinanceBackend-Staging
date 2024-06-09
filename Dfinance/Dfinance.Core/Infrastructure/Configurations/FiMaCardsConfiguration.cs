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
    public class FiMaCardsConfiguration: IEntityTypeConfiguration<FiMaCards>
    {
        public void Configure(EntityTypeBuilder<FiMaCards> builder)
        {
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.AccountId).HasColumnName("AccountID");
            builder.Property(e => e.Description).HasMaxLength(50);
        }

    }
}
