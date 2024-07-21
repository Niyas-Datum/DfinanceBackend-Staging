using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Validation;

namespace Dfinance.DataModels.Dto.Finance
{
    public class DebitCreditDto
    {
        public int? Id { get; set; }
        [NullValidation(ErrorMessage = "Voucher Number is Mandatory!!")]
        public string? VoucherNo { get; set; }
        [NullValidation(ErrorMessage = "Voucher Date is Mandatory!!")]
        public DateTime? VoucherDate { get; set; }
        public string? Reference { get; set; }
        [NullValidation(ErrorMessage = "Party is Mandatory!!")]
        public PopUpDto? Party {  get; set; }
        public DropdownDto? Particulars { get; set; }
        public string? Narration { get; set; }
        public List<AccountDetail> accountDetails { get; set; }
        public List<FillAdvanceDrCrDto>? billandRef { get; set; }

    }
    public class AccountDetail
    {
        public PopUpDto? AccountCode { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? Debit { get; set; }   //creditnote
        public decimal? Credit { get; set; }  //debitnote
    }
    public class FillAdvanceDrCrDto
    {
        public int? VID { get; set; }    //TransactionId
        public int? VEID { get; set; }   //TransactionTableId
        public string? VNo { get; set; }  //voucher code + TransactionNo => InvoiceNo
        public DateTime? VDate { get; set; }  //Transaction Date
        public string? Description { get; set; }  //TransactionEntries(desc)
        public decimal? BillAmount { get; set; }  // amount * exchangerate
        public decimal? Amount { get; set; }     //0
        public int? AccountID { get; set; }    //accId(trans.entries)
        public bool? Selection { get; set; }
        public decimal? Allocated { get; set; }
        public string? Account { get; set; }
        public string? DrCr { get; set; }   //
        public int? PartyInvNo { get; set; }  //EntryNo(trans.addi)
        public DateTime? PartyInvDate { get; set; } //EntryDate(trans.addi)
    }
}
