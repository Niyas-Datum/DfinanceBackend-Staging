using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Purchase.Services.Interface
{
    public interface IMaterialReceiptService
    {
        CommonResponse GetData(int pageId, int voucherId);
        CommonResponse FillFromWarehouse(int branchId);
        CommonResponse FillTransItems(int partyId, int PageID, int locId, int voucherId);
        CommonResponse FillBranchAccount();
        CommonResponse FillMaster(int? PageId = 0, int? transId = 0, int? voucherId = 0);
        CommonResponse FillById(int transId, int pageId);
        CommonResponse SaveMatReceipt(MaterialTransferDto materialDto, int voucherId, int pageId);
        CommonResponse UpdateMatReceipt(MaterialTransferDto materialDto, int voucherId, int pageId);
        CommonResponse DeleteMatReceipt(int TransId, int PageId);
        //CommonResponse GetMaterialSettings();
        CommonResponse SizeMasterPopup();
        CommonResponse FindQuantity(int itemId, int locId, int qty, int? transId = 0);
        CommonResponse GetLatestVoucherDate();
        CommonResponse GetMarginPrice(int? itemId, int? accountId, int? voucherId, string? unit);
        CommonResponse FillImportTransactions(int voucherno, DateTime? date = null);
        CommonResponse FillImportItemList(int? transId, int? voucherId);
        CommonResponse FillReference(List<ReferenceDto> referenceDto);
    }
}
