using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public partial class InvAvgCost
    {
        public long Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int ItemId { get; set; }
        public int? BranchId { get; set; }
        public decimal LastRate { get; set; }
        public decimal AvgCost { get; set; }
        public string? BatchNo { get; set; }

       // public virtual MaCompany? Branch { get; set; }
       // public virtual ItemMaster Item { get; set; } = null!;
    }
}
