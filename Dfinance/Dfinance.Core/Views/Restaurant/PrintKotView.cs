using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views
{
    public class PrintKotView
    {
        public int ID { get; set; }
        public int TransactionID { get; set; }
        public int? ItemID { get; set; }
        public int? SerialNo { get; set; }
        public string? ItemName { get; set; }
        public string? Barcode { get; set; }
        public int? RefTransID1 { get; set; }
        public string? Unit { get; set; }
        public int? PriceCategoryId { get; set; }
        public string? PriceCategory { get; set; }
        public decimal? Qty { get; set; }
        public int? Pcs { get; set; }
        public decimal? Rate { get; set; }
        public decimal? ProformaAmount { get; set; }
        public decimal? AdvanceRate { get; set; }
        public decimal? OtherRate { get; set; }
        public int? MasterMiscID1 { get; set; }
        public int? RowType { get; set; }
        public string? RefCode { get; set; }
        public string? Description { get; set; }
        public string? Remarks { get; set; }
        public bool? IsBit { get; set; }
        public string? ItemCode { get; set; }
        public string? StockType { get; set; }
        public string? CategoryType { get; set; }
        public string? Commodity { get; set; }
        public int? CommodityId { get; set; }
        public string? Category {  get; set; }
        public decimal? LengthCm { get; set; }
         public decimal? LengthFt { get; set; }
        public decimal? LengthIn { get; set; }
        public decimal? ThicknessFt { get; set; }
        public decimal? ThicknessIn { get; set; }
        public decimal? ThicknessCm { get; set; }
        public decimal? GirthCm { get; set; }
        public decimal? GirthFt { get; set; }
        public decimal? GirthIn { get; set; }
        public decimal? POLotQty { get; set; }
        public int? Selection {  get; set; }
        public string? Quality { get; set; }
        public string? LotStatus { get; set; }
        public int? Status { get; set; }
        public int? InvAvgCostId { get; set; }
        public decimal? Additional { get; set; }
        public decimal? Discount { get; set; }
        public bool? IsReturn { get; set; }
        public decimal? Factor { get; set; }
        public int? QtyPrecision { get; set; }
        public int? BasicQtyPrecision { get; set; }  
        public decimal? Margin {  get; set; }
        public decimal? RateDisc { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? Amount { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? DiscountPerc {  get; set; }
        public decimal? TaxPerc {  get; set; }
        public string? TranType { get; set; }
        public int? SizeMasterID { get; set; }
        public decimal? TaxValue { get; set; }
        public int? TaxTypeID { get; set; }
        public decimal? CostPerc { get; set; }
        public int? InLocID { get; set; }
        public int? OutLocID { get; set; }
        public string? BatchNo { get; set; }
        public string? SizeMasterName { get; set; }
        public int? TaxAccountID { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? FocQty { get; set; }
        public int? RefID { get; set; }
        public int? RefTransItemID { get; set; }
        public decimal? StockQty { get; set; }
        public decimal? BasicQty { get; set; }
        public decimal? TempQty { get; set; }
        public decimal? TempRate { get; set; }
        public decimal? ReplaceQty { get; set; }
        public decimal? PrintedMRP { get; set; }
        public decimal? PrintedRate { get; set; }
        public decimal? PTSRate { get; set; }
        public decimal? PTRRate { get; set; }
        public string? TempBatchNo { get; set; }
        public int? StockItemId { get; set; }
        public string? StockItem {  get; set; }
        public string? ArabicName { get; set; }
        public string? Location { get; set; }
        public string? UnitArabic {  get; set; }
        public int? CostAccountId { get; set; }
        public decimal? CGSTPerc { get; set; }
        public decimal? SGSTPerc { get; set; }
        public decimal? SGSTValue { get; set; }
        public decimal? CGSTValue { get; set; }
        public string? HSN { get; set; }
        public int? BrandID { get; set; }
        public string? BrandName { get; set; }
        public string? ComplaintNature { get; set; }
        public string? AccLocation { get; set; }

    }
}
