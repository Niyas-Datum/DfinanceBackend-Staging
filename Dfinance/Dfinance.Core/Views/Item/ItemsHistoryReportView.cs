using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Item
{
    public class ItemsHistoryReportView
    {
        public int? VID { get; set; }
        public int? ItemID { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public int? AccountID { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public string? VNo { get; set; }
        public DateTime? VDate { get; set; }
        public string? Unit { get; set; }
        public decimal? QtyIn { get; set; }
        public decimal? RateIn {  get; set; }
        public decimal? AmountIn { get; set; }
        public decimal? QtyOut { get; set; }    
        public decimal? RateOut { get; set; }   
        public decimal? AmountOut { get;set; }
    }
}
