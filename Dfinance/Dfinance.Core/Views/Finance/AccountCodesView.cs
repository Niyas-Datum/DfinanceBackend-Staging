using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Finance
{
    public class AccountCodesView
    {
        public string? AccountCode { get; set; }
        public string? AccountName { get; set;}
        public string? Details { get; set;}
        public int? ID { get; set; }
        //public decimal? AccBalance { get; set; }
        public bool? IsBillWise { get; set; }
        public bool? IsCostCentre { get; set; }

    }
}
