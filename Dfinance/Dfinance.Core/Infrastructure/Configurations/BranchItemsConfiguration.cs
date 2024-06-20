using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class BranchItemsConfiguration: IEntityTypeConfiguration<BranchItems>
    {
        public void Configure(EntityTypeBuilder<BranchItems> builder)
        {
            builder.ToTable("InvBranchItems");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.BranchId).HasColumnName("BranchID");
            builder.Property(e => e.ItemId).HasColumnName("ItemID");
            builder.Property(e => e.TaxTypeId).HasColumnName("TaxTypeID");
            builder.Property(e => e.PurchaseRate).HasColumnName("PurchaseRate");
            builder.Property(e => e.SellingPrice).HasColumnName("SellingPrice");
            builder.Property(e => e.ROL).HasColumnName("ROL");
            builder.Property(e => e.ROQ).HasColumnName("ROQ");
            builder.Property(e => e.RackLocation).HasColumnName("RackLocation");

            //relationship with MaCompany
            builder.HasOne(d => d.Branch).WithMany(p => p.BranchItems)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_InvBranchItems_MaCompanies");

            //relationship with ItemMaster
            builder.HasOne(d => d.Item).WithMany(p => p.BranchItems)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_InvBranchItems_InvItemMaster");

        }
    }
}
