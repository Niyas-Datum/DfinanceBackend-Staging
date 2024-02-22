using Dfinance.DataModels.Dto.Common;

namespace Dfinance.DataModels.Dto.CustSupp
{
    public class CustomerDetailsDto
    {
        public int? CommoditySought { get; set; }
        public DropdownDto? SalesType { get; set; }  // {Cash/Credit}
        public int? QuantityPlanned { get; set; }
        public decimal? BasicUnit { get; set; }
        public DropdownDto? CreditCollectionType { get; set; }
        public string? DL1 { get; set; }
        public string? DL2 { get; set; }
        public DropDownDtoName? PriceCategory { get; set; }
        public DropdownDto? PlaceOfSupply { get; set; }

        // Business Details
        public DropdownDto? BusinessType { get; set; } // BusPrimaryType {Primary(P)/Secondary(S)}
        public DropdownDto? AvailedAnyLoanLimits { get; set; } // isLoanAvailable {Yes(Y)/No(N)}
        public DropdownDto? BusinessNature { get; set; } // BusRetailType {Retail(R)/Wholesale(W)}
        public string? OtherMerchantsOfCustomer { get; set; } // MainMerchants input value {Owned(O),Rental(R)}
        public DropdownDto? BusinessAddress { get; set; } // AddressOwned
        public decimal? ValueOfProperty { get; set; } // Value of Property
        public int? YearsOfBusiness { get; set; } // BusYear 
        public decimal? YearlyTurnover { get; set; } // BusYearTurnover
        public string? MarketReputation { get; set; } // MarketReputation

        // For Branch
        public DropDownDtoName? CategoryRecommended { get; set; } // BandByImportId
        public int? LimitRecommended { get; set; } // SalesLimitByImport

        // For HQ
        public DropDownDtoName? CategoryFixed { get; set; } // BandByHoId
        public int? LimitFixedForCustomer { get; set; } // SalesLimitByHo
        public int? CreditPeriodPermitted { get; set; } // CreditPeriodByHo
        public decimal? OverdueAmountLimit { get; set; } // OverdueLimitPerc
        public int? OverduePeriodLimit { get; set; } // OverduePeriodLimit
        public int? ChequeBounceCountLimit { get; set; } // ChequeBounceLimit
        public decimal? SalesPriceLowVarLimit { get; set; } // SalesPriceLowVarPerc
        public decimal? SalesPriceUpVarLimit { get; set; } // SalesPriceUpVarPerc
    }
}
