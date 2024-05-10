using Dfinance.DataModels.Dto.Common;

namespace Dfinance.DataModels.Dto.CustSupp
{
    public class CustomerDetailsDto
    {
        public List<DropdownDto>? CommoditySought { get; set; }
        public DropDownDtoNature? SalesType { get; set; }  // {Cash/Credit}-CashCreditType
        public int? QuantityPlanned { get; set; }
        public decimal? BasicUnit { get; set; }
        public DropDownDtoNature? CreditCollectionType { get; set; }//[CreditCollnThru]{Direct}
        public string? DL1 { get; set; }
        public string? DL2 { get; set; }
        public DropDownDtoName? PriceCategory { get; set; }
        public DropdownDto? PlaceOfSupply { get; set; }//popup-pass the value to backend

        // Business Details
        public DropDownDtoNature? BusinessType { get; set; } // BusPrimaryType {Primary(P)/Secondary(S)}
        public DropDownDtoNature? AvailedAnyLoanLimits { get; set; } // isLoanAvailable {Yes(Y)/No(N)}
        public DropDownDtoNature? BusinessNature { get; set; } // BusRetailType {Retail(R)/Wholesale(W)}
        public string? OtherMerchantsOfCustomer { get; set; } // MainMerchants input value {Owned(O),Rental(R)}
        public DropDownDtoNature? BusinessAddress { get; set; } // AddressOwned
        public decimal? ValueOfProperty { get; set; } // Value of Property
        public int? YearsOfBusiness { get; set; } // BusYear 
        public decimal? YearlyTurnover { get; set; } // BusYearTurnover
        public DropDownDtoNature? MarketReputation { get; set; } // MarketReputation

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
