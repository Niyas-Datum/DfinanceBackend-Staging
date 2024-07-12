using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Item
{
    public class ItemExpiryReportView
    {
        public int? ItemID { get; set; }
        public string? ItemCode {  get; set; }
        public string? ItemName { get; set;}
        public string? VNo { get; set;}
        public DateTime? VDate { get; set; }
        public string? Origin {  get; set; }
        public string? Brand {  get; set; }
        public string? Manufacturer {  get; set; }
        public string? Commodity {  get; set; }
        public string? BatchNo { get; set;}
        public DateTime? ManufactureDate { get; set; }
        public DateTime? Expired { get; set; }
        public DateTime? ExpireOn { get; set; }
        public int? Days {  get; set; }
        public decimal? Qty { get; set; } 
        public decimal? Rate { get; set; }
        public string? Value {  get; set; } 
    }
}
