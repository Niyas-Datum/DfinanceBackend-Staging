
namespace Dfinance.Core.Views.Inventory
{
    public class FillItemUnitsView
    {
        public int ID {  get; set; }
        public int ItemID {  get; set; }
        public string Unit {  get; set; }
        public string BasicUnit {  get; set; }
        public decimal? Factor {  get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? MRP { get; set; }
        public decimal? WholeSalePrice { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public decimal? OtherPrice { get; set; }
        public decimal? LowestRate { get; set; }
        public string? Barcode { get; set; }
        public bool? Active { get; set; }
        public int? BranchID { get; set; }
    }
}
