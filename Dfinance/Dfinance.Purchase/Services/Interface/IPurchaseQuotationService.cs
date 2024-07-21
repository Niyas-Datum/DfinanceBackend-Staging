using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Purchase.Services.Interface
{
    public interface IPurchaseQuotationService
    {
        CommonResponse SavePurchaseQtn(InventoryTransactionDto invTranseDto, int PageId, int voucherId);
        CommonResponse UpdatePurchaseQuotation(InventoryTransactionDto invTranseDto, int PageId, int voucherId);
        CommonResponse DeletePurchaseQuotation(int TransId, int PageId);
        CommonResponse CancelPurchaseQuotation(int TransId, int pageId, string reason);
    }
}
