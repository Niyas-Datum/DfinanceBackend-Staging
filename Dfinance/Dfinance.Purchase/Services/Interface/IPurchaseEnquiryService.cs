using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Purchase.Services.Interface
{
    public interface IPurchaseEnquiryService
    {
        CommonResponse SavePurchaseEnquiry(InventoryTransactionDto purchaseEnqDto, int PageId, int voucherId);
        CommonResponse UpdatePurchaseEnquiry(InventoryTransactionDto purchaseEnqDto, int PageId, int voucherId);
        CommonResponse DeletePurchaseEnq(int TransId, int pageId);
        CommonResponse CancelPurchaseEnq(int TransId, int pageId, string reason);
    }
}
