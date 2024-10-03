using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Inventory.SalesInvoice
{
    public class BatchNoPopupView
    {
        public string? BatchNo {  get; set; }
        public decimal? Qty { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? PrintedMRP { get; set; }
    }
}
