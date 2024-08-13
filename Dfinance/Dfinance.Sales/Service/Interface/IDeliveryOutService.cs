using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Sales.Service.Interface
{
    public interface IDeliveryOutService
    {
        public CommonResponse SaveDeliveryOut(InventoryTransactionDto salesDto, int PageId, int voucherId);
        public CommonResponse UpdateDeliveryOut(InventoryTransactionDto salesDto, int PageId, int voucherId)
    }
}
