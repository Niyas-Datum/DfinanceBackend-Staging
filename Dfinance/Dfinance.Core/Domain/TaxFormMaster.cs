using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class TaxFormMaster
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string CrystalPath { get; set; } = null!;

        public virtual ICollection<Voucher> FiMaVouchers { get; set; } = new List<Voucher>();
    }
}
