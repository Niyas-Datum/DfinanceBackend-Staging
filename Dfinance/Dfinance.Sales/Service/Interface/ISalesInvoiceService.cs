﻿using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using System.Data;

namespace Dfinance.Sales
{
    public interface ISalesInvoiceService
    {
        CommonResponse BatchNoPopup(int locId, int itemId);
        CommonResponse GetDefaultCustomer();
        CommonResponse GetData(int pageId, int voucherId);
        CommonResponse FillTransItems(int partyId, int PageID, int locId, int voucherId);
        CommonResponse FillSales(int PageId, bool? post);
        CommonResponse FillSalesById(int TransId);
        CommonResponse SaveSales(InventoryTransactionDto purchaseDto, int PageId, int voucherId);
        CommonResponse UpdateSales(InventoryTransactionDto purchaseDto, int PageId, int voucherId);
        CommonResponse GetMonthlySalesSummary(DateTime? startDate, DateTime? endDate);
        CommonResponse GetFillSalesDaySummary(string? criteria, DateTime? startDate, DateTime? endDate, int? branch, int? user);

        CommonResponse DeleteSales(int TransId, int pageId);
        CommonResponse CancelSales(int TransId, int pageId, string reason);
        CommonResponse GetSalesPurchaseSummary(DateTime startDate, DateTime endDate, int? branch, int? user);
        CommonResponse AreaWiseSales(string? viewby, DateTime startdate, DateTime enddate, int? item, int? Area);
        CommonResponse SalesReport(string Criteria, DateTime DateFrom, DateTime DateUpto, int? VoucherID, bool? Detailed, int? AccountID, string? VoucherNo, int? SalesManID);


        CommonResponse UserwiseProfit(DateTime startDate, DateTime endDate, int pageId, int? User, bool? detailed);

        CommonResponse SalesCommission(DateTime startdate, DateTime enddate, int? salesmanId, int? userId);
        CommonResponse TopCustomerSupplier(DateTime startdate, DateTime enddate, int? pageId);
        CommonResponse BuyerLoadData();
        CommonResponse BuyerListReport(DateTime DateFrom, DateTime DateUpto, string? Mode, int? PartyId, int? ItemId);
        
        //CommonResponse GetMonthlySalesSummary(DateTime? startDate, DateTime? endDate);  
        //CommonResponse GetFillSalesDaySummary(string? criteria,DateTime startDate, DateTime endDate,int? branch,int? user);

    }
}
