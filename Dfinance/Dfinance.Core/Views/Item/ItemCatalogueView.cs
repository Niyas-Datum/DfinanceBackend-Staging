using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Item
{
    public class ItemCatalogueView
    {
        public long? SlNo { get; set; }
        public int ID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal? Stock { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? SellingPrice { get; set; }
        public string? Commodity { get; set; }
        public string? Category { get; set; }
        public string? BarCode { get; set; }
        public string? Origin { get; set; }
        public string? Image { get; set; }
        public decimal? MRP { get; set; }
        public bool StockItem { get; set; }
        public string? OEMNo { get; set; }
        public string? Manufacturer { get; set; }
        public string? ModelNo { get; set; }
        public string? Quality { get; set; }
        public decimal? CashPrice { get; set; }
        public decimal? CreditPrice { get; set; }
        public string? PartNo { get; set; }
        public string? Location { get; set; }
        public decimal? Weight { get; set; }
        public decimal? AvgCost { get; set; }
        public decimal? LastPurchaseRate { get; set; }
        public decimal? Margin { get; set; }
    }

    public class ItemCatalogueViews
    {
        public long? SlNo { get; set; }
        public int ID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal? SellingPrice { get; set; }
        public string? Unit { get; set; }
        public decimal? Stock { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? PurchaseValue { get; set; }
        public decimal? SalesValue { get; set; }
        public string? Category { get; set; }
        public string? Commodity { get; set; }
    }
}
