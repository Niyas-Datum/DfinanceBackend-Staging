using Dfinance.Core.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{

    // new user branch details
    public partial class MaEmployeeDetail
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte ActiveFlag { get; set; }
        public bool IsMainBranch { get; set; }

       
        //connected to MaRoles user Roles
        public int? MaRoleId { get; set; }
        public virtual MaRoles? MaRole { get; set; }



        //conected createdbranchId
        public int BranchId { get; set; }
        public virtual MaCompany EmployeeBranch { get; set; }



        public virtual ICollection<MaUserRight> MaUserPagePermisions { get; set; }


        //connected from MaEmployee table
        public int EmployeeId { get; set; }
        public virtual MaEmployee Employee { get; set; } = null!;


        //connected to MaDepartment 
        //emloyee department connected  to Madepartment 
        public int DepartmentId { get; set; }
        public virtual MaDepartment Department { get; set; } = null!;



        //createdBY -=> logined user
        //from logined user
        public int CreatedBy { get; set; }
        public virtual MaEmployee CreatedByNavigation { get; set; } = null!;


        //----branch supervisor
        public int? SupervisorId { get; set; }
        public virtual MaEmployee? Supervisor { get; set; }



    }
}
