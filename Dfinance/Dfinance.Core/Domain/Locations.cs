using Dfinance.Core.Views.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Dfinance.Core.Domain
{
    public class Locations
    {
        public int Id { get; set; }
        public int LocationTypeId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Remarks { get; set; } = null!;
        public decimal? ClearingPerCft { get; set; }
        public decimal? GroundRentPerCft { get; set; }
        public decimal? LottingPerPiece { get; set; }
        public decimal? LorryHirePerCft { get; set; }
        public bool Active { get; set; }
        public int? DevCode { get; set; }
        public int CreatedBranchId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual MaCompany CreatedBranch { get; set; } = null!;
        public virtual MaEmployee CreatedByNavigation { get; set; } = null!;
        public virtual LocationTypes LocationTypes { get; set; }
        //public virtual ICollection<FiTransactionAdditional> FiTransactionAdditionalFromLocations { get; set; } = new List<FiTransactionAdditional>();
        //public virtual ICollection<FiTransactionAdditional> FiTransactionAdditionalInLocs { get; set; } = new List<FiTransactionAdditional>();
        //public virtual ICollection<FiTransactionAdditional> FiTransactionAdditionalOutLocs { get; set; } = new List<FiTransactionAdditional>();
        //public virtual ICollection<FiTransactionAdditional> FiTransactionAdditionalToLocations { get; set; } = new List<FiTransactionAdditional>();
        //public virtual ICollection<InvTransItemDetail> InvTransItemDetails { get; set; } = new List<InvTransItemDetail>();
         public virtual ICollection<LocationBranchList> LocationBranchLists { get; set; } = new List<LocationBranchList>();
        //public virtual ICollection<LocationRestriction> LocationRestrictions { get; set; } = new List<LocationRestriction>();
       //public virtual ICollection<LocationTypes> LocationTypes { get; set; } = null!;
        //public virtual ICollection<MaRoute> MaRouteFromLocations { get; set; } = new List<MaRoute>();
        //public virtual ICollection<MaRoute> MaRouteToLocations { get; set; } = new List<MaRoute>();
        //public virtual ICollection<TransLoadSchedule> TransLoadSchedules { get; set; } = new List<TransLoadSchedule>();
    }
}
