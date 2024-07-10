using Dfinance.Core.Views.Inventory;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<FillAdvanceView>? billandRef { get; set; }

    }
    public class AccountDetail
    {
        public PopUpDto? AccountCode { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? Debit { get; set; }   //creditnote
        public decimal? Credit { get; set; }  //debitnote
    }
    
}
