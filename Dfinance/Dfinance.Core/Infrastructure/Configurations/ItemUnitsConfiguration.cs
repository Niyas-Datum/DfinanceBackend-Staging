using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Dfinance.Core.Infrastructure.Configurations
{
    public class ItemUnitsConfiguration : IEntityTypeConfiguration<ItemUnits>
    {
        public void Configure(EntityTypeBuilder<ItemUnits> builder)
        {
            builder.ToTable("InvItemUnits");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.HasIndex(e => e.Id, "PK_InvItemUnits").IsUnique();
            builder.Property(e => e.ItemId).HasColumnName("ItemID");
            builder.Property(e => e.BranchId).HasColumnName("BranchID");
            builder.Property(e => e.Active)
                   .IsRequired()
                   .HasDefaultValue(true);

            /**
            * @relation: ItemUnits -> ItemMaster       
            * @connection: one to many 
            * **/
            builder.HasOne(d => d.Item).WithMany(p => p.ItemUnit)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_InvItemUnits_InvItemMaster");

            /**
            * @relation: ItemUnits -> UnitMaster       
            * @connection: one to many 
            * **/
            builder.HasOne(d => d.Units).WithMany(p => p.UnitsItem)
                .HasForeignKey(d => d.Unit)
                .HasConstraintName("FK_InvItemUnits_UnitMaster");

            /**
           * @relation: ItemUnits -> UnitMaster       
           * @connection: one to many 
           * **/
            builder.HasOne(d => d.BasicUnits).WithMany(p => p.BasicUnitItem)
                .HasForeignKey(d => d.BasicUnit)
                .HasConstraintName("FK_InvItemUnits_UnitMaster1");


            /**
           * @relation: ItemUnits -> MaCompany       
           * @connection: one to many 
           * **/
            builder.HasOne(d => d.Branch).WithMany(p => p.ItemUnit)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_MaCompanies_InvItemUnits_BranchID");
        }
    }
}
