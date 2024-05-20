using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Shared.Domain;

namespace Dfinance.Inventory.Service.Interface
{
    public interface IInventoryItemService
    {
        CommonResponse SaveInvTransItems(InventoryTransactionDto purchaseDto, int voucherId, int transId);        
        CommonResponse GetItemData(int itemId, int partyId, int voucherId);
        CommonResponse DeleteInvTransItem(int Id);
        CommonResponse UpdateInvTransItems(InventoryTransactionDto purchaseDto, int voucherId, int transId);
        
        
    }
}
