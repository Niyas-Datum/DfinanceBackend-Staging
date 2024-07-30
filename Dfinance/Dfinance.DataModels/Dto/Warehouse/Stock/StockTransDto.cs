using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.DataModels.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto
{
    public class StockTransDto
    {
        public int Id { get; set; }
        public string VoucherNo { get; set; }
        public DropdownDto Warehouse { get; set; }
        public DateTime Date { get; set; }
        public DropdownDto Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        public string? Description { get; set; }
        public List<InvTransItemDto> Items { get; set; }
        public bool? Cancelled { get; set; }
        public int VoucherId { get; set; }
        public int PageId { get; set; }
        public string? Reference { get; set; }
        public StockTransAdditional StockTransAdditional { get; set; }
        public List<ReferenceStockDto> references { get; set; }
    }
    public class StockTransAdditional
    {
        public int TransactionId { get; set; }
        public string? Terms { get; set; }
        [NullValidation(ErrorMessage = "Warehouse is Mandatory!!")]
        public DropdownDto? FromLocationId { get; set; }
        public DropdownDto? ToLocationId { get; set; }
        public DropdownDto? InLocId { get; set; }
        public DropdownDto? OutLocId { get; set; }
        public PopUpDto? SalesMan { get; set; }
    }
    public class ReferenceStockDto
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
    }

}
