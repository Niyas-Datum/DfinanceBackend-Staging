using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class DocType
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public byte ActiveFlag { get; set; }
        public bool IsLcdoc { get; set; }
        public bool IsPodoc { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsLcmandatoryDoc { get; set; }
        public byte? DevCode { get; set; }
        public string Directory { get; set; } = null!;
        public string? Pirdesc { get; set; }
        public int CreatedBranchId { get; set; }
        public int? DocRowType { get; set; }
        public virtual MaCompany CreatedBranch { get; set; } = null!;
       public virtual MaEmployee CreatedByNavigation { get; set; } = null!;
    }
}
