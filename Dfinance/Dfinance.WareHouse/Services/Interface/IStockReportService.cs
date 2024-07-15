using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Domain;

namespace Dfinance.Warehouse.Services.Interface
{
    public interface IStockReportService
    {
        CommonResponse GetLoadData();
        CommonResponse FillItemReports(StockRegistration stockRegistration);
        CommonResponse GetItemStockLoadData();
        CommonResponse FillStockItemRegister(ItemStockRegisterRpt itemStockRegister);
    }
}
