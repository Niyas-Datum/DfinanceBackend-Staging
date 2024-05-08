using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class InvUniqueItemConfiguration : IEntityTypeConfiguration<InvUniqueItems>
    {
        public void Configure(EntityTypeBuilder<InvUniqueItems> builder)
        {
            builder.ToTable("InvUniqueItems");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.ItemId).HasColumnName("ItemID");
            builder.Property(e => e.TransItemId).HasColumnName("TransItemID");
            builder.Property(e => e.TransactionId).HasColumnName("TransactionID");
            builder.Property(e => e.UniqueNo).HasMaxLength(100);

            //relationship:InvUniqueItems->itemmaster
            builder.HasOne(d => d.Item)
                   .WithMany(p => p.InvUniqueItems)
                   .HasForeignKey(d => d.ItemId)
                   .HasConstraintName("FK_InvUniqueItems_InvItemMaster");

            //relationship:InvUniqueItems->InvTransItems
            builder.HasOne(d => d.TransItem)
                   .WithMany(p => p.InvUniqueItems)
                   .HasForeignKey(d => d.TransItemId)
                   .HasConstraintName("FK_InvUniqueItems_InvTransItems");

            //relationship:InvUniqueItems->FiTransactions
            builder.HasOne(d => d.Transaction)
                   .WithMany(p => p.InvUniqueItems)
                   .HasForeignKey(d => d.TransactionId)
                   .HasConstraintName("FK_InvUniqueItems_FiTransactions");
        }
    }
}
