using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Validation;


namespace Dfinance.DataModels.Dto.Inventory.Purchase
{
    public class InventoryTransactionDto
    {
        public int? Id { get; set; }
       
        public string? VoucherNo { get; set; }//TransNo

        [NullValidation(ErrorMessage = "Voucher Date is Mandatory!!")]
        public DateTime Date { get; set; }
        public string? Reference {  get; set; }
        public List<ReferenceDto>? References { get; set; }

        [NullValidation(ErrorMessage = "Party is Mandatory!!")]
        public PopUpDto Party { get; set; }  //POPUP
        public DropdownDto? Currency { get; set; }

        [DecimalValidation(4, ErrorMessage = "Enter valid ExchangeRate!!")]
        public decimal ExchangeRate { get; set; }

        //[NullValidation(ErrorMessage = "Warehouse is Mandatory!!")]
        //public DropdownDto Warehouse { get; set; }//dropdown
        public PopUpDto Project { get; set; }//POPUP
        public string? Description { get; set; }        
        public bool? GrossAmountEdit { get; set; }
        public InvTransactionAdditionalDto? FiTransactionAdditional { get; set; }
        public List<InvTransItemDto> Items { get; set; }
        public InvTransactionEntriesDto? TransactionEntries { get; set; }       
        
    }
   public class InvTransItemDto
    {
        public int? TransactionId {  get; set; }              
        public int ItemId { get; set; }
        public string? ItemCode { get; set; }//popup
        public string? ItemName { get; set; }
        public string? BatchNo { get; set; }//generated Batchno
        public UnitPopupDto? Unit { get; set; }//popup
        
        [DecimalValidation(4, ErrorMessage = "Quantity is not valid!!")]
        public decimal Qty { get; set; }
        [DecimalValidation(4, ErrorMessage = "FOCQty is not valid!!")]
        public decimal? FocQty { get; set; }
        public decimal? BasicQty { get; set; }
        public decimal? Additional { get; set; }
        [DecimalValidation(4, ErrorMessage = "Rate is not valid!!")]
        [NullValidation(ErrorMessage = "Rate must be greater than 0!!")]
        public decimal Rate { get; set; }
        [DecimalValidation(4, ErrorMessage = "OtherRate is not valid!!")]
        public decimal? OtherRate { get; set; }
        [DecimalValidation(4, ErrorMessage = "Margin is not valid!!")]
        public decimal? Margin { get; set; }
        [DecimalValidation(4, ErrorMessage = "RateDiscount is not valid!!")]
        public decimal? RateDisc { get; set; }
        [DecimalValidation(4, ErrorMessage = "GrossAmt is not valid!!")]
        public decimal? GrossAmt { get; set; }
        [DecimalValidation(4, ErrorMessage = "Discount is not valid!!")]
        public decimal? Discount { get; set; }
        [DecimalValidation(4, ErrorMessage = "DiscountPerc is not valid!!")]
        public decimal? DiscountPerc { get; set; }
        [DecimalValidation(4, ErrorMessage = "Amount is not valid!!")]
        public decimal Amount { get; set; }
        [DecimalValidation(4, ErrorMessage = "TaxValue is not valid!!")]
        public decimal? TaxValue { get; set; }
        [DecimalValidation(4, ErrorMessage = "TaxPerc is not valid!!")]
        public decimal? TaxPerc { get; set; }
        [DecimalValidation(4, ErrorMessage = "PrintedMRP is not valid!!")]
        public decimal? PrintedMRP { get; set; }
        [DecimalValidation(4, ErrorMessage = "PtsRate is not valid!!")]
        public decimal? PtsRate { get; set; }
        [DecimalValidation(4, ErrorMessage = "PtrRate is not valid!!")]
        public decimal? PtrRate { get; set; }
        public int? Pcs { get; set; }
        public int? StockItemId { get; set; }
        [DecimalValidation(4, ErrorMessage = "Total is not valid!!")]
        public decimal? Total {  get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Description { get; set; }
        [DecimalValidation(4, ErrorMessage = "LengthFt is not valid!!")]
        public decimal? LengthFt { get; set; }
        [DecimalValidation(4, ErrorMessage = "LengthIn is not valid!!")]
        public decimal? LengthIn { get; set; }
        [DecimalValidation(4, ErrorMessage = "LengthCm is not valid!!")]
        public decimal? LengthCm { get; set; }
        [DecimalValidation(4, ErrorMessage = "GirthFt is not valid!!")]
        public decimal? GirthFt { get; set; }
        [DecimalValidation(4, ErrorMessage = "GirthIn is not valid!!")]
        public decimal? GirthIn { get; set; }
        [DecimalValidation(4, ErrorMessage = "GirthCm is not valid!!")]
        public decimal? GirthCm { get; set; }
        [DecimalValidation(4, ErrorMessage = "ThicknessFt is not valid!!")]
        public decimal? ThicknessFt { get; set; }
        [DecimalValidation(4, ErrorMessage = "ThicknessIn is not valid!!")]
        public decimal? ThicknessIn { get; set; }
        [DecimalValidation(4, ErrorMessage = "ThicknessCm is not valid!!")]
        public decimal? ThicknessCm { get; set; }
        public string? Remarks { get; set; }
        public int? TaxTypeId { get;set; }
        public int? TaxAccountId { get; set; }
        public int? CostAccountId { get; set; }
        public int? BrandId {  get; set; }
        public decimal? Profit { get; set; }
        public string? RepairsRequired {  get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        [DecimalValidation(4, ErrorMessage = "ReplaceQty is not valid!!")]
        public decimal? ReplaceQty { get; set; }
        [DecimalValidation(4, ErrorMessage = "PrintedRate is not valid!!")]
        public decimal? PrintedRate { get; set; }
        public string? Hsn { get; set; }
        [DecimalValidation(4, ErrorMessage = "AvgCost is not valid!!")]
        public decimal? AvgCost { get; set; }
        public bool? IsReturn {  get; set; }
        public DateTime? ManufactureDate { get; set; }
        public PopUpDto? PriceCategory { get; set; }//popup
        public PopUpDto? SizeMaster { get; set; }//popup according to settings
        public List<InvUniqueItemDto>? UniqueItems { get; set; }
    }

    public class InvUniqueItemDto
    {
        
        public string? UniqueNumber {  get; set; }
    }
    public class InvTransactionEntriesDto
    {       
       // public string? Terms { get; set; }
        [DecimalValidation(4, ErrorMessage = "TotalDisc is not valid!!")]
        public decimal? TotalDisc { get; set; }
        [DecimalValidation(4, ErrorMessage = "Amount is not valid!!")]
        public decimal? Amt { get; set; }
        [DecimalValidation(4, ErrorMessage = "Roundoff is not valid!!")]
        public decimal? Roundoff { get; set; }
        [DecimalValidation(4, ErrorMessage = "NetAmount is not valid!!")]
        public decimal? NetAmount { get; set; }
       
        [DecimalValidation(4, ErrorMessage = "GrandTotal is not valid!!")]
        public decimal? GrandTotal { get; set; }
        public DropdownDto PayType { get; set; }
        public DateTime? DueDate { get; set; }
        [DecimalValidation(4, ErrorMessage = "TotalPaid is not valid!!")]
        public decimal? TotalPaid { get; set; }
      
        [DecimalValidation(4, ErrorMessage = "Balance is not valid!!")]
        public decimal Balance { get; set; }
        public List<InvAdvanceDto>? Advance { get; set; }
        public List<InvAccountDetailsDto>? Cash { get; set; }
        public List<InvAccountDetailsDto>? Card { get; set; }
        public List<InvChequesDto>? Cheque { get; set; }
        public List<InvAccountDetailsDto>? Tax { get; set; }
        public List<InvAccountDetailsDto>? AddCharges { get; set; }
        
       
    }
    public class InvAccountDetailsDto
    {
       
       // public int AccountId { get; set; }
        public AccountNamePopUpDto? AccountCode { get; set; }
        //public string? AccountName { get; set; }
        public string? Description { get; set; }
        [DecimalValidation(4, ErrorMessage = "Amount is not valid!!")]
        public decimal? Amount { get; set; }
       // public string? TransType { get; set; }
        public AccountNamePopUpDto? PayableAccount { get; set; }
    }
    public class InvChequesDto
    {
      
        public AccountNamePopUpDto? PDCPayable { get; set; }
        public int VEID { get; set; }
        public string? CardType { get; set; }
        [DecimalValidation(4, ErrorMessage = "Commission is not valid!!")]
        public decimal? Commission { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public int? ClrDays { get; set; }
        public int? BankID { get; set; }
        public AccountNamePopUpDto? BankName { get; set; }
        public string? Status { get; set; }
        public int? PartyID { get; set; }
        public string? Description { get; set; }
        [DecimalValidation(4, ErrorMessage = "Amount is not valid!!")]
        public decimal? Amount { get; set; }
       
    }
    public class InvAdvanceDto

    {       
        public int? VID { get; set; }    
        public int? VEID { get; set; }   
        public string? VNo { get; set; }  
        public DateTime? VDate { get; set; } 
        public string? Description { get; set; }
        [DecimalValidation(4, ErrorMessage = "BillAmount is not valid!!")]
        public decimal? BillAmount { get; set; }
        [DecimalValidation(4, ErrorMessage = "Amount is not valid!!")]
        public decimal? Amount { get; set; }    
        public int? AccountID { get; set; }   
        public bool? Selection { get; set; }
        [DecimalValidation(4, ErrorMessage = "Allocated is not valid!!")]
        public decimal? Allocated { get; set; }
        public string? Account { get; set; }
        public string? DrCr { get; set; }   
        public int? PartyInvNo { get; set; }  
        public DateTime? PartyInvDate { get; set; } 
    }
    public class InvTransactionAdditionalDto
    {
        public int TransactionId {  get; set; }
        public string? Terms { get; set; }        
        [NullValidation(ErrorMessage = "Warehouse is Mandatory!!")]
        public DropdownDto? Warehouse { get; set; }//dropdown for purchase,sales
        public string? PartyInvoiceNo { get; set; }
        public DateTime? PartyDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? OrderNo { get; set; }
        public string? PartyNameandAddress { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DropdownDto? TransPortationType { get; set; }
        public int? CreditPeriod { get; set; }
        public PopUpDto? SalesMan { get; set; }
        public DropdownDto? SalesArea { get; set; }
        public decimal? StaffIncentives { get; set; }
        public string? MobileNo { get; set; }
        public PopUpDto? VehicleNo { get; set; }
        public string? Attention { get; set; }
        public string? DespatchNo { get; set; }
        public DateTime? DespatchDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? DeliveryNote { get; set; }
        public string? PartyName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public PopUpDto? DelivaryLocation { get; set; }
        public string? TermsOfDelivery { get; set; }
        public DropdownDto PayType { get; set; }
        public bool? Approve {  get; set; }
        public bool? CloseVoucher {  get; set; }

    }
}

