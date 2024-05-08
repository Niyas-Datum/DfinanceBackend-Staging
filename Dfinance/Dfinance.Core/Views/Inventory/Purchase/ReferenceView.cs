using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Inventory.Purchase
{
    public class ReferenceView
    {
        public bool? Sel { get; set; }
        public bool? AddItem { get; set; }
        public int? VoucherID { get; set; }
        public string? VNo { get; set; }
        public DateTime? VDate { get; set; }
        public string? ReferenceNo { get; set; }
        public int? AccountID { get; set; }
        public string? AccountName { get; set; }
        public decimal? Amount { get; set; }
        public string? PartyInvNo { get; set; }
        public DateTime? PartyInvDate { get; set; }
        public int? ID { get; set; }
        public string? VoucherType { get; set; }
        public string? MobileNo { get; set; }
    }


}
