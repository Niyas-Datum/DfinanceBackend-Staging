using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public partial class Currency
    {
        public int CurrencyId { get; set; }

        public string Currency1 { get; set; } = null!;

        public string Abbreviation { get; set; } = null!;

        public bool DefaultCurrency { get; set; }

        public double CurrencyRate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public byte ActiveFlag { get; set; }

        public int CreatedBranchId { get; set; }

        public byte? Precision { get; set; }

        public string? Culture { get; set; }

        public string? FormatString { get; set; }

        public string? Coin { get; set; }

        public virtual MaCompany CreatedBranch { get; set; } = null!;

        public virtual MaEmployee CreatedByNavigation { get; set; } = null!;

       // public virtual ICollection<FiTransactionEntry> FiTransactionEntries { get; set; } = new List<FiTransactionEntry>();

       // public virtual ICollection<FiTransaction> FiTransactions { get; set; } = new List<FiTransaction>();
    }
}

