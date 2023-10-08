using Dfinance.Core.Domain.PageRolesConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain.Roles
{
    public partial class MaUserRight
    {
        public int Id { get; set; }

        //this id is connecto employee-details table
        public int UserDetailsId { get; set; }

        //we are setting page perwmission 
        public int PageMenuId { get; set; }

        public bool IsView { get; set; }

        public bool IsCreate { get; set; }

        public bool IsEdit { get; set; }

        public bool IsCancel { get; set; }

        public bool IsDelete { get; set; }

        public bool IsApprove { get; set; }

        public bool IsEditApproved { get; set; }

        public bool IsHigherApprove { get; set; }

        public bool IsPrint { get; set; }

        public bool? IsEmail { get; set; }

        public bool? FrequentlyUsed { get; set; }

        public virtual MaPageMenu PageMenu { get; set; } = null!;

        public virtual MaEmployeeDetail UserDetails { get; set; } = null!;
    
    }
}
