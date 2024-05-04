using Dfinance.Shared.Domain;

namespace Dfinance.Item.Services.Inventory.Interface
{
    public interface IItemUnitsService
    {
        CommonResponse SaveItemUnits(List<ItemUnitsDto> unitList, List<int> branch, int ItemId);
        CommonResponse UpdateItemUnits(List<ItemUnitsDto> units, int ItemId, List<int> branch);
        CommonResponse DeleteItemUnits(int UnitId);
        CommonResponse FillItemUnits(int ItemId, int BranchId );
        CommonResponse GetItemUnits(int itemId);

    }
}
