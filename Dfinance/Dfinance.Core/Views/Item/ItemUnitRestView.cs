using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Item
{
    public class ItemUnitRestView
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Unit { get; set; } = null!;
        public string BasicUnit { get; set; } = null!;
        public decimal Factor { get; set; }
        public bool? Active { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? PurchaseRate { get; set; }
        public string? BarCode { get; set; }
        public decimal? WholeSalePrice { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? DiscountPrice { get; set; }//WholeSalePrice2
        public decimal? OtherPrice { get; set; }//RetailPrice2
        public decimal? LowestRate { get; set; }
        public decimal? MRP { get; set; }
    }
}
