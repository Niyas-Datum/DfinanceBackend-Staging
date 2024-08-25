using System;
using System.Collections.Generic;

namespace Dfinance.Core.Domain
{
    public partial class MaTax
    {
        public MaTax()
        {
            MaTaxDetails = new HashSet<MaTaxDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Note { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int CreatedBranchId { get; set; }

        public virtual MaCompany CreatedBranch { get; set; } = null!;
        public virtual MaEmployee CreatedByNavigation { get; set; } = null!;
        public virtual ICollection<MaTaxDetail> MaTaxDetails { get; set; }
    }
}
