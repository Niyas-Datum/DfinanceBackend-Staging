

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Dfinance.DataModels.Validation;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Common;

public class ItemMasterDto
    {  
   public int? Id { get; set; }
    [NullValidation(ErrorMessage ="ItemCode is Mandatory!!")]   
    public string ItemCode { get; set; }
    
   [NullValidation( ErrorMessage = "Item Name is mandatory!!")]
    public string ItemName { get; set; }
    public string? ArabicName { get; set; }     
   
    public UnitPopupDto Unit { get; set; }    
    public string? BarCode { get; set; }
        
    public CatPopupDto? Category { get; set; }//popup=>commodity in db
    public bool? IsUniqueItem { get; set; }
    public bool? StockItem { get; set; }
   
   
    [DecimalValidation(4,ErrorMessage = "Enter valid Purchase Rate!!")]
    public decimal? CostPrice { get; set; }

    [DecimalValidation(4, ErrorMessage = "Enter valid Selling Price!!")]
    public decimal? SellingPrice { get; set; }

    [DecimalValidation(4, ErrorMessage = "Enter valid MRP!!")]
    public decimal? MRP { get; set; }

    [DecimalValidation(4, ErrorMessage = "Enter valid Margin!!")]
    public decimal? Margin { get; set; }

    [DecimalValidation(4, ErrorMessage = "Enter valid MarginValue!!")]
    public decimal? MarginValue { get; set; }
        public DropDownDtoName? TaxType { get; set; }//dropdown
        public bool? IsExpiry { get; set; }

    [RegularExpression("^[0-9]+$", ErrorMessage = "Enter valid ExpiryPeriod!!")]
        public int? ExpiryPeriod { get; set; }
        public bool? IsFinishedGood { get; set; }
        public bool? IsRawMaterial { get; set; }

        public string? Location { get; set; }

    [DecimalValidation(4, ErrorMessage = "Enter valid Item Discount!!")]
    public decimal? ItemDisc { get; set; }
        public string? HSN { get; set; }
        public ParentItemPopupDto? Parent { get; set; }//popup=>id
        public DropdownDto? Quality { get; set; }//dropdown=>id
        public string? ModelNo { get; set; }
        public PopUpDto? Color { get; set; }//popup=>id
        public PopUpDto? Brand { get; set; }//popup=>id
        public PopUpDto? CountryOfOrigin { get; set; }//popup=>id

    [DecimalValidation(4, ErrorMessage = "Enter valid ROL!!")]
    public decimal? ROL { get; set; }
        public string? PartNo {  get; set; }//StockCode

    [DecimalValidation(4, ErrorMessage = "Enter valid ROQ!!")]
    public decimal? ROQ { get; set; }
        public string? Manufacturer { get; set; }

    [DecimalValidation(4, ErrorMessage = "Enter valid Weight!!")]
    public decimal? Weight { get; set; }
        public string? ShipMark { get; set; }
        public string? PaintMark { get; set; }
        public UnitPopupDto? SellingUnit { get; set; }//popup=>unit
        public string? OEMNo { get; set; }
        public UnitPopupDto? PurchaseUnit { get; set; }//popup=>unit
        public bool? IsGroup { get; set; }
        public bool? Active { get; set; }
        public DropDownDtoName? InvAccount { get; set; }//dropdown=>InvAccountId
        public DropDownDtoName? SalesAccount { get; set; }//dropdown=>SalesAccountId
        public DropDownDtoName? CostAccount { get; set; }//dropdown=>CostAccountId
        public DropDownDtoName? PurchaseAccount { get; set; }//dropdown=>PurchaseAccountId
        public string? Remarks { get; set; }       
        public List<ItemUnitsDto>? ItemUnit { get; set; }        
        public List<DropDownDtoName> Branch {  get; set; }
        //public IFormFile? ImageFile { get; set; }
      public string? ImageFile { get; set; }
}

    public class ItemUnitsDto
    {
        public int UnitID {  get; set; }

    //[NullValidation(ErrorMessage = "Unit is mandatory!!")]
    public UnitPopupDto Unit {  get; set; }

    [NullValidation(ErrorMessage = "Basic Unit is mandatory!!")]
    public string BasicUnit {  get; set; }

    [NullValidation(ErrorMessage = "Factor is mandatory!!")]
    public decimal Factor {  get; set; }
  
    public decimal? PurchaseRate {  get; set; }

    [DecimalValidation(4, ErrorMessage = "Enter valid Selling Price!!")]
    public decimal? SellingPrice { get; set; }

    [DecimalValidation(4, ErrorMessage = "Enter valid MRP!!")]
    public decimal? MRP { get; set; }

    [DecimalValidation(4, ErrorMessage = "Enter valid WholeSalePrice!!")]
    public decimal? WholeSalePrice { get; set; }

    [DecimalValidation(4, ErrorMessage = "Enter valid RetailPrice!!")]
    public decimal? RetailPrice { get; set; }

    [DecimalValidation(4, ErrorMessage = "Enter valid WholeSalePrice2!!")]
    public decimal? WholeSalePrice2 { get; set; }//DiscountPrice in db

    [DecimalValidation(4, ErrorMessage = "Enter valid RetailPrice2!!")]
    public decimal? RetailPrice2 { get; set; }//OtherRate in db

    [DecimalValidation(4, ErrorMessage = "Enter valid LowestRate!!")]
    public decimal? LowestRate { get; set; }

    [DataType(DataType.Text,ErrorMessage ="Enter valid Barcode!!")]
    public string? BarCode { get; set; }

        public bool? Active {  get; set; }
        public int? Status {  get; set; }//1 for insert,2 for edit,3 for delete                                       

}
    


