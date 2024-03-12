using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dfinance.Core.Domain
{
    public class LocationTypes
    {
        public int Id { get; set; }
        public string LocationType { get; set; } = null!;
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte ActiveFlag { get; set; }
        public int? CreatedBranchId { get; set; }
        public string? SaleSite { get; set; }
        public byte? RouteType { get; set; }
        public virtual MaCompany? CreatedBranch { get; set; }
        public virtual MaEmployee? CreatedByNavigation { get; set; }
        public virtual ICollection<Locations> Locations { get; set; } = new List<Locations>();
        public virtual ICollection<LocationBranchList> LocationBranchList { get; set; }
    }
}
