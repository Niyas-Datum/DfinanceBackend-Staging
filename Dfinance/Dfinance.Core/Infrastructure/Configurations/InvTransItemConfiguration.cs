using Dfinance.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dfinance.Core.Infrastructure.Configurations
{
    public class InvTransItemConfiguration:IEntityTypeConfiguration<InvTransItems>
    {
        public void Configure(EntityTypeBuilder <InvTransItems> builder) 
        {
            builder.ToTable("InvTransItems");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.TransactionId).HasColumnName("TransactionID");
            builder.Property(e => e.ItemId).HasColumnName("ItemID");
            builder.Property(e => e.RefTransId1).HasColumnName("RefTransID1");
            builder.Property(e => e.MasterMiscId1).HasColumnName("MasterMiscID1");
            builder.Property(e => e.InvAvgCostId).HasColumnName("InvAvgCostID");
            builder.Property(e => e.CommodityId).HasColumnName("CommodityID");
            builder.Property(e => e.AccountId).HasColumnName("AccountID");
            builder.Property(e => e.TransactionEntryId).HasColumnName("TransactionEntryID");
            builder.Property(e => e.AvgCostId).HasColumnName("AvgCostID");
            builder.Property(e => e.RefTransItemId).HasColumnName("RefTransItemID");
            builder.Property(e => e.MeasuredById).HasColumnName("MeasuredByID");
            builder.Property(e => e.RefId).HasColumnName("RefID");
            builder.Property(e => e.InLocId).HasColumnName("InLocID");
            builder.Property(e => e.OutLocId).HasColumnName("OutLocID");
            builder.Property(e => e.TaxTypeId).HasColumnName("TaxTypeID");
            builder.Property(e => e.TaxAccountId).HasColumnName("TaxAccountID");
            builder.Property(e => e.SizeMasterId).HasColumnName("SizeMasterID");
            builder.Property(e => e.FocQty).HasColumnName("FOCQty");
            builder.Property(e => e.GroupItemId).HasColumnName("GroupItemID");
            builder.Property(e => e.PriceCategoryId).HasColumnName("PriceCategoryID");
            builder.Property(e => e.PrintedMrp).HasColumnName("PrintedMRP");
            builder.Property(e => e.PtsRate).HasColumnName("PTSRate");
            builder.Property(e => e.PtrRate).HasColumnName("PTRRate");
            builder.Property(e => e.StockItemId).HasColumnName("StockItemID");
            builder.Property(e => e.CostAccountId).HasColumnName("CostAccountID");
            builder.Property(e => e.CgstPerc).HasColumnName("CGSTPerc");
            builder.Property(e => e.SgstPerc).HasColumnName("SGSTPerc");
            builder.Property(e => e.SgstValue).HasColumnName("SGSTValue");
            builder.Property(e => e.CgstValue).HasColumnName("CGSTValue");
            builder.Property(e => e.Hsn).HasColumnName("HSN");
            builder.Property(e => e.BrandId).HasColumnName("BrandID");
            builder.Property(e => e.CessAccountId).HasColumnName("CessAccountID");
            builder.Property(e => e.ParentId).HasColumnName("ParentID");

            /**
            * @relation: InvTransItems -> FiMaAccounts               
            * @connection: one to many 
            * **/
            builder.HasOne(d => d.FiMaAccount)
                    .WithMany(p => p.InvTransItems)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_InvTransItems_FiMaAccounts");

            /**
           * @relation: InvTransItems -> Category               
           * @connection: one to many 
           * **/
            builder.HasOne(d => d.Commodity)
                   .WithMany(p => p.InvTransItems)
                   .HasForeignKey(d => d.CommodityId)
                   .HasConstraintName("FK_InvTransItems_Commodity");

            /**
           * @relation: InvTransItems -> ItemMaster               
           * @connection: one to many 
           * **/
            builder.HasOne(d => d.Item)
                   .WithMany(p => p.InvTransItemItems)
                   .HasForeignKey(d => d.ItemId)
                   .HasConstraintName("FK_InvTransItems_InvItemMaster");

            /**
         * @relation: InvTransItems -> ItemMaster               
         * @connection: one to many 
         * **/
            builder.HasOne(d => d.GroupItem)
                   .WithMany(p => p.InvTransItemGroupItems)
                   .HasForeignKey(d => d.GroupItemId)
                   .HasConstraintName("FK_InvTransItems_InvItemMaster1");

            /**
         * @relation: InvTransItems -> ItemMaster               
         * @connection: one to many 
         * **/
            builder.HasOne(d => d.StockItem)
                   .WithMany(p => p.InvTransItemStockItems)
                   .HasForeignKey(d => d.StockItemId)
                   .HasConstraintName("FK_InvItemMaster_InvTransItems_StockItemID");

            /**
       * @relation: InvTransItems -> InvTransItems               
       * @connection: one to many 
       * **/
            builder.HasOne(d => d.Ref)
                   .WithMany(p => p.InverseRef)
                   .HasForeignKey(d => d.RefId)
                   .HasConstraintName("FK_InvTransItems_InvTransItems");

            /**
      * @relation: InvTransItems -> FiTransactions               
      * @connection: one to many 
      * **/
            builder.HasOne(d => d.Transaction)
                    .WithMany(p => p.InvTransItems)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("FK_InvTransItems_FiTransactions");

            builder.HasOne(d => d.SizeMaster)
                    .WithMany(p => p.InvTransItems)
                    .HasForeignKey(d => d.SizeMasterId)
                    .HasConstraintName("FK_InvTransItems_InvSizeMaster");
        }
    }
}
