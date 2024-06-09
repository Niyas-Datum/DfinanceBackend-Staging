namespace Dfinance.Core.Views.Inventory.Purchase
{
    public class PurchaseReportView
    {
        public int? SlNo { get; set; }
        public int? VID { get; set; }
        public string? Type { get; set; }
        public string? VType { get; set; }
        public string? VNo { get; set; }
        public DateTime? VDate { get; set; }
        public string? ReferenceNo { get; set; }
        public int AccountID { get; set; }
        public string? Particulars { get; set; }
        public int? TaxFormID { get; set; }
        public int? ModelID { get; set; }
        public string? CounterName { get; set; }
        public decimal? Qty { get; set; }
        public string? Unit { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public long? Staff { get; set; }
        public long? AddedBy { get; set; }
        public string? Area { get; set; }
        public string? VATNO { get; set; }
        public string? PartyInvNo { get; set; }

    }
    public class PurchaseReportViews
    {
        public int? VID { get; set; }
        public bool? IsGroup { get; set; }
        public int? AccountID { get; set; }
        public string? Particulars { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
    }
}
