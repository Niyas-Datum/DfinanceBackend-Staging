using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dfinance.Core.Domain
{
    public class LocationBranchList
    {
        public int Id { get; set; }
        public int? LocationId { get; set; }
        public int? BranchId { get; set; }
        public bool? IsDefault { get; set; }
        public bool? Active { get; set; }
        public virtual MaCompany? Branch { get; set; }
        public virtual Locations? Locations { get; set; }
    }
}
