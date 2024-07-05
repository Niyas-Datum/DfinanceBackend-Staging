using Dfinance.DataModels.Dto.Item;
using Dfinance.Shared.Domain;

namespace Dfinance.Item.Services.Inventory.Interface
{
    public interface IItemMasterService
    {
        CommonResponse FillItemMaster(int[]? catId, int[]? brandId, string search = null, int pageNo = 0, int limit = 0);

        CommonResponse FillItemByID(int pageId,int Id, int BranchId = 0);
        CommonResponse GetNextItemCode();
        CommonResponse ParentItemPopup();
        CommonResponse SaveItemMaster(ItemMasterDto itemDto,int pageId);
        CommonResponse UpdateItemMaster(ItemMasterDto itemDto, int Id, int pageId);
        CommonResponse DeleteItem(int ItemId, int pageId);
        CommonResponse GenerateBarCode();
        CommonResponse TaxDropDown();       
        CommonResponse FillTransItems(int partyId, int PageID, int locId, int voucherId);
        //CommonResponse GetUniqueExpiryItem(int itemId);
        CommonResponse GetItemSearch(int? itemId,string? value,string? criteria);
        CommonResponse GetItemRegister(int? branchId, int? warehouseId, bool less = false, DateTime? date = null) 
     CommonResponse GetInventoryAgeing(int? AccountID, DateTime? FromDate, DateTime? ToDate, bool? OpeningBalance, int? VoucherID, string? Nature);
   //  CommonResponse GetItemExpiryReport(ItemExpiryReportDto itemExpiryReportDto);
    
    }
}
