using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class CRMMemberRegister
    {
        public long Id { get; set; }
        public int AccountId { get; set; }
        public long? HouseId { get; set; }
        public string? ReferenceNo { get; set; }
        public string? Father { get; set; }
        public string? Mother { get; set; }
        public string? Mobile { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? DateofBirth { get; set; }
        public int? Gender { get; set; }
        public int? MartialStatus { get; set; }
        public int? BloodGroup { get; set; }
        public int? EmpType { get; set; }
        public bool? Status { get; set; }
        public DateTime? DeathDate { get; set; }
        public bool? IsMain { get; set; }
        public string? ImagePath { get; set; }
        public DateTime? MouleedDate { get; set; }
        public bool? Active { get; set; }
        public bool? Abroad { get; set; }
        public string? AbroadMobileNo { get; set; }
        public int? RelationToPrimeMember { get; set; }
        public int? Age { get; set; }
        public int? MouleedDay { get; set; }
        public int? CategoryId { get; set; }
    }
}
    

