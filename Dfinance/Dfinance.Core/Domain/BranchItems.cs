using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class BranchItems
    {
        public long Id { get; set; }
        public int BranchId { get; set; }
        public int ItemId { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal SellingPrice { get; set; }
        public bool Active { get; set; }

        //relationships
        public virtual MaCompany Branch { get; set; }
        public virtual ItemMaster Item {  get; set; }
    }
}
