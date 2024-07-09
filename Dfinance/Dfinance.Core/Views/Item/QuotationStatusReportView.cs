namespace Dfinance.Core.Views.Item
{
    public class QuotationStatusReportView
    {
        public int? ItemID { get; set; }
        public string? ItemCode {  get; set; }  
        public string? ItemName { get; set;}
        public string? Unit { get; set;}
        public decimal? QtyInQuotation { get; set;}
        public decimal? DeliveredQty { get; set; }
    }
}
