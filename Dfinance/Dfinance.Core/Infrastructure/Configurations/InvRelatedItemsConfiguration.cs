using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class InvRelatedItemsConfiguration : IEntityTypeConfiguration<InvRelatedItems>
    {
        public void Configure(EntityTypeBuilder<InvRelatedItems> builder)
        {
            builder.ToTable("InvRelatedItems");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.ItemId1).HasColumnName("ItemID1");
            builder.Property(e => e.ItemId2).HasColumnName("ItemID2");

            //builder.HasOne(d => d.ItemId1Navigation)
            //    .WithMany(p => p.InvRelatedItemItemId1Navigations)
            //    .HasForeignKey(d => d.ItemId1)
            //    .OnDelete(DeleteBehavior.Restrict) 
            //    .HasConstraintName("FK_InvRelatedItems_ItemMaster1");

            //builder.HasOne(d => d.ItemId2Navigation)
            //    .WithMany(p => p.InvRelatedItemItemId2Navigations)
            //    .HasForeignKey(d => d.ItemId2)
            //    .OnDelete(DeleteBehavior.Restrict) 
            //    .HasConstraintName("FK_InvRelatedItems_ItemMaster2");
        }
    }


}
