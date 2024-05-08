namespace Dfinance.Core.Views.Inventory
{
    public class FillAdvanceView
    {
        public int? VID { get; set; }    //TransactionId
        public int? VEID { get; set; }   //TransactionTableId
        public string? VNo { get; set; }  //voucher code + TransactionNo => InvoiceNo
        public DateTime? VDate { get; set; }  //Transaction Date
        public string? Description { get; set; }  //TransactionEntries(desc)
        public decimal? BillAmount { get; set; }  // amount * exchangerate
        public decimal? Amount { get; set; }     //0
        public int? AccountID { get; set; }    //accId(trans.entries)
        public bool? Selection { get; set; }   
        public decimal? Allocated { get; set; }  
        public string? Account { get; set; }
        public string? DrCr { get; set; }   //
        public int? PartyInvNo { get; set; }  //EntryNo(trans.addi)
        public DateTime? PartyInvDate { get; set; } //EntryDate(trans.addi)


    }
}
