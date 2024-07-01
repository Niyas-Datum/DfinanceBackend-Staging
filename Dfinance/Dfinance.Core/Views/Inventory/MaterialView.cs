using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Inventory
{
    public class MaterialView
    {
    }
    public class FillMaVouchersUsingPageIDView
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
        public DateTime? CreatedOn { get; set; }
        public int? DocumentType { get; set; }
        public int? RowType { get; set; }
        public int? WorkFlowDays { get; set; }
        public int? ApprovalDays { get; set; }
        public bool? ApprovalRequired { get; set; }
        public bool? FinanceUpdate { get; set; }
        public bool? RateUpdate { get; set; }
        public string? ReportPath { get; set; }
        public string? Culture { get; set;}
        public string? FormatString { get; set; }
        public byte? PrecisionValue { get; set; }
        public string? MenuValue { get; set; }
        public int? PostAccountID { get; set; }
        public int? CashAccountID { get; set; }
        public int? CardAccountID { get; set; }
        public int? BankAccountID { get; set; }
        public int? FormID { get; set; }
        public int? PageID { get; set; }
    }


}
