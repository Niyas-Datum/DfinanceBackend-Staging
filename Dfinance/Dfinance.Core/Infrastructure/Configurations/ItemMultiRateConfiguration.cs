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
    public class ItemMultiRateConfiguration : IEntityTypeConfiguration<ItemMultiRate>
    {
        public void Configure(EntityTypeBuilder<ItemMultiRate> builder)
        {
            builder.ToTable("InvItemMultiRate");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.ItemId).HasColumnName("ItemID");
            builder.Property(e => e.PriceCategoryId).HasColumnName("PriceCategoryID");

            //relationship with PriceCategory
            builder.HasOne(d => d.PriceCategory).WithMany(p => p.MultiRate)
               .HasForeignKey(d => d.PriceCategoryId)
               .HasConstraintName("FK_InvItemMultiRate_MaPriceCategory");

            //relationship with ItemMaster
            builder.HasOne(d => d.Item).WithMany(p => p.MultiRate)
               .HasForeignKey(d => d.ItemId)
               .HasConstraintName("FK_InvItemMultiRate_InvItemMaster");
        }
    }
}
