using Dfinance.Shared.Domain;

namespace Dfinance.Item.Services.Inventory.Interface
{
    public interface IItemMasterService
    {
        CommonResponse FillItemMaster(int Id);      
        //CommonResponse FillItemByID(int Id, int BranchId);
        CommonResponse GetNextItemCode();       
        CommonResponse ParentItemPopup();
        CommonResponse SaveItemMaster(ItemMasterDto itemDto);
        CommonResponse UpdateItemMaster(ItemMasterDto itemDto, int Id);
        CommonResponse DeleteItem(int ItemId);
        CommonResponse GenerateBarCode();
        CommonResponse TaxDropDown();

    }
}
