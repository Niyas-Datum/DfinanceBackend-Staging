using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Item
{
    public class QuotationComparisonView
    {
        public string? VNO { get; set; }
        public DateTime? Date { get; set; }
        public string? PartyName { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Qty { get; set; }
        public string? Unit { get; set; }
        public decimal? Factor { get; set; }
        public decimal? BasicQty { get; set; }
        public int? AccountID { get; set; }
        public int? ItemID { get; set; }
        public int? VID { get; set; }

    }
}
