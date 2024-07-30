using Dfinance.DataModels.Dto;
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
        CommonResponse GetItemwiseStockLoadData();
        CommonResponse FillItemwiseStock(ItemsCatalogue itemsCatalogue);
        CommonResponse GetMonthwiseStockLoadData();
        CommonResponse FillMonthwiseStock(MonthwiseStockRpt monthwiseStk);
        CommonResponse GetWarehousewiseStockLoadData();
        CommonResponse FillWarehousewiseStock(string item, int branchId);
        CommonResponse GetBatchwiseStockLoadData();
        CommonResponse FillBatchwiseStock(BatchwiseStockRpt batchwiseStock);
        CommonResponse GetItemwiseRegStockLoadData();
        CommonResponse FillItemwiseRegStock(ItemwiseRegRpt itemwiseReg);
        CommonResponse FillVoucherLocations(int branchId);
        CommonResponse FillItemStockRpt(ItemStockRpt itemStock);
        CommonResponse GetItemStockRptLoadData();
        CommonResponse FillItemMovementAnalysis(ItemMovementAnalysis itemMovement);
        CommonResponse GetItemMovementAnalysisLoadData();

    }
}
