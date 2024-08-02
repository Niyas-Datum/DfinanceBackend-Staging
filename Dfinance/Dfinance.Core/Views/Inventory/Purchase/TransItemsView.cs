namespace Dfinance.Core.Views.Inventory.Purchase
{
    public class TransItemsView
    {
        public int ID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string? Location { get; set; }
        public string? Unit { get; set; }
        public decimal? Stock { get; set; }
        public decimal? Rate { get; set; }
        // public decimal? WholeSaleRate { get; set;}
        public decimal? PurchaseRate { get; set; }
        public decimal? TaxPerc { get; set; }
        public int? TaxTypeID { get; set; }
        public decimal? Factor { get; set; }
        public int? TaxAccountID { get; set; }
        public decimal? AvgCost { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? ArabicName { get; set; }
        public string? PartNo { get; set; }
        public string? OEMNo { get; set; }
        public string? Remarks { get; set; }
        public int? CostAccountID { get; set; }
        public int? BrandID { get; set; }
        public decimal? PrintedRate { get; set; }
        public string? BrandName { get; set; }
        public decimal? DiscountAmt { get; set; }
        public decimal? DiscountPerc { get; set; }
        public string? BarCode { get; set; }
        public int? CategoryID { get; set; }
        public int? Qty { get; set; }
        public string? ModelNo { get; set; }
        public decimal? SellingPrice { get; set; }
    }

    public class CommandTextView
    {
        public string commandText { get; set; }
    }
    public class QtyView
    {
        public decimal? Stock { get; set; }
    }
    public class NextBatchNoView
    {
        public double nextBatchNo { get; set; }
    }
    public class Fillvoucherview
    {
        public int ID { get; set; }
        public string? TransactionNo { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Amount { get; set; }
        public bool? cancelled { get; set; }
        public string? EntryNo { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? AccountName { get; set; }
        public string? Phone { get; set; }
        public bool? Status { get; set; }
        public int? SerialNo { get; set; }
        public string? VATNo { get; set; }
    }
    public class FillVoucherWithTrnasId
    {
        public int ID { get; set; }
        public string? TransactionNo { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Amount { get; set; }
        public bool? cancelled { get; set; }
        public string? EntryNo { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? AccountName { get; set; }
        public string? Phone { get; set; }
        
    }
    public class ItemTransaction// for tooltip in itemgrid
    {
        public string VNo { get; set; }
        public DateTime VDate { get; set; }
        public string? Customer { get; set; }
        public decimal? Rate { get; set; }
    }

    public class PriceCategoryPopUp//used in pricecategory pop up in item grid
    {
        public int ID { get; set; }
        public string? PriceCategory { get; set; }
        public decimal? Perc { get; set; }
        public decimal? Rate { get; set; }
    }
    public class UniqueItemView
    {
        public long ID { set; get; }
        public string? UniqueNo { get; set; }
    }
}
