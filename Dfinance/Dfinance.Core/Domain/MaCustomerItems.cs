using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public partial class MaCustomerItems
    {
        public int Id { get; set; }
        public int PartyId { get; set; }
        public int CommodityId { get; set; }

        public virtual Categories Commodity { get; set; } = null!;
        public virtual Parties Party { get; set; } = null!;
    }
}
