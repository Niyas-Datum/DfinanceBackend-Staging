using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Purchase.Services.Interface
{
    public interface IPurchaseRequestService
    {
        CommonResponse SavePurchaseRequest(InventoryTransactionDto invTranseDto, int PageId, int voucherId);
        CommonResponse UpdatePurchaseRequest(InventoryTransactionDto invTranseDto, int PageId, int voucherId);
        CommonResponse DeletePurchaseRequest(int TransId, int PageId);
    }
}
