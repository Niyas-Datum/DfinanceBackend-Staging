using Dfinance.DataModels.Dto.Item;
using Dfinance.DataModels.Dto.Common;
using Dfinance.Shared.Domain;
using System.Data;

namespace Dfinance.Item.Services.Inventory.Interface
{
    public interface IItemMasterService
    {
        CommonResponse FillItemMaster(int[]? catId, int[]? brandId, string search = null, int pageNo = 0, int limit = 0);

        CommonResponse FillItemByID(int pageId, int Id, int BranchId = 0);
        CommonResponse GetNextItemCode();
        CommonResponse ParentItemPopup();
        CommonResponse SaveItemMaster(ItemMasterDto itemDto, int pageId);
        CommonResponse UpdateItemMaster(ItemMasterDto itemDto, int Id, int pageId);
        CommonResponse DeleteItem(int ItemId, int pageId);
        CommonResponse GenerateBarCode();
        CommonResponse TaxDropDown();
        //CommonResponse FillTransItems(int partyId, int PageID, int locId, int voucherId);
        //CommonResponse GetUniqueExpiryItem(int itemId);
        CommonResponse GetItemSearch(int? itemId, string? value, string? criteria);
        CommonResponse GetItemRegister(int? branchId, int? warehouseId, bool less = false, DateTime? date = null);
        CommonResponse GetInventoryAgeing(int? AccountID, DateTime? FromDate, DateTime? ToDate, bool? OpeningBalance, int? VoucherID, string? Nature);
        CommonResponse GetItemExpiryReport(ItemExpiryReportDto itemExpiryReportDto);
        CommonResponse GetInventoryProfitSP(string? ViewBy, DateTime StartDate, DateTime EndDate, int? Customer, bool? Detailed, int Item, string? Salesman, int? AccountId);
        CommonResponse GetItemHistory(string? viewby, DateTime startdate, DateTime enddate, int? warehouse, int? customersupplier, int? item, int? unit, string? barcode, int orgin, int? brand, int? commodity, int? branch, int? vouchertype, string? serialno);

        CommonResponse GetROLReport(int? warehouse, int? type, int? commodity, int? item);
        CommonResponse GetQuotationStatusReport(int? VoucherId, string? VoucherNo);
        CommonResponse GetQuotationComparisonView(DateTime DateFrom, DateTime DateUpto, int BranchID, string? TransactionNo, int? AccountID, int? ItemID, int? VoucherID);
        CommonResponse GetPartialDelivery(DateTime DateFrom, DateTime DateUpto, int branchid, bool Detailed, int? customersupplier, int? item, int? voucher, string? Criteria);
        CommonResponse GetMonthlyInventorySummary(DateTime? startdate, DateTime? enddate, int? accountid, int? voucherid, int? drcr, int? partycategoryid, int? categorytypeid, int? commodity, int? item);
        public CommonResponse FillTransItems(Object PartyId = null, Object PageID = null, Object LocationID = null, Object VoucherID = null,
          Object PrimaryVoucherID = null, Boolean? IsSizeItem = null, Boolean IsMargin = false, Object ItemID = null, Boolean ISTransitLoc = false,
         Boolean IsFinishedGood = false, Boolean IsRawMaterial = false, Object ModeID = null, DateTime? VoucherDate = null,
          Object TransactionID = null, string Criteria = null);
    }
}
