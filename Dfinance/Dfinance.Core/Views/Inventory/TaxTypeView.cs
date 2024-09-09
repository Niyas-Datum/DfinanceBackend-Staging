using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Inventory
{
    public class FillTaxTypeByIdView
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int? SalePurchaseModeID { get; set; }
        public int? TaxAccountID { get; set; }
        public decimal? SalesPerc { get; set; }
        public decimal? PurchasePerc { get; set; }
        public bool? Active { get; set; }   
        public string? Description {  get; set; }
        public int? TaxMiscID { get; set; }
        public int? ReceivableAccountID { get; set; }
        public int? PayableAccountID { get; set; }
        public int? CGSTPayableAccountID { get; set; }
        public int? SGSTPayableAccountID { get; set; }
        public int? CGSTReceivableAccountID { get; set; }
        public int? SGSTReceivableAccountID { get; set; }
        public decimal? CessPerc { get; set; }
        public int? CessPayable { get; set; }
        public int? CessReceivable { get; set; }
        public bool? IsDefault { get; set; }
    }
}
