using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class InvUniqueItems
    {
        public long Id { get; set; }
        public int TransactionId { get; set; }
        public int? TransItemId { get; set; }
        public int? ItemId { get; set; }
        public string? UniqueNo { get; set; }

        public virtual ItemMaster? Item { get; set; }
        public virtual InvTransItems? TransItem { get; set; }
        public virtual FiTransaction Transaction { get; set; } = null!;
    }
}
