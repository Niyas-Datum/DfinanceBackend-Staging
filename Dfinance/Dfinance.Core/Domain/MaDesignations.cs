using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public partial class MaDesignation
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int CreatedBranchId { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public byte ActiveFlag { get; set; }

        //public virtual MaCompany CreatedBranch { get; set; } = null!;

        //public virtual MaEmployee CreatedByNavigation { get; set; } = null!;

        //public virtual ICollection<MaEmployee> MaEmployees { get; set; } = new List<MaEmployee>();
    }

}
