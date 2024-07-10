using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Item
{
    public class InventoryAgeingView
    {
        public int? VID { get; set; }
        public int? VEID { get; set; }
        public int? AccountID { get; set; }
        public string? VNo { get; set; }
        public DateTime? VDate { get; set; }
        public string? RefNo { get; set; }
        public string? VTYpe { get; set; }
        public string? CommonNarration { get; set; }
        public string? Narration { get; set; }
        public string? Particulars { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? RBalance { get; set; }
        public string? User { get; set; }
    }
    public class InventoryAgeingViews
    {
        public int? AccountID { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
       

    }
}
