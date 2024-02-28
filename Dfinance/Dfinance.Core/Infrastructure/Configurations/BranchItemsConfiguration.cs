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
