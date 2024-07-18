using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Finance
{
   

    public class ChequeregView
    {
        public int? VID { get; set; }
        public int? ID { get; set; }
        public int? VEID { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public DateTime? VDate { get; set; }
        public DateTime? PostedDate { get; set; }
        public int? PostedVID { get; set; }
        public string? PostedVNo { get; set; }
        public string? BankName { get; set; }
        public string? Party { get; set; }
        public int? PartyID { get; set; }
        public int? AccountID { get; set; }
        public string? Status { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public string? Description { get; set; }
        public string? SubStatus { get; set; }
    }
    
}
