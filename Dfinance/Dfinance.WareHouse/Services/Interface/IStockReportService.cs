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
        CommonResponse GetItemDetailsLoadData();
        CommonResponse FillStockItemDetails(ItemDetailsRpt itemDetails);
        CommonResponse GetWarehouseStockLoadData();
        CommonResponse FillWarehouseStockDetails(WarehouseStockRegRpt warehouseStock);
        CommonResponse GetCommudityLoadData();
        CommonResponse FillStockRegisterCommoditywise(CommodityStockRegRpt commodityStockReg);
        CommonResponse GetStockReceiptLoadData();
        CommonResponse FillStockReceiptIssue(StockReceiptIssueRpt stockReceiptIssue);
        CommonResponse GetUnitwiseStockLoadData();
        CommonResponse FillUnitwiseStock(UnitwiseStock unitwiseStock);
    }
}
