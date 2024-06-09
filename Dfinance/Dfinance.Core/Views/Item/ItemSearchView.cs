using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Item
{
    public class ItemSearchView
    {
        public int ID {  get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Commodity {  get; set; }
        public decimal Stock { get; set; }
        public decimal? PurchaseRate { get; set; }
        public string? OEMNo { get; set; }
        public string? PartNo { get; set; }
        public string? Manufacturer { get; set; }
        public decimal SellingPrice {  get; set; }
        public string? Location {  get; set; }
        public string? Barcode { get; set; }
        public string? ModelNo { get; set; }
        public string? Unit { get; set; }
        public string? Remarks { get; set; }
        public bool IsGroup {  get; set; }
        public bool? StockItem { get; set; }
        public bool Active {  get; set; }
        public decimal? ROL { get; set; }
        public decimal? CashPrice { get; set; }
        public decimal? CreditPrice { get; set; }
        public int? RefPageID { get; set; }
        public decimal? AvgCost {  get; set; }
        public decimal? MRP { get; set; }
        public string Tax {  get; set; }


    }
}
