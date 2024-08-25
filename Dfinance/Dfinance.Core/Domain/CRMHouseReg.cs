using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class CRMHouseReg
    {
        public long Id { get; set; }
        public string? HouseNo { get; set; }
        public string? HouseName { get; set; }
        public string? FamilyName { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Locality { get; set; }
        public string? WardNo { get; set; }
        public string? Street { get; set; }
        public string? Panchayath { get; set; }
        public string? Taluk { get; set; }
        public string? PostBoxNo { get; set; }
        public string? District { get; set; }
        public string? PhoneNo { get; set; }
        public DateTime? YearOccupied { get; set; }
        public string? LandMark { get; set; }
        public string? TypeofRoof { get; set; }
        public string? NoofFloor { get; set; }
        public int? TotalArea { get; set; }
        public bool? Lpgconnection { get; set; }
        public string? ConsumerNo { get; set; }
        public bool? Eletricity { get; set; }
        public string? MeterNo { get; set; }
        public bool? Rented { get; set; }
        public bool? WaterConnection { get; set; }
        public string? WaterSource { get; set; }
        public bool? SpecialCare { get; set; }
        public string? Reason { get; set; }
        public bool? Active { get; set; }
        public string? Remarks { get; set; }
    }
}

 