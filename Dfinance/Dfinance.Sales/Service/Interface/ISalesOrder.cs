using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Sales.Service.Interface
{
    public interface ISalesOrder
    {
        CommonResponse SaveSalesOrder(InventoryTransactionDto purchaseDto, int PageId, int voucherId);
        CommonResponse UpdateSalesOrder(InventoryTransactionDto purchaseDto, int PageId, int voucherId);
    }
}
