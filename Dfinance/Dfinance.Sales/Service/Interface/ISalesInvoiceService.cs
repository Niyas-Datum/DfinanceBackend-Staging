using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Sales
{
    public interface ISalesInvoiceService
    {
        CommonResponse GetData(int pageId, int voucherId);
        CommonResponse FillTransItems(int partyId, int PageID, int locId, int voucherId);
        CommonResponse FillSales(int PageId, bool? post);
        CommonResponse FillSalesById(int TransId);
        CommonResponse SaveSales(InventoryTransactionDto purchaseDto, int PageId, int voucherId);
        CommonResponse UpdateSales(InventoryTransactionDto purchaseDto, int PageId, int voucherId);
        CommonResponse DeleteSales(int TransId);
        CommonResponse GetMonthlySalesSummary(DateTime? startDate, DateTime? endDate);  
    }
}
