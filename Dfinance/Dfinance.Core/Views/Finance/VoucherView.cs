using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Finance
{
    public class VoucherView
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public int? PrimaryVoucherID { get; set; }
        public string? PrimaryVoucherName { get; set; }
        public string? Type { get; set; }
        public bool? Active { get; set; }
        public string? Code { get; set; }
        public byte? DevCode { get; set; }
        public int? CreatedBranchID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? DocumentType { get; set; }
        //public int? Numbering { get; set; }
        public int? RowType { get; set; }
        public int? WorkflowDays { get; set; }
        public int? ApprovalDays { get; set; }
        public bool? ApprovalRequired { get; set; }
        public bool? FinanceUpdate { get; set; }
        public bool? RateUpdate { get; set; }
        public string? ReportPath { get; set; }
        public string ? Culture { get; set; }
        public string? FormatString { get; set; }
        public string? MenuValue { get; set; }
        public int? PostAccountID { get; set; }
        public int? CashAccountID { get; set; }
        public int? CardAccountID { get; set; }
        public int? BankAccountID { get; set; }
        public int? FormID { get; set; }
        public byte? PrecisionValue { get; set; }
        public int? PageID { get;}
        
    }
    public class FillVoucher
    {
        public int ID { get; set; }
        public string? TransactionNo { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Amount { get; set; }
        public bool? Cancelled { get; set; }
        public string? EntryNo { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? AccountName { get; set; }
        public string? Phone { get; set; }
        public bool? Status { get; set; }
        public int? SerialNo { get; set; }
        public string? VATNo { get; set; }
    }

    public class FillVouTranId
    {
        public int ID { get; set; }
        public string? TransactionNo { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Amount { get; set; }
        public bool? Cancelled { get; set; }
        public string? EntryNo { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? AccountName { get; set; }
        public string? Phone { get; set; }
    }
}
