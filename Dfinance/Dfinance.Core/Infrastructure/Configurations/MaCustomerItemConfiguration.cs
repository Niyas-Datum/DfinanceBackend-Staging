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
    
    public class MaCustomerItemConfiguration : IEntityTypeConfiguration<MaCustomerItems>
    {
        public void Configure(EntityTypeBuilder<MaCustomerItems> builder)
        {
            builder.Property(e => e.Id).HasColumnName("ID");

            builder.Property(e => e.CommodityId).HasColumnName("CommodityID");

            builder.Property(e => e.PartyId).HasColumnName("PartyID");

            builder.HasOne(d => d.Commodity)
                .WithMany(p => p.MaCustomerItems)
                .HasForeignKey(d => d.CommodityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaCustomerItems_Commodity");

            builder.HasOne(d => d.Party)
                .WithMany(p => p.MaCustomerItems)
                .HasForeignKey(d => d.PartyId)
                .HasConstraintName("FK_MaCustomerItems_Parties");

        }
    }
}
