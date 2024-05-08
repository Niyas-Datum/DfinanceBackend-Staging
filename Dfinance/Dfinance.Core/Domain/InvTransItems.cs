using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class InvTransItems
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int? ItemId { get; set; }
        public int? RefTransId1 { get; set; }
        public string? Unit { get; set; }
        public decimal? Qty { get; set; }
        public decimal? BasicQty { get; set; }
        public int? Pcs { get; set; }
        public decimal? Rate { get; set; }
        public decimal? AdvanceRate { get; set; }
        public decimal? OtherRate { get; set; }
        public int? MasterMiscId1 { get; set; }
        public int? RowType { get; set; }
        public string? Description { get; set; }
        public string? Remarks { get; set; }
        public bool? IsBit { get; set; }
        public int? InvAvgCostId { get; set; }
        public bool? IsReturn { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Additional { get; set; }
        public decimal? Factor { get; set; }
        public int? CommodityId { get; set; }
        public int? AccountId { get; set; }
        public int? TransactionEntryId { get; set; }
        public decimal? LengthFt { get; set; }
        public decimal? LengthIn { get; set; }
        public decimal? LengthCm { get; set; }
        public decimal? GirthFt { get; set; }
        public decimal? GirthIn { get; set; }
        public decimal? GirthCm { get; set; }
        public decimal? ThicknessFt { get; set; }
        public decimal? ThicknessIn { get; set; }
        public decimal? ThicknessCm { get; set; }
        public decimal? ShortageQty { get; set; }
        public int? AvgCostId { get; set; }
        public int? RefTransItemId { get; set; }
        public int? Status { get; set; }
        public bool? Cancel { get; set; }
        public int? MeasuredById { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsSameForPcs { get; set; }
        public decimal? StockQty { get; set; }
        public int? RefId { get; set; }
        public int? InLocId { get; set; }
        public int? OutLocId { get; set; }
        public string? BatchNo { get; set; }
        public decimal? Margin { get; set; }
        public decimal? DiscountPerc { get; set; }
        public decimal? TaxPerc { get; set; }
        public decimal? TaxValue { get; set; }
        public int? TaxTypeId { get; set; }
        public int? TaxAccountId { get; set; }
        public int? SizeMasterId { get; set; }
        public string? TranType { get; set; }
        public decimal? CostPerc { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? FocQty { get; set; }
        public int? GroupItemId { get; set; }
        public int? PriceCategoryId { get; set; }
        public decimal? RateDiscPerc { get; set; }
        public decimal? RateDisc { get; set; }
        public int? SerialNo { get; set; }
        public decimal? TempQty { get; set; }
        public decimal? ReplaceQty { get; set; }
        public decimal? PrintedMrp { get; set; }
        public decimal? PrintedRate { get; set; }
        public decimal? PtsRate { get; set; }
        public decimal? PtrRate { get; set; }
        public decimal? TempRate { get; set; }
        public decimal? ExchangeRate { get; set; }
        public int? StockItemId { get; set; }
        public bool? Visible { get; set; }
        public int? CostAccountId { get; set; }
        public decimal? CgstPerc { get; set; }
        public decimal? SgstPerc { get; set; }
        public decimal? SgstValue { get; set; }
        public decimal? CgstValue { get; set; }
        public string? Hsn { get; set; }
        public int? BrandId { get; set; }
        public string? ComplaintNature { get; set; }
        public string? AccLocation { get; set; }
        public string? RepairsRequired { get; set; }
        public decimal? CessPerc { get; set; }
        public decimal? CessValue { get; set; }
        public int? CessAccountId { get; set; }
        public decimal? AvgCost { get; set; }
        public decimal? Profit { get; set; }
        public int? ParentId { get; set; }

        //relationships
        public virtual FiMaAccount FiMaAccount {  get; set; }// with FimaAccounts
        public virtual Categories Commodity {  get; set; }//with Category
       
        public virtual ItemMaster Item { get; set; }//with ItemMaster
       
        public virtual ItemMaster? GroupItem { get; set; }
       
        public virtual ItemMaster? StockItem { get; set; }
        public virtual InvTransItems? Ref { get; set; }//self reference
        public virtual ICollection<InvTransItems> InverseRef { get; set; }
        public virtual FiTransaction Transaction {  get; set; }
        public ICollection<InvUniqueItems> InvUniqueItems { get; set; }
      //  public virtual ICollection<InvBatchWiseItem> InvBatchWiseItems { get; set; }
    }
}
