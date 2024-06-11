namespace Dfinance.Core.Views.Inventory.Purchase
{
    public class ReferenceView
    {
        public bool? Sel { get; set; }
        public bool? AddItem { get; set; }
        public int? VoucherID { get; set; }
        public string? VNo { get; set; }
        public DateTime? VDate { get; set; }
        public string? ReferenceNo { get; set; }
        public int? AccountID { get; set; }
        public string? AccountName { get; set; }
        public decimal? Amount { get; set; }
        public string? PartyInvNo { get; set; }
        public DateTime? PartyInvDate { get; set; }
        public int? ID { get; set; }
        public string? VoucherType { get; set; }
        public string? MobileNo { get; set; }
    }

    public class RefItemsView
    {
        public int ID { get; set; }
        public int TransactionID { get; set; }
        public int? ItemID { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? Unit { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountPerc { get; set; }
        public string? TranType { get; set; }
        public int? SizeMasterID { get; set; }
        public decimal? Factor { get; set; }
        public decimal? LengthFt { get; set; }
        public decimal? LengthCm { get; set; }
        public decimal? LengthIn { get; set; }
        public decimal? GirthCm { get; set; }
        public decimal? GirthIn { get; set; }
        public decimal? GirthFt { get; set; }
        public decimal? ThicknessCm { get; set; }
        public decimal? ThicknessIn { get; set; }
        public decimal? ThicknessFt { get; set; }
        public decimal? Qty { get; set; }
        public decimal? FOCQty { get; set; }
        public decimal? BasicQty { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; }
        public int? PartyID { get; set; }
        public string? PartyName { get; set; }
        public int? StaffID { get; set; }
        public string? StaffName { get; set; }
        public string? StaffIncentive { get; set; }
        public int? AreaID { get; set; }
        public int? LocationID { get; set; }
        public int? ToLocationID { get; set; }
        public int? BranchID { get; set; }
        public int? Period { get; set; }
        public int? TypeID { get; set; }
        public int? PaymentTypeID { get; set; }
        public decimal? TaxPerc { get; set; }
        public decimal? TaxValue { get; set; }
        public int? TaxTypeID { get; set; }
        public int? TaxAccountID { get; set; }
        public int? FromBranchID { get; set; }
        public int? ToBranchID { get; set; }
        public int? FromWarehouseID { get; set; }
        public int? ToWarehouseID { get; set; }
        public decimal? Margin { get; set; }
        public decimal? OtherRate { get; set; }
        public string? BatchNo { get; set; }
        public string? Description { get; set; }
        public string? Remarks { get; set; }
        public bool? IsBit { get; set; }
        public string? HSN { get; set; }
        public string? ComplaintNature { get; set; }
        public string? AccLocation { get; set; }
        public int? BrandID { get; set; }
        public string? BrandName { get; set; }
        public int? CostAccountID { get; set; }
        public string? ArabicName { get; set; }
        public int? MeasuredByID { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? CGSTPerc { get; set; }
        public decimal? CGSTValue { get; set; }
        public decimal? SGSTPerc { get; set; }
        public decimal? SGSTValue { get; set; }
        public int? CessAccountID { get; set; }
        public decimal? CessPerc { get; set; }
        public decimal? CessValue { get; set; }
        public int? Status { get; set; }
        public decimal? AvgCost { get; set; }
        public decimal? Profit { get; set; }
    }

    public class ImportItemListView
    {
       // public bool? Select { get; set; }
        public int? ItemID { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? Unit { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Rate { get; set; }
        public decimal? PrintedMRP { get; set; }
        public decimal? Amount { get; set; }
    }
}
