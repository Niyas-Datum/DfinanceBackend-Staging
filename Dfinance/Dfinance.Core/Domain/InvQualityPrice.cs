using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class InvQualityPrice
    {
        public int Id { get; set; }
        public int QualityId { get; set; }
        public decimal? Rate { get; set; }

        public virtual MaMisc Quality { get; set; } = null!;
    }
}
