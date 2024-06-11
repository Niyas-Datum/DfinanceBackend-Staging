using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class TransItemExpense
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int ItemId { get; set; }
        public int AccountId { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public int? ChargeTypeId { get; set; }
        public int? Veid { get; set; }
        public DateTime? DueDate { get; set; }
        public int? TransItemId { get; set; }

        public virtual FiMaAccount Account { get; set; } = null!;
        public virtual MaChargeType? ChargeType { get; set; }
        public virtual ItemMaster Item { get; set; } = null!;
        public virtual FiTransaction Transaction { get; set; } = null!;
        public virtual FiTransactionEntry? Ve { get; set; }
    }
}
