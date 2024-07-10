using Dfinance.Core.Views.Finance;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<CheqDetailView> ChequeDetails { get; set; }
       // public ChequeTranDto chequeTranDto { get; set; }
    }
    //public class ChequeDetails
    //{
    //    public bool? Selection { get; set; }
    //    public string? ChequeNo { get; set; }
    //    public DateTime? ChequeDate { get; set; }
    //    public DateTime? VDate { get; set; }
    //    public string? BankName { get; set; }   
    //    public string? CustomerSupplier { get; set;} //party
    //    public decimal? Received {  get; set; } //debit
    //    public decimal? Issued { get; set; } //credit
    //    public string? Description { get; set; }
    //    public string? AccountCode { get; set; }
    //    public string? AccountName { get; set; }
    //    public string? Status { get; set;}

    //    public int? AccountId { get; set; } //not bank accid
    //    public int? RefTransID { get; set; } //enter into tranentries
    //}

    //public class ChequeTranDto
    //{
    //    public int? VId { get; set; }
    //    public int? VEId { get; set; }
    //    public int? ChequeId { get; set; }
    //    public string? TranType { get; set; }
    //}
}
