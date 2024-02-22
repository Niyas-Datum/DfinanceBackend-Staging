using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class MaCustomerCategories
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public decimal SalesLimit { get; set; }
        public decimal SalesPriceLowVarPerc { get; set; }
        public decimal SalesPriceUpVarPerc { get; set; }
        public byte SalesAllowed { get; set; }
        public decimal OverdueLimitPerc { get; set; }
        public int OverduePeriodLimit { get; set; }
        public int ChequeBounceLimit { get; set; }
        public int CreditPeriod { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn {get;set;}
        public bool ActiveFlag { get; set; }
        public int CreatedBranchID { get; set; }

        public virtual ICollection<MaCustomerDetails> MaCustomerDetailBandByHos { get; set; } = new List<MaCustomerDetails>();

        public virtual ICollection<MaCustomerDetails> MaCustomerDetailBandByImports { get; set; } = new List<MaCustomerDetails>();
    }

}

