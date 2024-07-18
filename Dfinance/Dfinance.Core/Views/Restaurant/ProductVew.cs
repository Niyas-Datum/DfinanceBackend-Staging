using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views
{
    public class ProductVew
    {
        public int? ID { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set;}
        public string? BarCode { get; set;}
        public decimal? Rate { get; set;}
        public decimal? SellingPrice { get; set;}
        public string? ArabicName { get; set;}
        public int? TaxTypeID { get; set;}
        public decimal? TaxPerc {  get; set;}
        public int? CategoryID { get; set; }
        public int? TaxAccountID { get; set; }
        public string? ImagePath { get; set; }
        public string? ShipMark { get; set; }
        public string? PaintMark { get; set; }
        public string? Unit { get; set;}
        public decimal? DiscountAmt { get; set;}
        public decimal? DiscountPerc { get; set;}
    }
}
