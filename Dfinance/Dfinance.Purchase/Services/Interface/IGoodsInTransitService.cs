using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Purchase.Services.Interface
{
    public interface IGoodsInTransitService
    {
        CommonResponse SaveGoodsInTransit(InventoryTransactionDto invTranseDto, int PageId, int voucherId);
        CommonResponse UpdateGoodsInTransit(InventoryTransactionDto invTranseDto, int PageId, int voucherId);
        CommonResponse DeleteGoodsInTransit(int TransId, int PageId);
        CommonResponse CancelGoodsInTransit(int TransId, int pageId, string reason);
    }
}
