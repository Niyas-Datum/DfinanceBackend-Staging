using Dfinance.DataModels.Dto.Common;


namespace Dfinance.DataModels.Dto.Inventory
{
    public class PurchaseDto
    {
        public int VoucherNo { get; set; }//TransNo
        public DateTime Date { get; set; }
        public PopUpDto? Reference { get; set; }
        public DropdownDto Warehouse { get; set; }
        public int Supplier {  get; set; }  //POPUP
        public PopUpDto SalesMan { get; set; }//POPUP
        public int Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        public PopUpDto Project { get; set; }//POPUP
        public string VATNo { get; set; }
        public DateTime PartyInvoiceDate { get; set; }
        public int PartyInvoiceNo {  get; set; }    
        public string Description { get; set; }
        //Items
        public string? ItemCode {  get; set; }
        public string ItemName { get; set; }
        public string BatchNo {  get; set; }
        public string unit { get; set;}
        public decimal Qty { get; set; } 
        public decimal? FocQty { get; set; }
        public decimal Rate { get;}
        public int GrossAmt { get; set; }
        public decimal? Discount { get; set;}
        public int Amount { get; set; } 
        public decimal TaxPerc { get; set; }

        //Bottom
        public string? Terms { get; set; }
        public decimal TotalDisc { get; set; }
        public decimal Amt { get; set; }
        public decimal Roundoff { get; set; }
        public decimal NetAmount { get; set; }
        public decimal? Tax { get; set; }
        public int AddCharges { get; }
        public decimal GrandTotal {  get; set; }
        public int PayType { get; set; }
        public decimal? Advance { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal Cash { get; set; }
        public decimal Card { get; set; }
        public decimal Balance { get; set; }
        public int Cheque { get; set; }
        public DateTime DueDate { get; set; }
    }
}
