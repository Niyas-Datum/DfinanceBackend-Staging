using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.General
{
    public class FillCustomeritem
    {
        public int ID { get; set; }
        public int PartyID { get; set; }
        public int CommodityID { get; set; }
    }

    public class FillPartyView
    {
        public int? ID { get; set; }
        public int? PartyID { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public string? Address { get; set; }
        public string? MobileNo { get; set; }
        public string? VATNo { get; set; }
        public decimal? AccBalance { get; set; }

    }
    public class FillSideBar
    {
        public int? ID { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? AccountID { get; set; }
        public string? ImagePath { get; set; }
        public string? Nature { get; set; }
    }
    public class  FillPartyById
    {
        public int ID { get; set; }
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
        public int? CompanyID { get; set; }
        public int? BranchID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsMultiNature { get; set; }
        public string? ImagePath { get; set; }
        public string? Code { get; set; }
        public int? AccountID { get; set; }
        public string? DL1 { get; set; }
        public string? DL2 { get; set; }
        public long? AreaID { get; set; }
        public string? BulidingNo { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public string? CountryCode { get; set; }
        public string? Area { get; set; }
        public decimal? CreditLimit { get; set; }
        public int? PriceCategoryID { get; set; }
        public string? PlaceOfSupply { get; set; }
        public string? ArabicName { get; set; }
        public long? SalesManID { get; set; }
        public string? SalesMan {  get; set; }
        public int? PartyCategoryID { get; set; }
        public string? DistrictArabic {  get; set; }
        public string? CityArabic { get; set; }
        public string? ProvinceArabic { get; set; }
    }
    public class FillCustdetails
    {
        public int ID { get; set; }
        public int PartyID { get; set; }
        public int? PlannedPcs {  get; set; }
        public decimal? PlannedCFT { get; set; }
        public string? CashCreditType { get; set; }
        public int? CreditPeriod { get; set; }
        public string? CreditCollnThru { get; set; }
        public string? BusPrimaryType { get; set; }
        public string? BusRetailType { get; set; }
        public int? BusYears { get; set; }
        public decimal? BusYearTurnover { get; set; }
        public string? IsLoanAvailed { get; set; }
        public string? MainMerchants { get; set; }
        public string? AddressOwned { get; set; }
        public decimal? ValueofProperty { get; set; }
        public string? MarketReputation { get; set; }
        public int? BandByImportID { get; set; }
        public decimal? SalesLimitByImport { get; set; }
        public int? BandByHOID{ get; set; }
        public decimal? SalesLimitByHO { get; set; }
        public int? CreditPeriodByHO { get; set; }
        public decimal? OverdueLimitPerc { get; set; }
        public int? OverduePeriodLimit { get; set; }
        public int? ChequeBounceLimit { get; set; }
        public decimal? SalesPriceLowVarPerc { get; set; }
        public decimal? SalesPriceUpVarPerc { get; set; }

    }
    public class FillDeldetails
    {
        public int ID { get; set; }

        public int? PartyID { get; set; }

        public string? LocationName { get; set; }

        public string? ProjectName { get; set; }

        public string? ContactPerson { get; set; }

        public string? ContactNo { get; set; }
        public string? Address { get; set; }
    }

    public class CrdtCollView
    {
        public int ID { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }
    }
}
