using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain.Roles
{
    public partial class MaRoles
    {

        public int Id { get; set; }

        public string Role { get; set; } = null!;

        public int Active { get; set; }

        //created user branch Id
        public int? CreatedBranchId { get; set; }

        //create user id  -  maemployees
        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public virtual MaCompany? CreatedBranch { get; set; }

        //public virtual MaEmployee? CreatedByNavigation { get; set; }

        // many employees have sames roles
        //public virtual ICollection<MaEmployeeDetail> MaEmployeeDetails { get; set; } = new List<MaEmployeeDetail>();
    }
}
