using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Purchase.Services.Interface
{
    public interface IPurchaseService
    {
        CommonResponse GetData(int pageId,int voucherId);       
        CommonResponse FillTransItems(int partyId, int PageID, int locId,int voucherId);
        CommonResponse FillPurchase(int PageId, bool? post);
        CommonResponse FillPurchaseById(int TransId,int PageId);
        CommonResponse SavePurchase(InventoryTransactionDto purchaseDto, int PageId,int voucherId);
        CommonResponse UpdatePurchase(InventoryTransactionDto purchaseDto, int PageId,int voucherId);
        CommonResponse DeletePurchase(int TransId,int PageId);
        CommonResponse GetPurchaseReport(PurchaseReportDto reportdto);
    }
}
