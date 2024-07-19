using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Validation;
using System.Text.Json.Serialization;

namespace Dfinance.DataModels.Dto.Finance
{
    public class JournalDto
    {
        public int? Id { get; set; }
        [NullValidation(ErrorMessage = "Voucher Number is Mandatory!!")]
        public string? VoucherNo { get; set; }
        [NullValidation(ErrorMessage = "Voucher Date is Mandatory!!")]
        public DateTime? VoucherDate { get; set; }
        public string? Narration { get; set; }
        public PopUpDto? CostCentre { get; set; }
        public string? ReferenceNo { get; set; }
        public DropdownDto? Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public List<AccountData>? AccountData { get; set; }
    }
    public class AccountData
    {
        public PopUpDto? AccountCode { get; set; }       
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? Amount { get; set; }  //Debit(PayVou)   //Credit(ReceiptVou)
        public decimal? Debit { get; set; }   //(journalvou)
        public decimal? Credit { get; set; }   //(journalvou)
        public List<BillandRef>? BillandRef { get; set; }
    }
    public class JournalVoucherDto
    {
        public int? Id { get; set; }
        [NullValidation(ErrorMessage = "Voucher Number is Mandatory!!")]
        public string? VoucherNo { get; set; }
        [NullValidation(ErrorMessage = "Voucher Date is Mandatory!!")]
        public DateTime? VoucherDate { get; set; }
        public string? Narration { get; set; }
        public PopUpDto? CostCentre { get; set; }
        public string? ReferenceNo { get; set; }
        public DropdownDto? Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public List<AccountDetails>? AccountDetails { get; set; }
        public BillandRef? BillandRef { get; set; }
             
    }
    

    public class ContraDto : JournalVoucherDto
    {
        [JsonIgnore]
        public new List<BillandRef>? BillandRef { get; set; }
    }

}



