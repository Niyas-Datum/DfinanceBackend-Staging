using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Item;
using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Dfinance.WareHouse.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;
using System.Transactions;
using static Dfinance.Shared.Routes.InvRoute;

namespace Dfinance.Stock.Services
{
    public class StockTransactionService : IStockTransactionService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<StockTransactionService> _logger;
        private readonly IInventoryTransactionService _transactionService;
        private readonly IInventoryAdditional _additionalService;
        private readonly IInventoryItemService _itemService;
        private readonly IUserTrackService _userTrackService;
        private readonly IItemMasterService _item;
        public StockTransactionService(DFCoreContext context, IAuthService authService, ILogger<StockTransactionService> logger, IInventoryTransactionService transactionService,
           IInventoryAdditional additionalService, IInventoryItemService itemService, IUserTrackService userTrackService, IItemMasterService item)
        {
            _additionalService = additionalService;
            _context = context;
            _authService = authService;
            _logger = logger;
            _transactionService = transactionService;
            _itemService = itemService;
            _userTrackService = userTrackService;
            _item = item;
        }
        //fills the ToWarehouse dropdown for Inventory Write off(Damage)        
        public CommonResponse FillDamageWH()
        {
            var damage = _context.Locations.Where(l => l.DevCode == 4).Select(l => new { l.Id, l.Name }).ToList();
            return CommonResponse.Ok(damage);
        }
        public CommonResponse FillStockItems(int voucherId, DateTime VoucherDate, Object LocationID = null)
        {
            Object PrimaryVoucherID = _transactionService.GetPrimaryVoucherID(voucherId);
            var StockItems = _item.FillTransItems(null, null, LocationID, null, PrimaryVoucherID, null, false, null, false, false, false, null, VoucherDate);
            return CommonResponse.Ok(StockItems);
        }
        public CommonResponse FillUniqueItems(int itemId)
        {

            var UniqueItems = _context.CommandTextView
                 .FromSqlRaw($"select dbo.GetCommandText('GetUniqueNo',null,null,null,null,null,'False',null,{itemId},'False',null,null,null,null,null,null,118)")
                .ToList();
            var res = UniqueItems.FirstOrDefault();
            var data = _context.UniqueItemView.FromSqlRaw(res.commandText).ToList();
            return CommonResponse.Ok(data);
        }
        public CommonResponse SaveStockTrans(StockTransactionDto stockTransDto, int voucherId, int pageId)
        {
            var pageName = _context.MaPageMenus.Where(p => p.Id == pageId).Select(p => p.MenuText).FirstOrDefault();
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 2))
            {
                return PermissionDenied("Create " + pageName);
            }
            int primVoucherId = _transactionService.GetPrimaryVoucherID(voucherId);
            if ((VoucherType)primVoucherId == VoucherType.Stock_Transfer || (VoucherType)primVoucherId == VoucherType.Stock_Issue || (VoucherType)primVoucherId == VoucherType.Stock_Receipt || (VoucherType)primVoucherId == VoucherType.Stock_Request)
            {
                using (var transactionScope = new TransactionScope())
                {
                    try
                    {
                        int transId = (int)SaveTransaction(stockTransDto, voucherId, pageId).Data;
                        SaveTransAdditionals(transId, voucherId, stockTransDto);
                        if (stockTransDto.StockItems.Count > 0)
                        {
                            SaveStockTransItems(stockTransDto, voucherId, transId);
                        }
                        if (stockTransDto.references.Count > 0 && stockTransDto.references.Any(x=>x.Sel==true) )
                        {
                            List<int?> referIds = stockTransDto.references.Select(x => x.Id).ToList();
                            _transactionService.SaveTransReference(transId, referIds);
                        }
                        var jsonData = JsonSerializer.Serialize(stockTransDto);
                        _userTrackService.AddUserActivity(stockTransDto.VoucherNo, transId, 0, "Added", "FiTransactions", pageName, 0, jsonData);
                        transactionScope.Complete();
                        return CommonResponse.Ok(pageName + " Saved Successfully");
                    }
                    catch
                    {
                        transactionScope.Dispose();
                        _logger.LogError("Failed to Save " + pageName);
                        return CommonResponse.Error("Failed to Save " + pageName);
                    }
                }
            }
            else
                return CommonResponse.Ok("Invalid voucher");
        }
        public CommonResponse UpdateStockTrans(StockTransactionDto stockTransDto, int voucherId, int pageId)
        {
            var pageName = _context.MaPageMenus.Where(p => p.Id == pageId).Select(p => p.MenuText).FirstOrDefault();
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 3))
            {
                return PermissionDenied("Update " + pageName);
            }
            int primVoucherId = _transactionService.GetPrimaryVoucherID(voucherId);
            if ((VoucherType)primVoucherId == VoucherType.Stock_Transfer || (VoucherType)primVoucherId == VoucherType.Stock_Issue || (VoucherType)primVoucherId == VoucherType.Stock_Receipt || (VoucherType)primVoucherId == VoucherType.Stock_Request)
            {
                using (var transactionScope = new TransactionScope())
                {
                    try
                    {
                        int transId = (int)SaveTransaction(stockTransDto, voucherId, pageId).Data;
                        SaveTransAdditionals(transId, voucherId, stockTransDto);
                        if (stockTransDto.StockItems.Count > 0)
                        {
                            UpdateStockTransItems(stockTransDto, voucherId, transId);
                        }
                        if (stockTransDto.references.Count > 0 && stockTransDto.references.Any(x => x.Sel == true))
                        {
                            List<int?> referIds = stockTransDto.references.Select(x => x.Id).ToList();
                            _transactionService.UpdateTransReference(transId, referIds);
                        }
                        var jsonData = JsonSerializer.Serialize(stockTransDto);
                        _userTrackService.AddUserActivity(stockTransDto.VoucherNo, transId, 1, "Updated", "FiTransactions", pageName, 0, jsonData);

                        transactionScope.Complete();
                        return CommonResponse.Ok(pageName + " Updated Successfully");
                    }
                    catch
                    {
                        transactionScope.Dispose();
                        _logger.LogError("Failed to Update " + pageName);
                        return CommonResponse.Error("Failed to Update " + pageName);
                    }
                }
            }
            else
                return CommonResponse.Ok("Invalid voucher");
        }
        private CommonResponse SaveTransaction(StockTransactionDto stockTransDto, int voucherId, int pageId)
        {

            string criteria;
            int? transId = null;
            var machineName = Environment.MachineName;
            bool IsPostDated = false;
            var companyId = _authService.GetBranchId().Value;
            var addedBy = _authService.GetId().Value;
            var status = "Approved";
            var addedDate = DateTime.Now;
            bool isAutoEntry = false;
            bool posted = true;
            bool active = true;
            bool cancelled = false;
            decimal ExchangeRate = 1;
            char ApprovalStatus = 'A';
            try
            {
                if (stockTransDto.Id == null || stockTransDto.Id == 0)
                {
                    criteria = "InsertTransactions";
                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    _context.Database.ExecuteSqlRaw("EXEC VoucherSP " +
                            "@Criteria={0}, @Date={1}, @EffectiveDate={2}, @VoucherID={3}, @MachineName={4}, " +
                            "@TransactionNo={5}, @IsPostDated={6}, @CurrencyID={7}, @ExchangeRate={8}, @RefPageTypeID={9}, " +
                            "@RefPageTableID={10}, @ReferenceNo={11}, @CompanyID={12}, @FinYearID={13}, @InstrumentType={14}, " +
                            "@InstrumentNo={15}, @InstrumentDate={16}, @InstrumentBank={17}, @CommonNarration={18}, @AddedBy={19}, " +
                            "@ApprovedBy={20}, @AddedDate={21}, @ApprovedDate={22}, @ApprovalStatus={23}, " +
                            "@ApproveNote={24}, @Action={25}, @Status={26}, @IsAutoEntry={27}, " +
                            "@Posted={28}, @Active={29}, @Cancelled={30}, @AccountID={31}, @Description={32}, " +
                            "@RefTransID={33}, @CostCentreID={34}, @PageID={35}, @NewID={36} OUTPUT",
                            criteria, stockTransDto.VoucherDate, DateTime.Now, voucherId, machineName, stockTransDto.VoucherNo,
                            IsPostDated, null, ExchangeRate, null, null,
                            stockTransDto.Reference, companyId, null, null, null, null, null, null, addedBy, null,
                            addedDate, null, ApprovalStatus, null, null, status, isAutoEntry, posted, active, cancelled, null,
                            stockTransDto.Description, null, null, pageId, newId);
                    transId = (int)newId.Value;
                }
                else
                {
                    int Transaction = _context.FiTransaction.Where(x => x.Id == stockTransDto.Id).Select(x => x.Id).FirstOrDefault();
                    if (Transaction == null)
                    {
                        return CommonResponse.NotFound();
                    }
                    criteria = "UpdateTransactions";
                    transId = stockTransDto.Id;
                    _context.Database.ExecuteSqlRaw("EXEC VoucherSP " +
                                "@Criteria={0}, @Date={1}, @EffectiveDate={2}, @VoucherID={3}, @MachineName={4}, " +
                                "@TransactionNo={5}, @IsPostDated={6}, @CurrencyID={7}, @ExchangeRate={8}, @RefPageTypeID={9}, " +
                                "@RefPageTableID={10}, @ReferenceNo={11}, @CompanyID={12}, @FinYearID={13}, @InstrumentType={14}, " +
                                "@InstrumentNo={15}, @InstrumentDate={16}, @InstrumentBank={17}, @CommonNarration={18}, @AddedBy={19}, " +
                                "@ApprovedBy={20}, @AddedDate={21}, @ApprovedDate={22}, @ApprovalStatus={23}, " +
                                "@ApproveNote={24}, @Action={25}, @Status={26}, @IsAutoEntry={27}, " +
                                "@Posted={28}, @Active={29}, @Cancelled={30}, @AccountID={31}, @Description={32}, " +
                                "@RefTransID={33}, @CostCentreID={34}, @PageID={35}, @ID={36}",
                                 criteria, stockTransDto.VoucherDate, DateTime.Now, voucherId, machineName, stockTransDto.VoucherNo,
                                IsPostDated, null, ExchangeRate, null, null,
                                stockTransDto.Reference, companyId, null, null, null, null, null, null, addedBy, null,
                                addedDate, null, DBApprovalStatus.APPROVED, null, null, status, isAutoEntry, posted, active, cancelled, null,
                                stockTransDto.Description, null, null, pageId, Transaction);
                }
            }
            catch (Exception ex)
            {

            }
            return CommonResponse.Ok(transId);
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
       
        private CommonResponse SaveTransAdditionals(int transId, int voucherId, StockTransactionDto stockTransDto)
        {
            try
            {
                int? outLocId = null, inLocId=null,otherBranchId = null;
                int primVoucherId = _transactionService.GetPrimaryVoucherID(voucherId);
                switch ((VoucherType)primVoucherId)
                {
                   case VoucherType.Stock_Issue:
                        outLocId = stockTransDto.FromWarehouse.Id;
                        otherBranchId = stockTransDto.ToBranch.Id;
                        break;
                    case VoucherType.Stock_Request:
                        otherBranchId = stockTransDto.ToBranch.Id;
                        break;
                    case VoucherType.Stock_Receipt:
                        inLocId = stockTransDto.ToWarehouse.Id;
                        otherBranchId = stockTransDto.FromBranch.Id;
                        break;
                }
                string criteria;
                var additionalId = _context.FiTransactionAdditionals.Any(x => x.TransactionId == transId);
                if (!additionalId)
                    criteria = "InsertFiTransactionAdditionals";
                else
                    criteria = "UpdateFiTransactionAdditionals";

                _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0},@TransactionID={1},@FromLocationID={2},@ToLocationID={3},@Terms={4},@BranchID={5},@InLocID={6}, @OutLocID={7}",
                    criteria,//0
                    transId,//1                    
                    stockTransDto.FromWarehouse.Id,//9
                    stockTransDto.ToWarehouse.Id,//10                   
                    stockTransDto.Terms,otherBranchId,
                    inLocId,outLocId
                    );
                return CommonResponse.Ok();
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        private CommonResponse SaveStockTransItems(StockTransactionDto stockTransDto, int voucherId, int transId)
        {
            try
            {
                int slNo = 1;
                int refTransId1 = 0;
                string criteria = "InsertInvTransItems";
                decimal factor = 1;
                string tranType = "Normal";
                decimal tempRate = 0;
                int? rowType = null;
                bool visible = true;
                int refTransItemId = 0;
                int? outLocId = null;
                int? inLocId = null;
                decimal? FOCQty = null;

                int InTransitLocation = _context.Locations.Where(l => l.DevCode == 3).Select(l => l.Id).SingleOrDefault();
                int primVoucherId = _transactionService.GetPrimaryVoucherID(voucherId);
                switch ((VoucherType)primVoucherId)
                {
                    case VoucherType.Stock_Issue:
                        rowType = -1;
                        outLocId = stockTransDto.FromWarehouse.Id;
                        inLocId = InTransitLocation;
                        break;
                    case VoucherType.Stock_Transfer:
                        inLocId = stockTransDto.ToWarehouse.Id;
                        outLocId = stockTransDto.FromWarehouse.Id;
                        break;
                    case VoucherType.Stock_Receipt:
                        inLocId = stockTransDto.ToWarehouse.Id;
                        outLocId = InTransitLocation;
                        break;

                }
                if (stockTransDto.StockItems != null && stockTransDto.StockItems.Count > 0)
                {
                    foreach (var item in stockTransDto.StockItems)
                    {
                        factor = _context.ItemUnits.Where(x => x.ItemId == item.ItemId && x.Unit == item.Unit.Unit).Select(x => x.Factor).SingleOrDefault();
                        var importItems = _context.InvTransItems.Where(i => i.TransactionId == item.TransactionId).Select(i => i.ItemId).ToList();
                        foreach (var i in importItems)
                        {
                            if (i == item.ItemId)
                                refTransItemId = _context.InvTransItems.Where(t => t.TransactionId == item.TransactionId && t.ItemId == item.ItemId)
                               .Select(t => t.Id).SingleOrDefault();
                        }
                        SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        var data = _context.Database.ExecuteSqlRaw("Exec VoucherAdditionalsSP @Criteria={0}, @TransactionID={1}, @ItemID={2}, @SerialNo={3}," +
                                    "@Unit={4}, @Qty={5}, @FOCQty={6}, @BasicQty={7}, @Rate={8}, @RowType={9}, " +
                                    "@Factor={10}, @RefTransItemID={11},@StockQty={12}, @InlocID={13}, @OutLocID={14}, @SizeMasterID={15}, " +
                                    "@TaxTypeID={16}, @TaxAccountID={17}, @TranType={18},@TaxPerc={19},@TaxValue={20},@NewID={21} OUTPUT",
                                    criteria,
                                    transId,
                                   item.ItemId == 0 ? null : item.ItemId,
                                    slNo,
                                    item.Unit.Unit,
                                    item.Qty,
                                    FOCQty,
                                    item.BasicQty,
                                    item.Rate,
                                    rowType == 0 ? null : rowType,
                                    factor,
                                    refTransItemId == 0 ? null : refTransItemId,
                                    item.StockQty,
                                    inLocId,
                                    outLocId,
                                    item.SizeMaster.Id == 0 ? null : item.SizeMaster.Id,
                                    item.TaxTypeId == 0 ? null : item.TaxTypeId,
                                    item.TaxAccountId == 0 ? null : item.TaxAccountId,
                                    tranType,
                                    item.TaxPerc,
                                    item.TaxValue,
                                    newId
                        );
                        var transItemId = (int)newId.Value;

                        //check whether the IsUniqueItem field of this item is true or false
                        //for stock transfer and inventory write off
                        if ((VoucherType)primVoucherId == VoucherType.Stock_Transfer)
                        {
                            var uniqueItem = _context.ItemMaster.Where(i => i.Id == item.ItemId).Select(i => i.IsUniqueItem).SingleOrDefault();
                            if (uniqueItem == true)
                                if (item.Qty > 0 && item.UniqueItems.Count > 0)
                                {
                                    SaveUniqueItems(transId, transItemId, item.ItemId, item.UniqueItems);
                                }
                        }
                    }
                    slNo++;
                }
                return CommonResponse.Ok("Inserted successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse UpdateStockTransItems(StockTransactionDto stockTransDto, int voucherId, int transId)
        {
            try
            {
                var uniqueRemove = _context.InvUniqueItems.Where(u => u.TransactionId == transId).ToList();
                if (uniqueRemove.Count > 0)
                {
                    _context.InvUniqueItems.RemoveRange(uniqueRemove);
                    _context.SaveChanges();
                }
                var itemsRemove = _context.InvTransItems.Where(i => i.TransactionId == transId).ToList();
                if (itemsRemove.Count > 0)
                {
                    _context.InvTransItems.RemoveRange(itemsRemove);
                    _context.SaveChanges();
                    SaveStockTransItems(stockTransDto, voucherId, transId);
                }

                return CommonResponse.Ok("TransItems Updated Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }




        /********************************************************InvUniqueItems******************************************************************************/
        private CommonResponse SaveUniqueItems(int transId, int transItemId, int itemId, List<InvUniqueItemDto> uniqueItems)
        {
            try
            {
                string criteria = "InsertInvUniqueItems";
                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                foreach (var u in uniqueItems)
                {
                    var data = _context.Database.ExecuteSqlRaw("Exec VoucherAdditionalsSP @Criteria={0},@TransactionID={1},@TransItemID={2}," +
                       "@ItemID={3},@UniqueNo={4},@NewID={5} OUTPUT",
                       criteria, transId, transItemId, itemId, u, newId);
                    var uniqueItemId = (int)newId.Value;
                }
                return CommonResponse.Ok("Inserted successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        public CommonResponse UpdateInvTransItems(StockTransactionDto transDto, int voucherId, int pageId)
        {
            try
            {
                var uniqueRemove = _context.InvUniqueItems.Where(u => u.TransactionId == transDto.Id).ToList();
                if (uniqueRemove.Count > 0)
                {
                    _context.InvUniqueItems.RemoveRange(uniqueRemove);
                    _context.SaveChanges();
                }
                var itemsRemove = _context.InvTransItems.Where(i => i.TransactionId == transDto.Id).ToList();
                if (itemsRemove.Count > 0)
                {
                    _context.InvTransItems.RemoveRange(itemsRemove);
                    _context.SaveChanges();
                    SaveStockTransItems(transDto, voucherId, pageId);
                }

                return CommonResponse.Ok("TransItems Updated Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

    }
}
