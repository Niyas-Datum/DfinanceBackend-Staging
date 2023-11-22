using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class MaArea
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Note { get; set; }
        public int? ParentId { get; set; }
        public bool IsGroup { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set;}
        public bool? Active { get; set;}
        public int? CreatedBranchId { get; set; }

        //Relationships

        public virtual MaCompany maCompany { get; set; }
        public virtual MaEmployee maEmployee { get; set; }

    }
}
