using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Purchase.Services.Interface
{
    public interface IPurchaseReturnService
    {
        CommonResponse SavePurchaseRtn(InventoryTransactionDto purchaseRtnDto, int PageId, int voucherId);
        CommonResponse UpdatePurchaseRtn(InventoryTransactionDto purchaseRtnDto, int PageId, int voucherId);
        CommonResponse DeletePurchaseRtn(int TransId, int pageId);
        CommonResponse CancelPurchaseRtn(int TransId, int pageId, string reason);
    }
}
