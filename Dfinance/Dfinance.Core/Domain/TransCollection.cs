using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class TransCollection
    {
        public int Id { get; set; }
        public int? TransactionId { get; set; }
        public int? PayTypeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public int? InstrumentTypeId { get; set; }
        public string? InstrumentNo { get; set; }
        public DateTime? InstrumentDate { get; set; }
        public string? InstrumentBank { get; set; }
        public string? Description { get; set; }
        public int? Veid { get; set; }

        public virtual FiTransaction? Transaction { get; set; }
        public virtual FiTransactionEntry? Ve { get; set; }
        public virtual ICollection<TransCollnAllocation> TransCollnAllocations { get; set; }
    }
}
