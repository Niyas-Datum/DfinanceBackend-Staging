using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.General
{
    public class RecallVoucherView
    {
        public int? Sel {  get; set; }
        public int? ID { get; set; }
        public string? VType { get; set; }
        public string? VNo { get; set;}
        public DateTime? VDate { get; set; }
        public string? AccountName { get; set; }
        public string? CommonNarration { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedOn { get; set;}
        public DateTime? ModifiedOn { get;set;}
    }
}
