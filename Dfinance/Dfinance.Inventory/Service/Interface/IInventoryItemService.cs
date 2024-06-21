using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Shared.Domain;

namespace Dfinance.Inventory.Service.Interface
{
    public interface IInventoryItemService
    {
        CommonResponse SaveInvTransItems(List<InvTransItemDto> Items, int voucherId, int transId, decimal? exchangeRate, int? warehouse);        
        CommonResponse GetItemData(int itemId, int partyId, int voucherId);
        CommonResponse DeleteInvTransItem(int Id);
        CommonResponse UpdateInvTransItems(List<InvTransItemDto> Items, int voucherId, int transId, decimal? exchangeRate, int? warehouse);
    }
}
