using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Purchase.Services.Interface
{
    public interface IInternationalPurchaseService
    {
        CommonResponse GetData(int pageId, int voucherId);
        CommonResponse FillTransItems(int partyId, int PageID, int locId, int voucherId);
        CommonResponse FillInPurchase(int PageId, bool? post);
        CommonResponse FillInPurchaseById(int TransId, int PageId);
        CommonResponse SaveInPurchase(InventoryTransactionDto invTranseDto, int PageId, int voucherId);
        CommonResponse UpdateInPurchase(InventoryTransactionDto invTranseDto, int PageId, int voucherId);
        CommonResponse DeleteInPurchase(int TransId, int PageId);
        CommonResponse CancelInPurchase(int TransId, int pageId, string reason);
    }
}
