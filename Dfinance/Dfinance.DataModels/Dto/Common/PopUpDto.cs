using Dfinance.Core.Views.Inventory.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Common
{
    public class PopUpDto
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        public string? Code {  get; set; }
        public string? Description { get; set; }
    }
    public class AccountNamePopUpDto
    {
        public string? Alias {  get; set; }
        public string? Name { get; set; }
        public int? ID { get; set; }
    }

    public class ReferenceDto
    {
        public bool? Sel { get; set; }
        public bool? AddItem { get; set; }
        public int? VoucherId { get; set; }
        public string? VNo { get; set; }
        public DateTime? VDate { get; set; }
        public string? ReferenceNo { get; set; }
        public int? AccountId { get; set; }
        public string? AccountName { get; set; }
        public decimal? Amount { get; set; }
        public string? PartyInvNo { get; set; }
        public DateTime? PartyInvDate { get; set; }
        public int? Id { get; set; }
        public string? VoucherType { get; set; }
        public string? MobileNo { get; set; }
        public List<ImportItemListDto> RefItems { get; set; }
    }
    public class ImportItemListDto
    {
        public bool? Select { get; set; }
        public int? ItemID { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? Unit { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Rate { get; set; }
        public decimal? PrintedMRP { get; set; }
        public decimal? Amount { get; set; }
    }
    public class FillAdvanceDto

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
