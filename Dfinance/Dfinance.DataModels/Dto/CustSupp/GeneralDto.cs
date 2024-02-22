using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.CustSupp
{
    public class GeneralDto
    {
            /// <summary>
            /// Update add new field DistrictArabic 
            /// </summary>
            public int? Id { get; set; }
            public DropDownDesc Type { get; set; }//Nature
            public DropdownDto? Category { get; set; }
            public string? Code { get; set; }
            public string? Salutation { get; set; }
            public bool? Active { get; set; }
            public string? Name { get; set; }
            public string? ArabicName { get; set; }
            public string? ContactPersonName { get; set; }
            public string? TelephoneNo { get; set; }
            public string? AddressLineOne { get; set; }
            public string? AddressArabic { get; set; }//AddressLine2
            public string? MobileNo { get; set; }
            public string? VATNO { get; set; }//salesTaxNo
            public decimal? CreditPeriod { get; set; }
            public decimal? CreditLimit { get; set; }
            public long? SalesMan { get; set; }//salemanid
            public string? City { get; set; }
            public string? POBox { get; set; }
            public string? CountryCode { get; set; }
            public int? Country { get; set; }//dropdown
            public string? BulidingNo { get; set; }
            public string? District { get; set; }
            public string? DistrictArabic { get; set; }
            public string? CityArabic { get; set; }
            public string? ProvinceArabic { get; set; }
        public DropDownDtoName? Area { get; set; }//DropDown
            public string? Province { get; set; }

            //*************otherDetails*****************************
            public string? FaxNo { get; set; }
            public string? ContactPerson2 { get; set; }
            public string? EmailAddress { get; set; }
            public string? TelephoneNo2 { get; set; }
            public string? CentralSalesTaxNo { get; set; }
            public bool? ActasSupplierAlso { get; set; }//IsMultiNature
            public string? PANNo { get; set; }
            public bool? LetSystemGenNewAccForParty { get; set; }
            public DropDownDtoName? AccountGroup { get; set; }//DropDown
            public DropDownDtoName? Account { get; set; }//Dropdown
            public string? Remarks { get; set; }
            public string Image { get; set; }
            public CustomerDetailsDto CustomerDetails { get; set; }
            public List<DeliveryDetailsDto> DeliveryDetails { get; set; }
    }
}
