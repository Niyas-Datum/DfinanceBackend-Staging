using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Finance
{
    public class FillVoucherView
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
       
        public int? PrimaryVoucherID { get; set; }
        public string? PrimaryVoucherName { get; set; }
        public string? Type { get; set; }
        public bool? Active { get; set; }
        public string? Code { get; set; }
        public byte? DevCode { get; set; }
        public int CreatedBranchID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? DocumentType { get; set; }
        public string? DocumentTypeName { get; set; }
      
        public int? Numbering { get; set; }
        public bool? FinanceUpdate { get; set; }
        public bool? RateUpdate { get; set; }
        public int? RowType { get; set; }
        public bool? ApprovalRequired { get; set; }
      
        public int? WorkflowDays { get; set; }
        public int? ApprovalDays { get; set; }

        public string? Nature {  get; set; }
        public byte? ModuleType { get; set; }
        public string? ReportPath { get; set; }
      
    }
    
}
