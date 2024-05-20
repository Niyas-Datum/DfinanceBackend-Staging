using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Purchase.Services.Interface
{
    public interface IPurchaseOrderService
    {
        CommonResponse SavePurchaseOrder(InventoryTransactionDto invTranseDto, int PageId, int voucherId);
        CommonResponse UpdatePurchaseOrder(InventoryTransactionDto invTranseDto, int PageId, int voucherId);
        CommonResponse DeletePurchaseOrder(int TransId, int PageId);
    }
}
