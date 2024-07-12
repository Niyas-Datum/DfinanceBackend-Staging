using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Inventory;
using Dfinance.Core.Views.Inventory.Purchase;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Purchase.Services;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Dfinance.Warehouse.Services.Interface;
using Dfinance.WareHouse.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Dfinance.WareHouse.Services
{
    public class MaterialRequestService : IMaterialRequestService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<MaterialRequestService> _logger;
        private readonly IInventoryTransactionService _transactionService;
        private readonly IInventoryItemService _itemService;
        private readonly IWarehouseService _warehouse;
        private readonly IBranchService _branchService;
        private readonly IItemMasterService _item;
        private readonly DataRederToObj _rederToObj;
        private readonly IInvMatTransService _invMaterial;
        private readonly ISettingsService _settings;
        private readonly IConfiguration _configuration;
        public MaterialRequestService(DFCoreContext context, IAuthService authService, ILogger<MaterialRequestService> logger, IInventoryTransactionService transactionService,
            IInventoryItemService inventoryItemService, IWarehouseService warehouse, IBranchService branchService, IItemMasterService item, DataRederToObj rederToObj,
            IInvMatTransService invMaterial, ISettingsService settings, IConfiguration configuration)
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
            _configuration = configuration;
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
        public CommonResponse FillMaster(int? PageId = 0,int? transId=0,int? voucherId=0)
        {
            try
            {
                int branchid = _authService.GetBranchId().Value;
                string criteria = "";
                List<Fillvoucherview> result=null;
                List<FillVoucherWithTrnasId> result1=null;
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

                return CommonResponse.Ok(new { res = result, res1 = result1,dat=data,dat1=data1 });
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
                return PermissionDenied("Fill Material Request");
            }
            string criteria = "MatReqFillById";
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
                   
                }
            }
            if (fillMaterialDto.fillTransactions != null)
            {
                return CommonResponse.Ok(fillMaterialDto);
            }
            _logger.LogInformation("Material Request not found");
            return CommonResponse.NotFound("Material Request not found");
        }

       
        public CommonResponse SaveMaterialReq(MaterialTransferDto materialDto, int voucherId, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 2))
            {
                return PermissionDenied("Save Material Request");
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
                        _itemService.SaveInvTransItems(materialDto.Items, voucherId, transId, null, null);
                    }
                    transactionScope.Complete();
                    _logger.LogInformation("Material Request Saved Successfully");
                    return CommonResponse.Ok("Material Request Saved Successfully");
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    _logger.LogError("Material Request Not Saved");
                    return CommonResponse.Error("Material Request Not Saved");
                }
            }
        }
        public CommonResponse UpdateMaterialReq(MaterialTransferDto materialDto, int voucherId, int pageId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 3))
            {
                return PermissionDenied("Update Matrial Request");
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
                    transactionScope.Complete();
                    _logger.LogInformation("Material Request Updated Successfully");
                    return CommonResponse.Ok("Material Request Updated Successfully");
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    _logger.LogError("Material Request Not Updated");
                    return CommonResponse.Error("Material Request Not Updated");
                }
            }
        }
        public CommonResponse DeleteMaterialReq(int TransId, int PageId)
        {
            try
            {
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 5))
                {
                    return PermissionDenied("Delete Material Request");
                }
                var result = _transactionService.DeleteTransactions(TransId);
                _logger.LogInformation("Successfully Deleted Material Request");
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to Delete Material Request");
                return CommonResponse.Error(ex);
            }
        }

        //get all settings value of material request
        //public CommonResponse GetMaterialSettings()
        //{
        //    string[] keys = { "Size Sales", "HideRateInStock", "AllowMinusStock", "HideRateInStock", "MinusStockWarning", "ROLSystem", "Voucher Auto Date" };
        //    var settings = _settings.GetAllSettings(keys).Data;
        //    return CommonResponse.Ok(settings);
        //}
        //public CommonResponse GetMaterialSettings()
        //{
            
        //    var settings = _settings.GetAllSettings().Data;
        //    return CommonResponse.Ok(settings);
        //}
        //fill sizemastername popup in item grid        
        //if size sales settings is true only       
        public CommonResponse SizeMasterPopup()
        {            
            var sizeSettings = (bool)_settings.GetSettings("Size Sales").Data;
            if(sizeSettings)
            {
                var result = _context.InvSizeMaster.Where(x => x.Active == true).Select(x => new
                {
                    x.Code,
                    SizeMasterName = x.Name,
                    x.Id
                }).ToList();
                return CommonResponse.Ok(result);
            }
            return CommonResponse.Ok(null);
        }
        //find current stock of item
        public CommonResponse FindQuantity(int itemId, int locId, int qty, int? transId = 0)
        {
            var allowMinusStock = (bool)_settings.GetSettings("AllowMinusStock").Data;
            var minusStockWarning = (bool)_settings.GetSettings("MinusStockWarning").Data;
            var rolSet = (bool)_settings.GetSettings("ROLSystem").Data;
            int branchId = _authService.GetBranchId().Value;
            var DbQty = _context.CurrentStockView.FromSqlRaw($"select dbo.StockQtyOnDateNotInTransaction('{itemId}','{branchId}',null,'{locId}','{transId}')").AsEnumerable().FirstOrDefault();


            if (DbQty.Stock < 1 || qty > DbQty.Stock)
            {
                if (allowMinusStock == true && minusStockWarning == true)
                    return CommonResponse.Error("This Item Out of Stock");
            }
            if (rolSet)
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
            var currentDate = DateTime.Now; 

            var latestTransactionDate = _context.FiTransaction.Where(T => T.CompanyId == branchId && T.AddedBy == userId)
                .OrderByDescending(T => T.Date).Select(T => T.Date).FirstOrDefault();

            if (latestTransactionDate != null)
            {
                currentDate = latestTransactionDate;
            }
            return CommonResponse.Ok(currentDate);
        }
    }
}
