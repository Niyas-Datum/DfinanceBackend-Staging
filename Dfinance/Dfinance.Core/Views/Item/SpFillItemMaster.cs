namespace Dfinance.Core.Views.Item
{
    public class ItemNextCode
    {
        public string Result { get; set; }
    }
    public class SpFillItemMaster
    {
        public int ID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string? ImagePath { get; set; }
        public string? BarCode { get; set; }
    }

    public class SpFillItemMasterById
    {
        public int ID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal? SellingPrice { get; set; }
        public string? OEMNo { get; set; }
        public string? PartNo { get; set; }
        public int? CategoryID { get; set; }
        public string? Manufacturer { get; set; }
        public string? BarCode { get; set; }
        public string? ModelNo { get; set; }
        public string? Unit { get; set; }
        public decimal? ROL { get; set; }
        public string? Remarks { get; set; }
        public bool? IsGroup { get; set; }
        public bool? StockItem { get; set; }
        public bool? Active { get; set; }
        public int? InvAccountID { get; set; }
        public int? PurchaseAccountID { get; }
        public int? CostAccountID { get; set; }
        public int? SalesAccountID { get; set; }
        public decimal? Stock { get; set; }
        public decimal? InvoicedStock { get; set; }
        public decimal? AvgCost { get; set; }
        public decimal? LastCost { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedUserID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedUserID { get; set; }
        public int? BranchID { get; set; }
        public string? Location { get; set; }
        public decimal? CashPrice { get; set; }
        public decimal? ROQ { get; set; }
        public int? CommodityID { get; set; }
        public string? CommodityCode { get; set; }
        public string? Commodity { get; set; }
        public string? ShipMark { get; set; }
        public string? PaintMark { get; set; }
        public int? QualityID { get; set; }
        public string? Quality { get; set; }
        public decimal? Weight { get; set; }
        public int? ParentID { get; set; }
        public string? ParentItem { get; set; }
        public decimal? Margin { get; set; }
        public decimal? PurchaseRate { get; set; }
        public string? Imagepath { get; set; }
        public int? TaxTypeID { get; set; }
        public string? TaxType { get; set; }
        public bool? SizeItem { get; set; }
        public int? ColorID { get; set; }
        public int? BrandID { get; set; }
        public string? ColorName { get; set; }
        public string? BrandName { get; set; }
        public string? OriginName { get; set; }
        public int? OriginID { get; set; }
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

    }
    public class ParentItemPoupView
    {
        public int ID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { set; get; }
    }

    public class BarcodeView
    {
        public long Barcode { get; set; }
    }

    public class ItemHistoryView
    {
        public int VTypeID { get; set; }
        public string? VType { get; set; }
        public int? VID { get; set; }
        public string? VNo { get; set; }
        public DateTime? VDate { get; set; }
        public int? AccountID { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public string? Unit { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; }
    }

    public class CurrentStockView
    {
        public decimal? Stock { get; set; }
    }
    public class TaxDropDownView
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public decimal? SalesPerc { get; set; }
    }
}
