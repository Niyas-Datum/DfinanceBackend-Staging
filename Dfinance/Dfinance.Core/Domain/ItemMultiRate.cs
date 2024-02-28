using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class ItemMultiRate
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int PriceCategoryId { get; set; }
        public decimal? SalesPerc { get; set; }
        public decimal? SalesDiscountPerc { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? SalePrice { get; set; }

        public virtual ItemMaster Item { get; set; } = null!;
        public virtual MaPriceCategory PriceCategory { get; set; } = null!;
    }
}
