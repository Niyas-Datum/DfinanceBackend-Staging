using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class MaPriceCategory
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Perc { get; set; }

        public string? Note { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }
        public int? CreatedBranchId { get; set; }

        public bool? Active { get; set; }
        public virtual ICollection<Parties> Parties { get; set; } = new List<Parties>();
        public virtual MaCompany? CreatedBranch { get; set; }//relationship with MaCompany
        public virtual MaEmployee? CreatedEmployee { get; set; }//relationship with MaEmployee
        public ICollection<ItemMultiRate> MultiRate { get; set; }
    }
}
