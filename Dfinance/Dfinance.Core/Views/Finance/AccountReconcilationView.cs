using Dfinance.Core.Views.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Finance
{
    public class AccountReconcilationView
    {
        public List<AccountReconcilView> accountReconcilViews { get; set; }
        [NotMapped]
        public AccountRecoView accountRecoView { get; set; }
    }
    public class AccountReconcilView
    {
        public int? ID { get; set; }
        public string? VNo { get; set; }
        public DateTime? VDate { get; set; }
        public string? Particulars { get; set; }
        public string? TransactionType { get; set; }
        public string? VType { get; set; }
        public string? InstrumentNo { get; set; }
        public DateTime? InstrumentDate { get; set; }
        public DateTime? BankDate { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Balance { get; set; }

    }
    public class AccountRecoView
    { 
        public decimal? BookBDebit { get; set; }
        public decimal? BookBCredit { get; set; }
        public decimal? BookBalance { get; set; }
        public decimal? CBDebit { get; set; }
        public decimal? CBCredit { get; set; }
        public decimal? CBalance { get; set; }
        public decimal? UCBDebit { get; set; }
        public decimal? UCBCredit { get; set; }
        public decimal? UCBalance { get; set; }
        public decimal? BookBAsOnDate { get; set; }
        public decimal? CBAsOnDate { get; set; }
        public decimal? UCBAsOnDate { get; set; }

    }
}

