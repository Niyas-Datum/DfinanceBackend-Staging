using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Validation;

namespace Dfinance.DataModels.Dto.Finance
{
    public class PdcClearingDto
    {
        public int? Id { get; set; }
        [NullValidation(ErrorMessage = "Voucher Number is Mandatory!!")]
        public string? VoucherNo { get; set; }
        [NullValidation(ErrorMessage = "Voucher Date is Mandatory!!")]
        public DateTime? VoucherDate { get; set; }
        public string? Narration { get; set; }
        public AccountNamePopUpDto? BankName { get; set; }
        public List<ChequeDetails> ChequeDetails { get; set; }
       // public ChequeTranDto chequeTranDto { get; set; }
    }
    public class ChequeDetails
    {
        public bool? Selection { get; set; }
        public int? ID { get; set; }
        public int? VEID { get; set; }
        public int? VID { get; set; }
        public int? Posted { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public DateTime? VDate { get; set; }
        public string? BankName { get; set; }
        public int? PartyID { get; set; }
        public int? EntryID { get; set; }
        public string? Party { get; set; } //party
        public string? Description { get; set; }
        public decimal? Debit { get; set; } //debit
        public decimal? Credit { get; set; } //credit
        public int? AccountID { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public string? Status { get; set; }
    }

    //public class ChequeTranDto
    //{
    //    public int? VId { get; set; }
    //    public int? VEId { get; set; }
    //    public int? ChequeId { get; set; }
    //    public string? TranType { get; set; }
    //}
}
