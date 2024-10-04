using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Purchase.Services.Interface
{
    public interface IPurchaseService
    {
        CommonResponse GetData(int pageId,int voucherId);       
        CommonResponse FillTransItems(int PageID, int locId, int voucherId, int? partyId = null);
        CommonResponse FillPurchase(int PageId,int? transactionId = null);
        CommonResponse FillPurchaseById(int TransId,int PageId);
        CommonResponse SavePurchase(InventoryTransactionDto purchaseDto, int PageId,int voucherId);
        CommonResponse UpdatePurchase(InventoryTransactionDto purchaseDto, int PageId,int voucherId);
        CommonResponse DeletePurchase(int TransId,int PageId);
        CommonResponse GetPurchaseReport(PurchaseReportDto reportdto);
        CommonResponse CancelPurchase(int TransId, int pageId, string reason);
        CommonResponse Fill(int transId);
        CommonResponse FillVoucherSettings(object keyword = null, int? voucherId = null);
    }
}
