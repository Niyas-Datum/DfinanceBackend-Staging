using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public partial class MaDepartment
    {
        public int Id { get; set; }

        //FK
        public int DepartmentTypeId { get; set; }

        //FK
        public int CompanyId { get; set; }

        //FK
        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public byte ActiveFlag { get; set; }

        public virtual MaCompany Branch { get; set; }
        public virtual MaEmployee MaEmployee { get; set; }
        public virtual ReDepartmentType DepartmentType { get; set; }


        public MaDepartment()
        {
            Branch = null!;
            MaEmployee = null!;
            DepartmentType = null!;

        }
    }
}
