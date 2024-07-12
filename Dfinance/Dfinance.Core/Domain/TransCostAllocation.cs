using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class TransCostAllocation
    {
        public int Id { get; set; }
        public int Veid { get; set; }
        public int CostCentreId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }

        public virtual CostCentre CostCentre { get; set; } = null!;
        public virtual FiTransactionEntry Ve { get; set; } = null!;
    }
}
