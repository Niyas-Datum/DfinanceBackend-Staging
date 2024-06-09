using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;

namespace Dfinance.Purchase.Reports.Interface
{
    public interface IPurchaseReportService
    {
        CommonResponse FillPurchaseReport(PurchaseReportDto reportdto);
    }
}
