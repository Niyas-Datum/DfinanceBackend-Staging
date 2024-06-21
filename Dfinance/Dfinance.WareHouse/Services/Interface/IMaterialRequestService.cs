using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.WareHouse.Services.Interface
{
    public interface IMaterialRequestService
    {
        CommonResponse GetData(int pageId, int voucherId);
        CommonResponse FillFromWarehouse(int branchId);
        CommonResponse FillMaster(int? PageId = 0, int? transId = 0, int? voucherId = 0);
        CommonResponse FillById(int TransId, int PageId);
        CommonResponse FillTransItems(int partyId, int PageID, int locId, int voucherId);
       // CommonResponse GetMaterialSettings();
        CommonResponse SaveMaterialReq(MaterialTransferDto materialDto, int voucherId, int pageId);
        CommonResponse UpdateMaterialReq(MaterialTransferDto materialDto, int voucherId, int pageId);
        CommonResponse DeleteMaterialReq(int TransId, int PageId);
        CommonResponse SizeMasterPopup();
        CommonResponse FindQuantity(int itemId, int locId, int qty, int? transId = 0);
        CommonResponse GetLatestVoucherDate();
    }
}
