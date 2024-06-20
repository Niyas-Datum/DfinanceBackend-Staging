using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.DataModels.Validation;

namespace Dfinance.DataModels.Dto.Finance
{
    public class PaymentVoucherDto
    {
        public int? Id { get; set; }
        public string? VoucherNo { get; set; }
        public DateTime? VoucherDate { get; set;}
        public string? Narration { get; set;}
        public PopUpDto? CostCentre { get; set;}
        public PopUpDto? Department { get; set;}
        public string ? ReferenceNo { get; set;}
        public DropdownDto? Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public List<AccountDetails>? AccountDetails { get; set;}
        public List<PaymentDetails>? Paydetails { get; set; }
        public List<BillandRef>? BillandRef { get; set; }
        public List<PaymentType>? Cash { get; set; }
        public List<PaymentType>? Card { get; set; }
        public List<PaymentType>? Epay { get; set; }
        public List<ChequeDto>? Cheque { get; set; }
        //public InvTransactionAdditionalDto? FiTransactionAdditional { get; set; }

    }
    public class AccountDetails
    {
        public PopUpDto? AccountCode { get; set; }
        //public string? AccountName { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? Amount { get; set; }  //Debit(PayVou)   //Credit(ReceiptVou)

    }
    public class PaymentDetails
    {
        public string? AccountName { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public string? TransType { get; set; }
    }

    public class PaymentType
    {
        public AccountNamePopUpDto? AccountCode { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public string? TransType { get; set; }   //need to display 
    }

    public class ChequeDto
    {
        public AccountNamePopUpDto? PDCPayable { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public AccountNamePopUpDto? BankName { get; set; }
        public int? ClearingDays { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }


        //tablefield
        public int? VEID { get; set; }
        public string? CardType { get; set; }
        public decimal? Commission { get; set; }
        public int? BankID { get; set; }
        public string? Status { get; set; }
        public int? PartyID { get; set; }
        public string? TransType { get; set; }
    }
    public class BillandRef
    {
        public bool? Selection { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int? PartyInvNo { get; set; }
        public DateTime? PartyInvDate { get; set; }
        public string? Description { get; set; }
        public string? Account { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public decimal? Allocated { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Balance { get; set; }

        //tablefield
        public int? VID { get; set; }
        public int? VEID { get; set; }
        public int? AccountID { get; set; }
    }
}







//}
//public class InvAccountDetailsDto
//{
//    public int AccountId { get; set; }
//    public AccountNamePopUpDto? AccountCode { get; set; }
//    public string? AccountName { get; set; }
//    public string? Description { get; set; }
//    [DecimalValidation(4, ErrorMessage = "Amount is not valid!!")]
//    public decimal? Amount { get; set; }
//    public string? TransType { get; set; }
//    public AccountNamePopUpDto? PayableAccount { get; set; }
//}
//public class InvAdvanceDto
//{
//    public int? VID { get; set; }
//    public int? VEID { get; set; }
//    public string? VNo { get; set; }
//    public DateTime? VDate { get; set; }
//    public string? Description { get; set; }
//    [DecimalValidation(4, ErrorMessage = "BillAmount is not valid!!")]
//    public decimal? BillAmount { get; set; }
//    [DecimalValidation(4, ErrorMessage = "Amount is not valid!!")]
//    public decimal? Amount { get; set; }
//    public int? AccountID { get; set; }
//    public bool? Selection { get; set; }
//    [DecimalValidation(4, ErrorMessage = "Allocated is not valid!!")]
//    public decimal? Allocated { get; set; }
//    public string? Account { get; set; }
//    public string? DrCr { get; set; }
//    public int? PartyInvNo { get; set; }
//    public DateTime? PartyInvDate { get; set; }
//}