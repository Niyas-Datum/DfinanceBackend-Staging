using Dfinance.DataModels.Dto.Common;

namespace Dfinance.DataModels.Dto.Inventory.Purchase
{
    public class PurchaseFillByIdDto
    {
      public FillTransactions? fillTransactions {  get; set; }
        public FillTransactionAdditional? fillAdditionals {  get; set; }
      public  List<FillTransactionEntries>? fillTransactionEntries { get; set; }
      public  FillVoucherAllocationUsingRef? fillVoucherAllocationUsingRef { get; set; }
        public  List<FillCheques>? fillCheques { get; set; }
        public  FillTransCollnAllocations? fillTransCollnAllocations { get; set; }
        public  List<FillInvTransItems>? fillInvTransItems { get; set; }
        public  FillInvTransItemDetails? fillInvTransItemDetails { get; set; }
        public  FillTransactionItemExpenses? fillTransactionItemExpenses { get; set; }
        public  FillDocuments? fillDocuments { get; set; }
        public List<FillTransactionExpenses>? fillTransactionExpenses { get; set; }
        public  FillDocumentRequests? fillDocumentRequests { get; set; }
        public  FillDocumentReferences? fillDocumentReferences { get; set; }
        public FillTransactionReferences? fillTransactionReferences { get; set; }
        public  FillTransLoadSchedules? fillTransLoadSchedules { get; set; }
        public  FillTransCollections? fillTransCollections { get; set; }
        public  FillTransEmployees? fillTransEmployees { get; set; }
        public  VMFuelLog? vMFuelLog { get; set; }
        public  FillDocumentImages? fillDocumentImages { get; set; }
        public  FillHRFinalSettlement? fillHRFinalSettlement { get; set; }
        public  FillTransCostAllocations? fillTransCostAllocations { get; set; }
        public FillTransactionAdditional? FillTransactionAdditional { get; set; }

        }
    public class FillTransactions
    {
        public int? ID { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int? VoucherID { get; set; }
        public int? SerialNo { get; set; }
        public string TransactionNo { get; set; }
        public bool? IsPostDated { get; set; }
        public int? CurrencyID { get; set; }
        public decimal? ExchangeRate { get; set; }
        public int? RefPageTypeID { get; set; }
        public int? RefPageTableID { get; set; }
        public string ReferenceNo { get; set; }
        public int? CompanyID { get; set; }
        public int? FinYearID { get; set; }
        public string InstrumentType { get; set; }
        public string InstrumentNo { get; set; }
        public DateTime? InstrumentDate { get; set; }
        public string InstrumentBank { get; set; }
        public string CommonNarration { get; set; }
        public int? AddedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? ApprovalStatus { get; set; }
        public string ApproveNote { get; set; }
        public string Action { get; set; }
        public int? StatusID { get; set; }
        public bool? IsAutoEntry { get; set; }
        public bool? Posted { get; set; }
        public bool? Active { get; set; }
        public bool? Cancelled { get; set; }
        public int? AccountID { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CreatedEmployee { get; set; }
        public string ApprovedEmployee { get; set; }
        public string EditedEmployee { get; set; }
        public string Currency { get; set; }
        public int? RefTransID { get; set; }
        public string RefCode { get; set; }
        public int? CostCentreID { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public int? CostCategoryID { get; set; }
        public bool? AllocateRevenue { get; set; }
        public bool? AllocateNonRevenue { get; set; }
        public int? PageID { get; set; }
        public string AccountNameArabic { get; set; }
    }
    public class FillTransactionAdditional
    {
        public int TransactionID { get; set; }
        public int? RefTransID1 { get; set; }
        public int? RefTransID2 { get; set; }
        public int? TypeID{ get; set; }
        public int? ModeID { get; set; }
        public int? MeasureTypeID { get; set; }
        public int? LoadMeasureTypeID { get; set; }
        public int? ConsignTermID { get; set; }
        public int? FromLocationID { get; set; }
        public int? ToLocationID { get; set; }
        public decimal? ExchangeRate1 { get; set; }
        public decimal? AdvanceExRate { get; set; }
        public decimal? CustomsExRate { get; set; }
        public int? ApprovalDays { get; set; }
        public int? WorkflowDays { get; set; }
        public int? PostedBranchID { get; set; }
        public DateTime? ShipBerthDate { get; set; }
        public bool? IsBit { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public decimal? Rate { get; set; }
        public decimal? SystemRate { get; set; }
        public int? Period { get; set; }
        public int? Days { get; set; }
        public int? LCOptionID { get; set; }
        public string? LCNo { get; set; }
        public decimal? LCAmt { get; set; }
        public decimal? AvailableLCAmt { get; set; }
        public decimal? CreditAmt { get; set; }
        public decimal? MarginAmt { get; set; }
        public decimal? InterestAmt { get; set; }
        public decimal? AvailableAmt { get; set; }
        public decimal? AllocationPerc { get; set; }
        public decimal? InterestPerc { get; set; }
        public decimal? TolerencePerc { get; set; }
        public int? CountryID { get; set; }
        public string Country {  get; set; }
        public int? CountryOfOriginID { get; set; }
        public int? MaxDays { get; set; }
        public string? DocumentNo { get; set; }
        public DateTime? DocumentDate { get; set; }
        public int? BEMaxDays { get; set; }
        public DateTime? EntryDate { get; set; }
        public string? EntryNo { get; set; }
        public string? ApplicationCode { get; set; }
        public string? BankAddress { get; set; }
        public string? Unit { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? AcceptDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ClearDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? HandOverTime { get; set; }
        public decimal? LorryHireRate { get; set; }
        public decimal? QtyPerLoad { get; set; }
        public string? PassNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public string? ReferenceNo { get; set; }
        public string? AuditNote { get; set; }
        public string? Terms { get; set; }
        public int? FirmID { get; set; }
        public int? VehicleID { get; set; }
        public int? WeekDays { get; set; }
        public int? BankWeekDays { get; set; }
        public int? RecommendByID { get; set; }
        public DateTime? RecommendDate { get; set; }
        public string? RecommendNote { get; set; }
        public string? RecommendStatus { get; set; }
        public bool? IsHigherApproval { get; set; }
        public int? LCApplnTransID { get; set; }
        public int? InLocID { get; set; }
        public int? OutLocID { get; set; }
        public decimal? ExchangeRate2 { get; set; }
        public string? LCAppNo {  get; set; }
        public string? LCOptionCode { get; set; }
        public string? FromLocation { get; set; }
        public string? SaleSite { get; set; }
        public int? LocTypeID { get; set; }
        public string? RefCode { get; set; }
        public string? RefCode1 { get; set; }
        public int? AccountID { get; set; }
        public int? RouteID { get; set; }
        public int? AccountID2 { get; set; }
        public string? AccountName { get; set; }
        //public int? VehicleID { get; set; }
        public string? VehicleName { get; set; }
        public string? Account2Name { get; set; }
        public decimal? Hours { get; set; }
        public int? Year { get; set; }
        public int? AreaID { get; set; }
        public int? OtherBranchID { get; set; }
        public string? BranchName { get; set; }
        public string? Area { get; set; }
        public int? TaxFormID { get; set; }
        public int? PriceCategoryID { get; set; }
        public string? PriceCategory { get; set; }
        public bool? IsClosed { get; set; }
        public int? DepartmentID { get; set; }
        public string? Department { get; set; }
        public string? PartyName { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public int? ItemID { get; set; }
        public string? ItemName { get; set; }
        public string? EquipMentName { get; set; }
        public string? VATNo { get; set; }
      


    }
    public class FillTransactionEntries
    {
        public int? Id { get; set; }
        public int? TransactionId { get; set; }
        public string? DrCr { get; set;}
        public string? Nature { get; set; }
        public int AccountId { get; set; }
        public DateTime? DueDate { get; set; }
        public int Alias {  get; set; }
        public string? Name { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Debit { get; set;}
        public decimal? Credit { get; set; }
        public decimal? FCAmount { get; set; }
        public DateTime ? BankDate { get; set;}
        public int? RefPageTypeID { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? ExchangeRate { get; set;}
        public int? RefPageTableId { get; set; }
        public string? ReferenceNo { get; set; }
        public string? Description { get; set; }
        public string? TranType { get; set;}
        public bool? IsBillWise { get; set; }//
        public int? RefTransId { get; set; }
        public decimal ? TaxPerc {  get; set; }
        public decimal? ValueOfGoods { get; set; }//
    }
    public class FillVoucherAllocationUsingRef
    {
        public int? Id { get; set; }
        public bool? Selection { get; set; }
        public int? VID { get; set; }
        public int? VEID { get; set; }
        public int? AccountID { get; set; }
        public decimal? Amount { get; set; }
        public decimal? BillAmount { get; set; }
        public int? RefTransId { get; set; }
        public decimal? Allocated { get; set; }
        public string? VNo { get; set; }
        public string? Description { get; set; }
        public string? Account { get; set; }
    }
    public class FillCheques
    {
        public int? Id { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string? ChequeNo { get; set; }
        public int? ClrDays { get; set; }
        public int? BankID { get; set; }
        public string? BankName { get; set; }
        public int? PartyID { get; set; }
        public int? TransactionId { get; set; }
        public decimal? ChqAmount { get; set;}
        public string? PDCPayable { get; set; }
        public int? PDCAccountId { get; set; }
        public int VEID { get; set; }
        public string? Description { get; set; }
        public string? TranType { get; set; }
        public string? CardType { get;set; }
    }
    public class FillTransCollnAllocations
    {
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
        public int? Id { get; set; }
        public int? TransCollectionID { get; set; }
        public int? VAllocationId { get; set; }
    }
    public class FillInvTransItems
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int? ItemId { get; set; }
        public int? SerialNo { get; set; }
        public string? ItemName { get; set; }
        public int? UniqueNo { get; set; }//
        public string? BarCode { get; set; }
        public int? RefTransId1 { get; set; }
        public string? Unit { get; set; }
        public int? PriceCategoryId { get; set; }
        public decimal? Qty { get; set; }
        public int? Pcs { get; set; }
        public decimal? Rate { get; set; }
        public decimal ? ProformaAmount { get; set; }
        public decimal? AdvanceRate { get; set; }
        public decimal? OtherRate { get; set; }
        public int? MasterMiscId1 { get; set; }
        public int? RowType { get; set; }
        public string ? RefCode { get; set; }
        public string? Description { get; set; }
        public string? Remarks { get; set; }
        public bool? IsBit { get; set; }
        public string? ItemCode { get; set; }
        public string? StockType { get; set; }
        public string ? CategoryType { get; set; }
        public string? Commodity { get; set; }
       // public int? CommodityId { get; set; }
        public string ? Category {  get; set; }
        public decimal? LengthCm { get; set; }
        public decimal? LengthFt { get; set; }
        public decimal? LengthIn { get; set; }
        public decimal? ThicknessCm { get; set; }
        public decimal? ThicknessFt { get; set; }
        public decimal? ThicknessIn { get; set; }
        public decimal? GirthFt { get; set; }
        public decimal? GirthIn { get; set; }
        public decimal? GirthCm { get; set; }
        public decimal? POLotQty { get; set; }
        public bool? Selection { get; set; }
        public string ? Quality { get; set; }//
        public string ? LotStatus { get; set; }
        public int? Status { get; set; }//
        public int? InvAvgCostId { get; set; }
        public decimal? Additional { get; set; }
        public decimal? Discount { get; set; }
        public bool? IsReturn { get; set; }
        public decimal? Factor { get; set; }
        public int? QtyPrecision { get; set; }
        public int? BasicQtyPrecision { get; set; }
        public decimal? Margin { get; set; }
        public decimal? RateDisc { get; set; }
        public decimal ? TotalAmount { get; set; }
        public decimal? Amount { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal ? DiscountPerc { get; set; }
        public decimal ? TaxPerc { get; set; }
        public string? TranType { get; set; }
        public int? SizeMasterId { get; set; }
        public decimal? TaxValue { get; set; }
        public int? TaxTypeId { get; set; }
        public decimal? CostPerc { get; set; }
        public int? InLocId { get; set; }
        public int? OutLocId { get; set; }
        public string? BatchNo { get; set; }
        public string? SizeMasterName { get; set; }
        public int ? TaxAccountId { get; set; }
        public DateTime ? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal ? OrderQty { get; set; }
        public decimal? FocQty { get; set; }
        public int? RefId { get; set; }
        public int ? RefTransItemId { get; set; }
        public decimal? StockQty { get; set; }
        public decimal ? BasicQty { get; set; }
        public decimal? TempQty { get; set; }
        public decimal? TempRate { get; set; }
        public decimal? ReplaceQty { get; set; }
        public decimal? PrintedMrp { get; set; }
        public decimal? PrintedRate { get; set; }
        public decimal? PtsRate { get; set; }
        public decimal? PtrRate { get; set; }
        public string? TempBatchNo { get; set; }//
        public int? StockItemId { get; set; }
        public string ? StockItem {  get; set; }
        public string ? ArabicName { get; set; }
        public string? Location { get; set; }
        public string? UnitArabic { get; set; }
        public int? CostAccountId { get; set; }
        public decimal? CgstPerc { get; set; }
        public decimal? CgstValue { get; set; }
        public decimal? SgstPerc { get; set; }
        public decimal? SgstValue { get; set; }
        public string? Hsn { get; set; }
        public int? BrandId { get; set; }
        public string ? BrandName { get; set; }
        public string? ComplaintNature { get; set; }
        public string? AccLocation { get; set; }
        public string? RepairsRequired { get; set; }
        public string? OutLocation { get; set; }
        public decimal? ExchangeRate { get; set; }
        public int? MeasuredById { get; set; }
        public int? CessAccountId { get; set; }
        public decimal? CessPerc { get; set; }
        public decimal? CessValue { get; set; }
        public string? ModelNo { get; set; }//
        public decimal? AvgCost { get; set; }
        public decimal? Profit { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? ParentId { get; set; }
        public string ? ParentName { get; set; }   
    }
    public class FillInvTransItemDetails
    {
        public int Id { get; set; }
        public int TransItemId { get; set; }
        public DateTime Date { get; set; }
        public string? Unit {  get; set; }
        public decimal? Qty { get; set; }
        public decimal? Rate { get; set; }
        public int? LocationId { get; set; }
        public int? ToLocationId { get; set; }
        public string ? ToLocation { get; set; }
        public int? VTypeId { get; set; }
        public string ? Location { get; set; }
        public int? AccountId { get; set; }
        public string ? Party {  get; set; }
        public int? CreditWeeks { get; set; }
        public string? Remarks { get; set; }
        public decimal? BalanceQty { get; set; }//
        public string ? VehicleType { get; set; }//
    }
    public class FillTransactionItemExpenses
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string ? ItemNo { get; set; }
        public int? ItemId { get; set; }
        public int? AccountId { get; set; }
        public int? AccountCode { get; set; }//
        public string? AccountName { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public int ? ChargeTypeId { get; set; }
        public int? VEID { get; set; }
        public DateTime? DueDate { get; set; }
        public int? TransItemId { get; set; }
    }
    public class FillDocuments
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int? DocTypeId { get; set;}
        public string ? DocumentType { get; set; }//
        public string? DocNo { get; set;}
        public string? Name { get; set;}
        public DateTime ? IssueDate { get; set; }
        public DateTime ? ExpiryDate { get; set; }
        public string ? Path { get; set; }
        public int? OriginalCopies { get; set; }
        public int? DuplicateCopies { get; set; }
        public int? AccountId { get; set; }
        public int? OrginType { get; set; }
        public string? Description { get; set; }
        public string? ContactInfo { get; set; }
        public string? IssuePlace { get; set; }
        public string? DocLocation { get; set; }
        public bool? Active { get; set; }
        public string? ModuleName { get; set; }
        public int? DocumentId { get; set; }
    }
    // public class FiTransactionAdditionals        (SpGetTransactionAdditionals)
    public class FillTransactionExpenses
    {
        public int SNo { get; set; }
        public int ID { get; set; }
        public int TransactionID { get; set; }
            public int AccountID { get; set; }
            public string? Description { get; set; }
            public decimal Amount { get; set; }
            public int PayableAccountID { get; set; }
            public string ExpenseAccountName { get; set; }
            public int ChargeTypeID { get; set; }
            public string TranType { get; set; }
            public string AccountName { get; set; }
            public string Alias { get; set; }
            public decimal Debit { get; set; }
            public decimal Credit { get; set; }
        }

    
    public class FillDocumentRequests
    {
        public int? Id { get; set; }
        public int? TransactionId { get; set; }
        public int? DocId { get; set; }
        public int? DocTypeId { get; set; }
        public int? Originals { get; set; }
        public int? Duplicates { get; set; }
        public bool? IsReceived { get; set; }
        public string? Description { get; set; }
        public bool? LcMandatory { get; set; }  
    }
    public class FillDocumentReferences
    {
        public int? TransactionId { get; set; }
        public int? DocId { get; set; }
    }
    public class FillTransactionReferences
    {
        public int? ID { get; set; }
        public int? TransactionID    { get; set;}
        public int? RefTransID { get; set;}
        public string? Description { get; set; }
    }
    public class FillTransLoadSchedules
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string? Code { get; set; }
        public DateTime? Date { get; set; }
        public int? LocationId { get; set; }
        public int? VehicleId { get; set; }
        public decimal? QtyPerLoad { get; set; }
        public string? Unit { get; set; }
        public string? Description { get; set; }
        public int? RefId { get; set; }
        public int ? LoadId { get; set; }
        public string ? LoadCode { get; set; }
        public int ? VehicleTypeId { get; set; }
        public int? StatusId { get; set; }
        public string? LoadStatus { get; set;}
        public bool? Active { get; set; }
        public int? GoodsShiftingId { get; set; }
        public int? GoodsShiftingCode { get; set; }//
        public string ? Tips { get; set; }
        public string? ImageUrl { get;set; }
    }
    public class FillTransCollections
    {
        public int? Id { get; set; }
        public int? TransactionId { get; set; }
        public int? PayTypeId { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string? InstrumentBank { get; set; }
        public DateTime? InstrumentDate { get; set; }
        public string? InstrumentType { get; set; }
        public string ? Name { get; set; }
        public int? Particulars { get; set; }
        public string? InstrumentNo { get; set; }
        public int? InstrumentTypeId { get; set; }
        public int? Veid { get; set; }
    }
    public class FillTransEmployees
    {
        public int Id { get; set; }
        public int? TypeId { get; set; }
        public int? HrEmployeeId { get; set; }
        public string? Name { get; set; }
        public string? TypeName { get; set; }
        public string ? Note { get; set; }
    }
    public class VMFuelLog
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? VEID { get; set; }
        public int? TypeId { get; set; }
        public DateTime ? Date { get; set; }
        public string? FuelStationName { get; set; }
        public string? TypeName { get; set; }
        public decimal ? Qty { get; set; }
        public decimal? Rate { get; set;}
        public decimal? Reading { get; set;}
        public string? Note { get; set; }
    }
    public class FillDocumentImages 
    {
        public int Id { get; set; }
        public int? DocId { get; set;}
        public string? Path { get; set; }
        public string? FileName { get; set; }
        public DateTime? Date { get; set; }
        public bool? Active { get; set; }
        public string? Remarks { get; set; }
        public int? DocFormatId { get; set; }
        public int? DocStatusId { get; set; }
        public string? VersionNo { get; set; }
        public int? SourceId { get; set; }
    }
    public class FillHRFinalSettlement
    {
        public int? Id { get; set; }
        public int? TransactionId { get; set; }
        public string? Type { get; set; } = null!;
        public string? Remarks { get; set; } = null!;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal? Days { get; set; }
        public decimal? GrDays { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Amount { get; set; }
    }
    public class FillTransCostAllocations
    {
        public int? Id { get; set; }
        public int? Veid { get; set; }
        public int? CostCentreId { get; set; }
        public string? ProjectCode { get; set; }
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public int? AccountId { get; set; }
        public string? AccountName { get; set;}

    }
}
