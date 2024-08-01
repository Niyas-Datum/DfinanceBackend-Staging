using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto
{
    public class StockRtnAdjustDto
    {
        public int Id { get; set; }
        public string? VoucherNo { get; set; }
        public DateTime? Date { get; set; }
        public DropdownDto? Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public List<InvTransItemDto> Items { get; set; }
        public List<ReferenceStockDto>? References { get; set; }
        public DropdownDto Warehouse { get; set; }
        public string? Description { get; set; }
        public string? Terms { get; set; }
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
