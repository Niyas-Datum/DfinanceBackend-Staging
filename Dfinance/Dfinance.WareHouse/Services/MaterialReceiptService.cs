using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Inventory.Purchase;
using Dfinance.Core.Views.Inventory;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Dfinance.Warehouse.Services.Interface;
using Dfinance.WareHouse.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Transactions;
using Dfinance.DataModels.Dto.Common;

namespace Dfinance.WareHouse.Services
{
    public class MaterialReceiptService:IMaterialReceiptService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<MaterialReceiptService> _logger;
        private readonly IInventoryTransactionService _transactionService;        
        private readonly IInventoryItemService _itemService;
        private readonly IWarehouseService _warehouse;
        private readonly IBranchService _branchService;
        private readonly IItemMasterService _item;
        private readonly DataRederToObj _rederToObj;
        private readonly IInvMatTransService _invMaterial;
        private readonly ISettingsService _settings;
        public MaterialReceiptService(DFCoreContext context, IAuthService authService, ILogger<MaterialReceiptService> logger, IInventoryTransactionService transactionService, 
             IInventoryItemService inventoryItemService, IWarehouseService warehouse, IBranchService branchService, IItemMasterService item, DataRederToObj rederToObj,
             IInvMatTransService invMaterial, ISettingsService settings)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
            _transactionService = transactionService;            
            _itemService = inventoryItemService;
            _warehouse = warehouse;
            _branchService = branchService;
            _item = item;
            _rederToObj = rederToObj;
            _invMaterial = invMaterial;
            _settings = settings;
        }
        private CommonResponse PermissionDenied(string msg)
        {
            _logger.LogInformation("No Permission for " + msg);
            return CommonResponse.Error("No Permission ");
        }
        private CommonResponse PageNotValid(int pageId)
        {
            _logger.LogInformation("Page not Exists :" + pageId);
            return CommonResponse.Error("Page not Exists");
        }
        //getting new VoucherNo,MainBranch Dropdown,ToWarehouse dropdown
        public CommonResponse GetData(int pageId, int voucherId)
        {
            int branchId = _authService.GetBranchId().Value;
            var voucherNo = _transactionService.GetAutoVoucherNo(voucherId).Data;
            var mainBranch = _branchService.GetBranchesDropDown().Data;
            var warehouse = _warehouse.WarehouseDropdownUsingBranch(branchId).Data;
            return CommonResponse.Ok(new { VoucherNo = voucherNo, MainBranch = mainBranch, ToWarehouse = warehouse });
        }
        //when selecting MainBranch, From Warehouse dropdown fills
        public CommonResponse FillFromWarehouse(int branchId)
        {
            var fromWh = _warehouse.WarehouseDropdownUsingBranch(branchId).Data;
            return CommonResponse.Ok(new { FromWarehouse = fromWh });
        }
        //fill items
        public CommonResponse FillTransItems(int partyId, int PageID, int locId, int voucherId)
        {
            var res = _item.FillTransItems(partyId, PageID, locId, voucherId);
            return CommonResponse.Ok(res);
        }
        public CommonResponse FillBranchAccount()
        {
            var result = _context.FiMaAccounts
        .Where(a => a.Active && !a.IsGroup && a.AccountCategoryNavigation.Description == "PROVIDER")
        .Select(a => new
        {
            AccountCode = a.Alias,
            AccountName = a.Name,
            a.Id
        })
        .ToList();
            var result1 = _context.FiMaAccounts.Where(a => a.FimaUniqueAccounts.Any(ua => ua.Keyword == "MATERIAL RECEIVE ACCOUNT"))
           .Select(a => new
           {
               AccountCode = a.Alias,
               AccountName = a.Name,
               a.Id
           })
           .ToList();
            return CommonResponse.Ok(new { BranchAccount = result, Account = result1 });
        }
        public CommonResponse FillMaster(int? PageId = 0, int? transId = 0, int? voucherId = 0)
        {
            try
            {
                int branchid = _authService.GetBranchId().Value;
                string criteria = "";
                List<Fillvoucherview> result = null;
                List<FillVoucherWithTrnasId> result1 = null;
                List<Fillvoucherview> data = null;
                List<FillMaVouchersUsingPageIDView> data1 = null;
                if (transId == null)
                {
                    criteria = "FillVoucher";
                    result = _context.Fillvoucherview.FromSqlRaw($"Exec LeftGridMasterSP @Criteria='{criteria}',@BranchID='{branchid}',@MaPageMenuID={PageId}").ToList();
                }
                else
                {
                    criteria = "FillVoucherByTransactionID";
                    result1 = _context.FillVoucherWithTrnasId.FromSqlRaw($"Exec VoucherSP @Criteria='{criteria}',@BranchID='{branchid}',@TransactionID={transId}").ToList();
                }
                if (PageId == null)
                {
                    criteria = "FillMaVouchers";
                    data = _context.Fillvoucherview.FromSqlRaw($"Exec LeftGridMasterSP @Criteria='{criteria}',@ID='{voucherId}'").ToList();
                }
                else
                {
                    criteria = "FillMaVouchersUsingPageID";
                    data1 = _context.FillMaVouchersUsingPageIDView.FromSqlRaw($"Exec LeftGridMasterSP @Criteria='{criteria}',@ID='{PageId}'").ToList();
                }

                return CommonResponse.Ok(new { r = result, r1 = result1,d=data,d1=data1 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse FillById(int transId, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 3))
            {
                return PermissionDenied("Fill Material Receipt");
            }
            string criteria = "MatReceiptFillById";
            PurchaseFillByIdDto fillMaterialDto = new PurchaseFillByIdDto();
            _context.Database.OpenConnection();
            using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
            {
                dbCommand.CommandText = $"Exec VoucherSP @Criteria='{criteria}',@TransactionID='{transId}'";

                using (var reader = dbCommand.ExecuteReader())
                {
                    fillMaterialDto.fillTransactions = _rederToObj.Deserialize<FillTransactions>(reader).FirstOrDefault();
                    reader.NextResult();
                    fillMaterialDto.fillTransactionEntries = _rederToObj.Deserialize<FillTransactionEntries>(reader).ToList();
                    reader.NextResult();
                    fillMaterialDto.fillInvTransItems = _rederToObj.Deserialize<FillInvTransItems>(reader).ToList();
                    reader.NextResult();
                    fillMaterialDto.fillDocuments = _rederToObj.Deserialize<FillDocuments>(reader).FirstOrDefault();
                    reader.NextResult();
                    fillMaterialDto.fillAdditionals = _rederToObj.Deserialize<FillTransactionAdditional>(reader).FirstOrDefault();
                    reader.NextResult();
                    fillMaterialDto.fillTransactionReferences = _rederToObj.Deserialize<FillTransactionReferences>(reader).FirstOrDefault();
                    reader.NextResult();
                }
            }
            if (fillMaterialDto.fillTransactions != null)
            {
                return CommonResponse.Ok(fillMaterialDto);
            }
            _logger.LogInformation("Material Receipt not found");
            return CommonResponse.NotFound("Material Receipt not found");
        }
        public CommonResponse SaveMatReceipt(MaterialTransferDto materialDto, int voucherId, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 2))
            {
                return PermissionDenied("Save Material Receipt");
            }
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    int transId = (int)_invMaterial.SaveMatTransaction(materialDto, pageId, voucherId).Data;
                    if (materialDto.materialTransAddDto != null)
                    {
                        _invMaterial.SaveMatTransAdditional(materialDto.materialTransAddDto, transId, voucherId);
                    }
                    if (materialDto.Items != null)
                    {
                        _invMaterial.SaveMaterialTransItems(materialDto.Items, voucherId, transId, materialDto.materialTransAddDto.ToWarehouse.Id);
                    }
                    if (materialDto.References.Count > 0 && materialDto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = materialDto.References.Select(x => x.Id).ToList();
                        _invMaterial.SaveMaterialReference(transId, referIds);
                    }
                    if(materialDto.BranchAccount!=null || materialDto.Account!=null)
                    {
                        _invMaterial.SaveMatTransEntries(transId, materialDto.Account.Id, materialDto.BranchAccount.Id, materialDto.Total,voucherId);
                    }
                    transactionScope.Complete();
                    _logger.LogInformation("Material Receipt Saved Successfully");
                    return CommonResponse.Ok("Material Receipt Saved Successfully");
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    _logger.LogError("Material Receipt Not Saved");
                    return CommonResponse.Error("Material Receipt Not Saved");
                }
            }
        }
        public CommonResponse UpdateMatReceipt(MaterialTransferDto materialDto, int voucherId, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 2))
            {
                return PermissionDenied("Update Material Receipt");
            }
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    int transId = (int)_invMaterial.SaveMatTransaction(materialDto, pageId, voucherId).Data;
                    if (materialDto.materialTransAddDto != null)
                    {
                        _invMaterial.SaveMatTransAdditional(materialDto.materialTransAddDto, transId, voucherId);
                    }
                    if (materialDto.Items != null)
                    {
                        _invMaterial.UpdateMaterialTransItems(materialDto.Items, voucherId, transId, materialDto.materialTransAddDto.ToWarehouse.Id);
                    }
                    if (materialDto.BranchAccount != null || materialDto.Account != null)
                    {
                        _invMaterial.UpdateMatTransEntries(transId, materialDto.Account.Id, materialDto.BranchAccount.Id, materialDto.Total, voucherId);
                    }
                    if (materialDto.References.Count > 0 && materialDto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = materialDto.References.Select(x => x.Id).ToList();
                        _invMaterial.UpdateMaterialReference(transId, referIds);
                    }
                    transactionScope.Complete();
                    _logger.LogInformation("Material Receipt Updated Successfully");
                    return CommonResponse.Ok("Material Receipt Updated Successfully");
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    _logger.LogError("Material Receipt Not Updated");
                    return CommonResponse.Error("Material Receipt Not Updated");
                }
            }
        }
        public CommonResponse DeleteMatReceipt(int TransId, int PageId)
        {
            try
            {
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 5))
                {
                    return PermissionDenied("Delete Material Receipt");
                }
                var result = _transactionService.DeletePurchase(TransId);
                _logger.LogInformation("Successfully Deleted Material Receipt");
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to Delete Material Receipt");
                return CommonResponse.Error(ex);
            }
        }
        //get all settings value of material request
        //public CommonResponse GetMaterialSettings()
        //{
        //    string[] keys = { "Size Sales", "HideRateInStock", "AllowMinusStock", "HideRateInStock", "MinusStockWarning", "ROLSystem", "Voucher Auto Date", "Load PreVoucher Info" };
        //    var settings = _settings.GetAllSettings().Data;
        //    return CommonResponse.Ok(settings);
        //}
        //fill sizemastername popup in item grid        
        //if size sales settings is true only       
        public CommonResponse SizeMasterPopup()
        {
            var result = _context.InvSizeMaster.Where(x => x.Active == true).Select(x => new
            {
                x.Code,
                SizeMasterName = x.Name,
                x.Id
            }).ToList();
            return CommonResponse.Ok(result);
        }
        //find current stock of item
        //find current stock of item
        public CommonResponse FindQuantity(int itemId, int locId, int qty, int? transId = 0)
        {
            var allowMinusStock = (bool)_settings.GetSettings("AllowMinusStock").Data;
            var minusStockWarning = (bool)_settings.GetSettings("MinusStockWarning").Data;
            var rolSet = (bool)_settings.GetSettings("ROLSystem").Data;
            int branchId = _authService.GetBranchId().Value;
            var DbQty = _context.CurrentStockView.FromSqlRaw($"select dbo.StockQtyOnDateNotInTransaction('{itemId}','{branchId}',null,'{locId}','{transId}')").AsEnumerable().FirstOrDefault();
            
            
            if(DbQty.Stock<1 || qty>DbQty.Stock)
            {
                if (allowMinusStock == true && minusStockWarning == true)
                    return CommonResponse.Error("This Item Out of Stock");
            }
            if(rolSet)
            {
                if (qty == 0)
                    return CommonResponse.Error("Quantity must be greater than zero!!");
                if (transId == 0)
                    transId = -1;                
                var rol = _context.ItemMaster.Where(i => i.Id == itemId).Select(i => i.ROL ?? 0).FirstOrDefault();
                if (rol != 0)
                {
                    if ((DbQty.Stock - qty) < rol)
                    {
                        return CommonResponse.Error("Stock gone under ROL,Current stock=" + DbQty.Stock);
                    }
                }
            }           
            return CommonResponse.Ok(DbQty.Stock);
        }
        //if Voucher Auto Date is true
        public CommonResponse GetLatestVoucherDate()
        {
            int branchId = _authService.GetBranchId().Value;
            int userId = _authService.GetId().Value;
            var currentDate = DateTime.Now; // default value if no record found

            var latestTransactionDate = _context.FiTransaction.Where(T => T.CompanyId == branchId && T.AddedBy == userId)
                .OrderByDescending(T => T.Date).Select(T => T.Date).FirstOrDefault();

            if (latestTransactionDate != null)
            {
                currentDate = latestTransactionDate;
            }
            return CommonResponse.Ok(currentDate);
        }
        public CommonResponse GetMarginPrice(int? itemId,int? accountId,int? voucherId,string? unit)
        {
            int branchId = _authService.GetBranchId().Value;
            var margin = _context.CurrentStockView.FromSqlRaw($"select dbo.GetMarginPrice('{branchId}','{itemId}','{accountId}','{voucherId}','{unit}')").AsEnumerable().FirstOrDefault();
            return CommonResponse.Ok(new { marginPrice = margin });
        }
        //fill the imported transactions in a window
        public CommonResponse FillImportTransactions(int voucherno, DateTime? date = null)
        {
            try
            {
                int BranchID = _authService.GetBranchId().Value;
                int? OtherBranchID = null;
                string criteria = "FillImportTransactions";

                // Fetching the setting value for ReferenceImportItemTracking from the database
                string referenceImportItemTracking = _context.MaSettings
                    .Where(setting => setting.Key == "ReferenceImportItemTracking")
                    .Select(setting => setting.Value)
                    .FirstOrDefault();

                bool isReferenceImportItemTrackingTrue = !string.IsNullOrEmpty(referenceImportItemTracking) &&
                                                         (referenceImportItemTracking == "True" || referenceImportItemTracking == "1");
                var data = _context.ReferenceView
                    .FromSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0}, @VoucherID={1}, @BranchID={2}, @Date={3}, @OtherBranchID={4}",
                        criteria, voucherno, BranchID, date ?? null, OtherBranchID ?? null)
                    .ToList();


                if (!isReferenceImportItemTrackingTrue)
                {
                    //data = data.Where(item => item.PartyInvNo - item.PartyInvNo > 0).ToList();
                }

                return CommonResponse.Created(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error("An error occurred while fetching references.");
            }
        }
        //selecting a reference, list the corresponding items
        public CommonResponse FillImportItemList(int? transId, int? voucherId)
        {
            string criteria = "FillImportItemListWeb";
            var data = _context.ImportItemListView.FromSqlRaw("Exec VoucherAdditionalsSP @Criteria={0},@VoucherID={1},@TransactionID={2}",
                criteria, voucherId, transId).ToList();
            return CommonResponse.Ok(data);
        }
        public CommonResponse FillReference(List<ReferenceDto> referenceDto)
        {
            int? transId = 0;
            string criteria = "FillImportItemsWeb";
            string itemIds = null;
            List<int?>? itemId = null;
            List<RefItemsView>? refItems = null;
            foreach (var refer in referenceDto)
            {
                if (refer.Sel == true)
                {
                    transId = refer.Id;
                    // importItems = (List<ImportItemListDto>?)FillImportItemList(transId, refer.VoucherId).Data;
                    if (refer.AddItem == true)
                    {
                        itemId = refer.RefItems.Select(r => r.ItemID).ToList();
                    }
                    else
                    {
                        itemId = refer.RefItems.Where(i => i.Select == true).Select(r => r.ItemID).ToList();
                    }
                    if (itemId.Count > 0)
                        itemIds = string.Join(",", itemId.ToArray());
                    refItems = _context.RefItemsView.FromSqlRaw("exec VoucherAdditionalsSP @Criteria={0},@TransactionID={1},@ItemIdString={2}",
                        criteria, transId, itemIds).ToList();
                }
            }
            return CommonResponse.Ok(refItems);

        }
    }
}
