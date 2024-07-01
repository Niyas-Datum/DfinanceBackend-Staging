namespace Dfinance.Core.Views.Finance
{
    public class DayBookView
    {
        public int? ID {  get; set; }
        public int? VoucherID {  get; set; }
        public string? VType {  get; set; }
        public int? BasicVTypeID { get; set; }
        public string? BasicVType { get; set; }
        public DateTime? VDate { get; set; }
        public string? VNo { get; set; }
        public string? Particulars { get; set; }
        public string? CommonNarration { get; set; }
        public string? BillDetails { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public int? CreatedBy { get; set; }
        public string? Posted { get; set; }
        public string? ApprovalStatus { get; set; }
        public bool? IsAutoEntry { get; set; }
        public string? UserName { get; set; }
        public string? TranType { get; set; }
    }
}
