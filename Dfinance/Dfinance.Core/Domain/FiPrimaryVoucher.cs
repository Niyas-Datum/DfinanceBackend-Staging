using Dfinance.Core.Views.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class FiPrimaryVoucher
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? DayBookDrCr { get; set; }

        public virtual ICollection<Voucher> FiMaVouchers { get; set; }
    }
}
