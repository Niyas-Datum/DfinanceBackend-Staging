using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public partial class ReDepartmentType
    {
        public int Id { get; set; }
        public string Department { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte ActiveFlag { get; set; }
        public int CreatedBranchId { get; set; }
        public virtual MaCompany CreatedBranch { get; set; }
        public virtual ICollection<MaDepartment> MaDepartments { get; set; }

        public ReDepartmentType()
        {
            Department = string.Empty;  
            MaDepartments = new List<MaDepartment>();
            CreatedBranch = null!;
        }

    }
}
