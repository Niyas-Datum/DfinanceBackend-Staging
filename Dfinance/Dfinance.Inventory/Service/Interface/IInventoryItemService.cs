using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Shared.Domain;

namespace Dfinance.Inventory.Service.Interface
{
    public interface IInventoryItemService
    {
        CommonResponse SaveInvTransItems(PurchaseDto purchaseDto, int voucherId, int transId);
        CommonResponse GetItemData(int itemId, int partyId, int voucherId);
        CommonResponse DeleteInvTransItem(int Id);
        CommonResponse UpdateInvTransItems(PurchaseDto purchaseDto, int voucherId, int transId);
        
        
    }
}
