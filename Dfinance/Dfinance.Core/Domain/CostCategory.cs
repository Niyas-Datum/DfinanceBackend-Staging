using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class CostCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool? AllocateRevenue { get; set; }

        public bool? AllocateNonRevenue { get; set; }

        // FK
        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        // FK
        public int? CreatedBranchId { get; set; }

        public bool? Active { get; set; }

        //Relationships
        public virtual MaCompany CreatedBranch { get; set; }
        
        public virtual MaEmployee CreatedByEmployee { get; set; }
    }
}
