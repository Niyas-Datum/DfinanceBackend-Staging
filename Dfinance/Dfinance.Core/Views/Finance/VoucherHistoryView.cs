using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Finance
{
    public class VoucherHistoryView
    {
        public string? Voucher {  get; set; }
        public int? Parent {  get; set; }   
        public int? VID { get; set; }
        public string? VNo {  get; set; }   
        public string? RefNo {  get; set; } 
        public DateTime? VDate { get; set; }    
        public int? VoucherID { get; set; } 
        public string? Account {  get; set; }   
        public int? ItemID { get; set; }    
        public string? ItemName { get; set;}
        public string? ItemCode { get; set;}
        public string? Unit { get; set;}
        public decimal? Qty { get; set; }
        public decimal? RefQty { get; set; }
        public decimal? Rate { get;set; }
        public int? AccountID { get; set;}

    }
}
