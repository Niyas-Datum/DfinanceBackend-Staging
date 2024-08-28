using System;
using System.Collections.Generic;

namespace Dfinance.Core.Domain
{
    public partial class MaTaxDetail
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public decimal SalesPerc { get; set; }
        public decimal PurchasePerc { get; set; }
        public int TaxTypeId { get; set; }

        public virtual MaTax Category { get; set; } = null!;
    }
}
