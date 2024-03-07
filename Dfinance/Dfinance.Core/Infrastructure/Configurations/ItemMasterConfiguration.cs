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
    public class ItemMasterConfiguration: IEntityTypeConfiguration<ItemMaster>
    {
        public void Configure(EntityTypeBuilder<ItemMaster> builder)
        {
            builder.ToTable("InvItemMaster");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.HasIndex(e => e.Id, "PK_InvItemMaster").IsUnique();
            builder.HasIndex(e => e.ItemCode, "IX_InvItemMaster").IsUnique();
            builder.Property(e => e.CategoryId).HasColumnName("CategoryID");
            builder.Property(e => e.InvAccountId).HasColumnName("InvAccountID");
            builder.Property(e => e.CostAccountId).HasColumnName("CostAccountID");
            builder.Property(e => e.PurchaseAccountId).HasColumnName("PurchaseAccountID");
            builder.Property(e => e.SalesAccountId).HasColumnName("SalesAccountID");
            builder.Property(e => e.CreatedUserId).HasColumnName("CreatedUserID");
            builder.Property(e => e.ModifiedUserId).HasColumnName("ModifiedUserID");
            builder.Property(e => e.BranchId).HasColumnName("BranchID");                     
            builder.Property(e => e.CommodityId).HasColumnName("CommodityID");
            builder.Property(e => e.QualityId).HasColumnName("QualityID");
            builder.Property(e => e.ParentId).HasColumnName("ParentID");
            builder.Property(e => e.TaxTypeId).HasColumnName("TaxTypeID");
            builder.Property(e => e.BrandId).HasColumnName("BrandID");
            builder.Property(e => e.ColorId).HasColumnName("ColorID");
            builder.Property(e => e.OriginId).HasColumnName("OriginID");
            builder.Property(e => e.BarcodeDesignId).HasColumnName("BarcodeDesignID");
            builder.Property(e => e.BarCode).HasMaxLength(100);
            builder.Property(e => e.ImagePath).HasMaxLength(200);
            builder.Property(e => e.ModelNo).HasMaxLength(50);
            builder.Property(e => e.PartNo).HasMaxLength(100);
            builder.Property(e => e.ShipMark).HasMaxLength(50);
            builder.Property(e => e.Location).HasMaxLength(50);
            builder.Property(e => e.PaintMark).HasMaxLength(50);
            builder.Property(e => e.Origin).HasMaxLength(100);
            builder.Property(e => e.ArabicName).HasMaxLength(200);

            /**
             * @relation: ItemMaster -> ItemMaster               
             * @connection: one to many 
             * **/
            builder.HasOne(d => d.ItemParent).WithMany(p => p.Items)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_InvItemMaster_InvItemMaster");

            /**
            * @relation: ItemMaster -> Commodity       
            * @connection: one to many 
            * **/
            builder.HasOne(d => d.CategoryItem).WithMany(p => p.Items)
                .HasForeignKey(d => d.CommodityId)
                .HasConstraintName("FK_InvItemMaster_Commodity");

            /**
           * @relation: ItemMaster -> MaMisc           
           * @connection: one to many 
           * **/
            builder.HasOne(d => d.MaMiscItem).WithMany(p => p.Items)
                .HasForeignKey(d => d.OriginId)
                .HasConstraintName("FK_InvItemMaster_MaMisc3");

            /**
          * @relation: ItemMaster -> FiMaAccounts           
          * @connection: one to many 
          * **/
            builder.HasOne(d => d.FiMaAccountItem).WithMany(p => p.Items)
                .HasForeignKey(d => d.InvAccountId)
                .HasConstraintName("FK_ItemMaster_FiMaAccounts");

            /**
         * @relation: ItemMaster -> FiMaAccounts           
         * @connection: one to many 
         * **/
            builder.HasOne(d => d.FiMaAccountItem).WithMany(p => p.Items)
                .HasForeignKey(d => d.InvAccountId)
                .HasConstraintName("FK_ItemMaster_FiMaAccounts1");

            /**
       * @relation: ItemMaster -> FiMaAccounts           
       * @connection: one to many 
       * **/
            builder.HasOne(d => d.FiMaAccountItem).WithMany(p => p.Items)
                .HasForeignKey(d => d.CostAccountId)
                .HasConstraintName("FK_ItemMaster_FiMaAccounts2");

            /**
       * @relation: ItemMaster -> FiMaAccounts           
       * @connection: one to many 
       * **/
            builder.HasOne(d => d.FiMaAccountItem).WithMany(p => p.Items)
                .HasForeignKey(d => d.PurchaseAccountId)
                .HasConstraintName("FK_ItemMaster_FiMaAccounts3");

        }
    }
}
