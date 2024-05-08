using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public partial class Parties
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? ContactPerson { get; set; }

        public string? Nature { get; set; }

        public string? AddressLineOne { get; set; }

        public string? AddressLineTwo { get; set; }

        public string? City { get; set; }

        public int? Country { get; set; }
        public string? Pobox { get; set; }

        public string? Remarks { get; set; }

        public string? TelephoneNo { get; set; }

        public string? MobileNo { get; set; }

        public string? EmailAddress { get; set; }

        public string? FaxNo { get; set; }
        public bool Active { get; set; }

        public string? PANNo { get; set; }

        public string? SalesTaxNo { get; set; }

        public string? CentralSalesTaxNo { get; set; }

        public string? Salutation { get; set; }

        public string? ContactPerson2 { get; set; }
        public string? TelephoneNo2 { get; set; }

        public int? CompanyId { get; set; }

        public int? BranchId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? Code { get; set; }
        public int? AccountId { get; set; }

        public bool? IsMultiNature { get; set; }

        public string? ImagePath { get; set; }

        public string? Dl1 { get; set; }

        public string? Dl2 { get; set; }
        public long? AreaId { get; set; }

        public decimal? CreditLimit { get; set; }

        public int? PriceCategoryId { get; set; }

        public string? ArabicName { get; set; }

        public string? PlaceOfSupply { get; set; }

        public long? SalesManId { get; set; }
        public int? PartyCategoryId { get; set; }

        public string? BulidingNo { get; set; }

        public string? District { get; set; }

        public string? Province { get; set; }

        public string? CountryCode { get; set; }

        public virtual FiMaAccount? FiMaAccount { get; set; }
        public virtual ICollection<DeliveryDetails> DeliveryDetails { get; set; } = new List<DeliveryDetails>();
      
        // public virtual ICollection<MaCustomerBank> MaCustomerBanks { get; set; } = new List<MaCustomerBank>();

        public virtual ICollection<MaCustomerDetails> MaCustomerDetails { get; set; } = new List<MaCustomerDetails>();

         public virtual ICollection<MaCustomerItems> MaCustomerItems { get; set; } = new List<MaCustomerItems>();

        public virtual MaPriceCategory? PriceCategory { get; set; }
    }


}

