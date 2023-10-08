using Dfinance.Core.Domain.PageRolesConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain.Roles
{
    public partial class MaRoleRight
    {
        public int Id { get; set; }

        // connected to MaRoleId
        public int RoleId { get; set; }

        //page name id 
        public int PageMenuId { get; set; }

        public bool IsView { get; set; }

        public bool IsCreate { get; set; }

        public bool IsEdit { get; set; }

        public bool IsCancel { get; set; }

        public bool IsDelete { get; set; }

        public bool IsApprove { get; set; }

        public bool IsEditApproved { get; set; }

        public bool IsHigherApprove { get; set; }

        public bool? IsPrint { get; set; }

        public bool? IsEmail { get; set; }

        //page details -> all pages saved in MaPageamenu
        public virtual MaPageMenu PageMenu { get; set; } = null!;

    }
}
