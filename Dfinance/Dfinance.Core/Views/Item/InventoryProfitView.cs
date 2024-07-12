namespace Dfinance.Core.Views.Item
{
    public class InventoryProfitItemView
    {
        public int? ItemID { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? BarCode { get; set; }
        public decimal? SalesQty { get; set; }
        public decimal? SalesRate { get; set; }
        public decimal? SalesAmount { get; set; }
        public decimal? StockQty { get; set; }
        public decimal? StockRate { get; set; }
        public decimal? StockAmount { get; set; }
        public decimal? GrossProfit { get; set; }

    }
    public class InventoryProfitVoucherView
    {
        public int? ID { get; set; }
        public string? VNo { get; set; }
        public DateTime? VDate {  get; set; }
        public string? VType {  get; set; }
        public int? AccountID { get; set; }
        public string? AccountCode { get; set; }   
        public string? AccountName { get; set; }
        public string? ReferenceNo {  get; set; }
        public int? ItemID { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public decimal? SalesQty { get; set; }
        public decimal? SalesUnit {  get; set; }
        public decimal? SalesRate { get; set; }
        public decimal? SalesAmount { get; set; }
        public decimal? StockQty { get; set; }
        public decimal? StockUnit { get; set; }
        public decimal? StockRate { get; set; }
        public decimal? StockAmount { get; set; }
        public decimal? GrossProfit { get; set; }
        public int? SalesMan {  get; set; }  
    }
    public class InventoryProfitVoucherViews
    {
        public int? ID { get; set; }
        public string? VNo { get; set; }
        public DateTime? VDate { get; set; }
        public string? VType { get; set; }
        public int? AccountID { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public string? ReferenceNo { get; set; }
        public decimal? SalesQty { get; set; }
        public decimal? SalesRate { get; set; }
        public decimal? SalesAmount { get; set; }
        public decimal? StockQty { get; set; }
        public decimal? StockRate { get; set; }
        public decimal? StockAmount { get; set; }
        public decimal? GrossProfit { get; set; }
        public decimal? DueAmount { get; set; }
        public int? SalesMan { get; set; }
    }
    public class InventoryProfitPartyView
    {
       
        public int? AccountID { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public int? ItemID { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? BarCode { get; set; }
        public decimal? SalesQty { get; set; }
        public decimal? SalesRate { get; set; }
        public decimal? SalesAmount { get; set; }
        public decimal? StockQty { get; set; }
        public decimal? StockRate { get; set; }
        public decimal? StockAmount { get; set; }
        public decimal? GrossProfit { get; set; }
        public int? SalesMan { get; set; }
    }
    public class InventoryProfitPartyViews
    {

        public int? AccountID { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; } 
        public decimal? SalesQty { get; set; }
        public decimal? SalesRate { get; set; }
        public decimal? SalesAmount { get; set; }
        public decimal? StockQty { get; set; }
        public decimal? StockRate { get; set; }
        public decimal? StockAmount { get; set; }
        public decimal? GrossProfit { get; set; }
        public int? SalesMan { get; set; }
    }
}

