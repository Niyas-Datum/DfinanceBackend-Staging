using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto
{
    public class PhysicalStockDto
    {
        public int Id { get; set; }
        public string VoucherNo { get; set; }
        public DropdownDto Warehouse { get; set; }
        public DateTime Date { get; set; }
        public DropdownDto Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        public string? Terms { get; set; }
        public List<InvTransItemDto> Items { get; set; }
    }
}
