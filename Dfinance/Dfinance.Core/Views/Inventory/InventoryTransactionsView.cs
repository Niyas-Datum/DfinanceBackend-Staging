using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Inventory
{
    public class InventoryTransactionsView
    {
        public int? ID { get; set; }
        public int? VTypeID { get; set; }
        public string? VType { get; set; }
        public int? BasicVTypeID { get; set; }
        public string? BasicVType { get;set; }
        public string? VNo {  get; set; }
        public DateTime VDate { get; set; }
        public string? AccountName { get; set;}
        public string? CostCentre { get; set;}
        public decimal? Amount {  get; set; }
        public int? SlNo { get; set; }
        public string? RefNo { get;set;}
        public string? CommonNarration { get; set; }
        public bool? IsAutoEntry { get; set; }
        public bool? IsPDC { get; set; }
        public bool? Posted { get; set; }
        public bool? Cancelled { get; set; }
        public bool? Active { get; set;}
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; } 
        public string? DocType {  get; set; }   
        public string? ApprovalStatus { get; set;}
        public string? Status { get; set; }
        public string Url { get; set; }

    }
}
