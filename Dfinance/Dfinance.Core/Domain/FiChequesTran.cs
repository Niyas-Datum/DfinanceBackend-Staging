using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public partial class FiChequesTran
    {
        public long Id { get; set; }
        public long? Vid { get; set; }
        public int? Veid { get; set; }
        public int ChequeId { get; set; }
        public string? TranType { get; set; }

        public virtual FiTransactionEntry? Ve { get; set; }

    }
}
