using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Finance
{
    public class CheqDetailView
    {
        public bool? Selection { get; set; }
        public int? ID { get; set; }
        public int? VEID { get; set; }
        public int? VID { get; set; }
        public int? Posted { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public DateTime? VDate { get; set; }
        public string? BankName { get; set; }
        public int? PartyID { get; set; }
        public int? EntryID { get; set; }
        public string? Party { get; set; } //party
        public string? Description { get; set; }
        public decimal? Debit { get; set; } //debit
        public decimal? Credit { get; set; } //credit
        public int? AccountID { get; set; }
        public string? AccountCode { get; set; }
        public string? AccountName { get; set; }
        public string? Status { get; set; }
    }
}
