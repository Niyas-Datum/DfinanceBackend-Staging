using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Item
{
    public class ROLView
    {
        public long? SlNo {  get; set; }
        public int? ID { get; set; }
        public int? ItemID { get; set; }
        public string? ItemName {  get; set; }  
        public string? Commodity { get; set;}
        public decimal? ROL {  get; set; }
        public decimal? ROQ { get; set;}
        public string? Unit {  get; set; }
        public decimal? Stock { get; set; } 
        public decimal? PurchaseRate { get; set; }
        public int? TaxTypeID { get; set; } 
        public decimal? TaxPerc {  get; set; }  
        public int? TaxAccountID { get; set; }
           
    }
}
