

using System.ComponentModel.DataAnnotations;

namespace Dfinance.Core.Domain
{
    public class ItemMaster
    {
        public int Id { get; set; }       
        public string ItemCode { get; set; }     
        public string ItemName { get; set; }
        public decimal? SellingPrice { get; set; }
        public string? OEMNo { get; set; }
        public string? PartNo { get; set; }
        public int? CategoryId { get; set; }
        public string? Manufacturer { get; set; }
        public string? BarCode { get; set; }
        public string? ModelNo { get; set; }
        public string Unit { get; set; }
        public decimal? ROL { get; set; }
        public string? Remarks { get; set; }
        public bool IsGroup { get; set; }
        public bool StockItem { get; set; }
        public bool Active { get; set; }
        public int? InvAccountId { get; set; }
        public int? CostAccountId { get; set; }
        public int? PurchaseAccountId { get; set; }
        public int? SalesAccountId { get; set; }
        public decimal? Stock { get; set; }
        public decimal? InvoicedStock { get; set; }
        public decimal? AvgCost { get; set; }
        public decimal? LastCost { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedUserId { get; set; }
        public int? ModifiedUserId { get; set; }
        public int? BranchId { get; set; }
        public string? Location { get; set; }
        public decimal? CashPrice { get; set; }
        public decimal? CreditPrice { get; set; }
        public decimal? ROQ { get; set; }
        public int? CommodityId { get; set; }
        public string? ShipMark { get; set; }
        public string? PaintMark { get; set; }
        public int? QualityId { get; set; }
        public decimal? Weight { get; set; }
        public int? ParentId { get; set; }
        public string? ImagePath { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? Margin { get; set; }
        public string? Origin { get; set; }
        public int? TaxTypeId { get; set; }
        public bool? SizeItem { get; set; }
        public int? BrandId { get; set; }
        public int? ColorId { get; set; }
        public int? OriginId { get; set; }
        public bool? IsExpiry { get; set; }
        public int? ExpiryPeriod { get; set; }
        public int? BarcodeDesignId { get; set; }
        public bool? IsFinishedGood { get; set; }
        public bool? IsRawMaterial { get; set; }
        public bool? IsUniqueItem { get; set; }
        public string? PurchaseUnit { get; set; }
        public string? SellingUnit { get; set; }
        public decimal? MarginValue { get; set; }
        public string? ArabicName { get; set; }
        public string? HSN { get; set; }
        public decimal? ItemDisc { get; set; }
        public decimal? MRP { get; set; }

        //relationships
        //one to many realtionship self
        public virtual ItemMaster ItemParent { get; set; }
        public ICollection<ItemMaster> Items { get; set;}

        //realtionship with Commodity table
        public virtual Categories CategoryItem { get; set; }

        //relationship with MaMisc
        public virtual MaMisc MaMiscItem { get; set; }

        //relationship with FiMaAccounts
        public virtual FiMaAccount FiMaAccountItem { get; set; }
        
        //relationship with itemunits
        public ICollection<ItemUnits> ItemUnit {  get; set; }

        //relationship with BranchItems
        public ICollection<BranchItems> BranchItems { get; set; }

        public ICollection<ItemMultiRate> MultiRate {  get; set; }

    }
}
