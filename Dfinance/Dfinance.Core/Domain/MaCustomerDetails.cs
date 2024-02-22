using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public partial class MaCustomerDetails
    {
        public int Id { get; set; }

        public int PartyId { get; set; }

        public int? PlannedPcs { get; set; }

        public decimal? PlannedCft { get; set; }

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
        public int? BandByImportId { get; set; }

        public decimal? SalesLimitByImport { get; set; }

        public int? BandByHoid { get; set; }

        public decimal? SalesLimitByHo { get; set; }

        public int? CreditPeriodByHo { get; set; }
        public decimal? OverdueLimitPerc { get; set; }

        public int? OverduePeriodLimit { get; set; }

        public int? ChequeBounceLimit { get; set; }

        public decimal? SalesPriceLowVarPerc { get; set; }

        public decimal? SalesPriceUpVarPerc { get; set; }

        public virtual MaCustomerCategories? BandByHo { get; set; }

        public virtual MaCustomerCategories? BandByImport { get; set; }
        public virtual Parties Party { get; set; } = null!;
        public virtual DeliveryDetails? DeliveryDetails { get; set; }
    }
}

