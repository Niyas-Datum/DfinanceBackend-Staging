using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;

namespace Dfinance.DataModels.Dto.Inventory.Stock
{
    public class OpeningStockDto
    {
        public int? Id { get; set; }
        public string VoucherNo {  get; set; }
        public DropdownDto Warehouse { get; set; }
        public DateTime Date {  get; set; }
        //public string? Reference {  get; set; }
        //public PopUpDto? Salesman { get; set; }
        public DropdownDto Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        public string? Terms {  get; set; }
        public List<InvTransItemDto> transItems { get; set; }
        //public List<ReferenceDto> references { get; set; }
    }
}
