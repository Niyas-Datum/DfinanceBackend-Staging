using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace Dfinance.Inventory.Service
{

    public class InventoryItemservice : IInventoryItemService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        private readonly IInventoryTransactionService _inventoryTransactionService;
        private readonly IItemMasterService _itemMaster;
        public InventoryItemservice(DFCoreContext context, IAuthService authService, IHostEnvironment hostEnvironment, IItemMasterService itemMaster, IInventoryTransactionService inventoryTransaction)
        {
            _context = context;
            _authService = authService;
            _environment = hostEnvironment;
            _itemMaster = itemMaster;
            _inventoryTransactionService = inventoryTransaction;
        }

        //Get the IsUnique, IsExpiry values of item from InvItemMaster
        //get the units of item
        //get next batch number
        //get the price category pop up of the item grid
        //get the item transaction details for tool tip in item grid
        public CommonResponse GetItemData(int itemId, int partyId, int voucherId)
        {
            try
            {
                //var unitData = _itemUnits.GetItemUnits(itemId);
                // var result = _itemMaster.GetUniqueExpiryItem(itemId);
                var batch = NextBatchNo();
                //string criteria = "GetLastItemRate";
                int branch = _authService.GetBranchId().Value;
                var PriceCat = PriceCategoryPopup(itemId);
                // var itemData = _context.ItemTransaction.FromSqlRaw($"Exec VoucherAdditionalsSP @Criteria='{criteria}',@BranchID='{branch}',@ItemID='{itemId}',@AccountID='{partyId}',@VoucherID='{voucherId}'").ToList();
                var outdata = new { BatchNo = batch, PriceCategory = PriceCat };


                return CommonResponse.Ok(outdata);

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        class abc
        {
            public int id { get; set; }
            public decimal? Rate { get; set; }
            public string PriceCategory { get; set; }
            public decimal Perc { get; set; }
        }
        //for price category popup in item grid
        private object PriceCategoryPopup(int itemId)
        {//lin
            var result = _context.MaPriceCategory.Include(x => x.MultiRate)

                  .Where(pc => (bool)pc.Active) // Active price categories
           .Select(pc => new abc()
           {
               id = pc.Id,
               PriceCategory = pc.Name,
               Perc = pc.Perc,
               Rate = pc.MultiRate.Where(x => x.Id == itemId && x.PriceCategoryId == pc.Id).Select(x => x.PurchaseRate).FirstOrDefault()
           }).ToList();

            return result;
        }
        private object GetSettings(string key)
        {
            var settings = _context.MaSettings
        .Where(m => key.Contains(m.Key))
        .Select(m => m.Value).FirstOrDefault();
            return settings;
        }

        /// <summary>
        /// Save Items
        /// </summary>
        /// <param name="invTransDto"></param>
        /// <param name="voucherId"></param>
        /// <param name="transId"></param>
        /// <returns></returns>
        public CommonResponse SaveInvTransItems(List<InvTransItemDto> Items, int voucherId, int transId, decimal? exchangeRate, int? warehouse)
        {
            try
            {
                //int itemId =  ;
                int slNo = 1;
                int refTransId1 = 0;
                string criteria = "InsertInvTransItems";
                decimal factor = 1;
                string tranType = "Normal";
                //decimal? tempRate = ;
                int? rowType = null;
                bool visible = true;
                //decimal avgCost = 0;
                int refTransItemId = 0;
                int? outLocId = null;
                int? inLocId = null;
                decimal? tempQty = null;
               
                bool temQtySameAsQty = Convert.ToBoolean(GetSettings("TempQtySameAsQty"));

                var primeryVoucherId = _inventoryTransactionService.GetPrimaryVoucherID(voucherId);
                switch ((VoucherType)primeryVoucherId)
                {
                    case VoucherType.Purchase:
                    case VoucherType.Purchase_Order:
                    case VoucherType.Opening_Stock:
                    case VoucherType.Stock_Adjustment:
                    case VoucherType.Stock_Return:
                        inLocId = warehouse;
                        break;
                    case VoucherType.Sales_Invoice:
                    case VoucherType.Purchase_Return:
                        outLocId = warehouse;
                        break;
                }
                if (Items != null && Items.Count > 0)
                {
                    foreach (var item in Items)
                    {
                        decimal? stockQty = (item.Qty + item.FocQty) * factor;
                        factor = _context.ItemUnits.Where(x => x.ItemId == item.ItemId && x.Unit == item.Unit.Unit).Select(x => x.Factor).SingleOrDefault();
                        var importItems = _context.InvTransItems.Where(i => i.TransactionId == item.TransactionId).Select(i => i.ItemId).ToList();
                        foreach (var i in importItems)
                        {
                            if (i == item.ItemId)
                                refTransItemId = _context.InvTransItems.Where(t => t.TransactionId == item.TransactionId && t.ItemId == item.ItemId)
                               .Select(t => t.Id).SingleOrDefault();
                        }

                        if ((VoucherType)primeryVoucherId == VoucherType.Purchase ||  (VoucherType)primeryVoucherId == VoucherType.Stock_Adjustment)
                        {
                            rowType = 1;
                            tempQty= SetTempQty(temQtySameAsQty, item);
                        }
                        else if ((VoucherType)primeryVoucherId == VoucherType.Sales_Invoice)
                        {
                            rowType = -1;
                            tempQty = SetTempQty(temQtySameAsQty, item);
                        }
                        if ((VoucherType)primeryVoucherId == VoucherType.Opening_Stock)
                        {
                            tempQty = SetTempQty(temQtySameAsQty, item);
                            stockQty = item.BasicQty;
                            rowType = 1;
                            item.TempRate = item.Rate;
                        }
                        if ((VoucherType)primeryVoucherId == VoucherType.Physical_Stock)
                        {
                            tempQty = SetTempQty(temQtySameAsQty, item);
                            stockQty = item.BasicQty;
                            rowType = null;
                            item.TempRate = item.Rate;
                        }
                        if ((VoucherType)primeryVoucherId == VoucherType.Stock_Return)
                        {
                            outLocId = _context.Locations.Where(x => x.DevCode == 3).Select(x => x.Id).FirstOrDefault();
                            item.TempRate = null;
                            tempQty = null;                            
                        }
                        item.IsReturn = null;
                        if ((VoucherType)primeryVoucherId == VoucherType.Purchase_Return || (VoucherType)primeryVoucherId == VoucherType.Sales_Return)
                            item.IsReturn = true;
                        

                        SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        var data = _context.Database.ExecuteSqlRaw("Exec VoucherAdditionalsSP " +
                                    "@Criteria={0}, @TransactionID={1}, @ItemID={2}, @SerialNo={3}, @RefTransID1={4}, " +
                                    "@Unit={5}, @Qty={6}, @FOCQty={7}, @BasicQty={8}, @Rate={9}, @AdvanceRate={10}, " +
                                    "@OtherRate={11}, @MasterMiscID1={12}, @RowType={13}, @Description={14}, @Remarks={15}, " +
                                    "@IsBit={16}, @InvAvgCostID={17}, @IsReturn={18}, @Discount={19}, @Additional={20}, " +
                                    "@Factor={21}, @CommodityID={22}, @AccountID={23}, @LengthFt={24}, @LengthIn={25}, " +
                                    "@LengthCm={26}, @GirthFt={27}, @GirthIn={28}, @GirthCm={29}, @ThicknessFt={30}, " +
                                    "@ThicknessIn={31}, @ThicknessCm={32}, @TransactionEntryID={33}, @Status={34}, @Cancel={35}, " +
                                    "@MeasuredByID={36}, @RefTransItemID={37}, @FinishDate={38}, @UpdateDate={39}, @IsSameForPcs={40}, " +
                                    "@StockQty={41}, @Margin={42}, @InlocID={43}, @OutLocID={44}, @BatchNo={45}, @SizeMasterID={46}, " +
                                    "@DiscountPerc={47}, @TaxPerc={48}, @TaxValue={49}, @TaxTypeID={50}, @TaxAccountID={51}, " +
                                    "@TranType={52}, @CostPerc={53}, @ManufactureDate={54}, @ExpiryDate={55}, @PriceCategoryID={56}, " +
                                    "@GroupItemID={57}, @RateDisc={58}, @RefID={59}, @TempQty={60}, @TempRate={61}, @ReplaceQty={62}, " +
                                    "@PrintedMRP={63}, @PrintedRate={64}, @PTSRate={65}, @PTRRate={66}, @TempBatchNo={67}, " +
                                    "@StockItemID={68}, @CostAccountID={69}, @CGSTPerc={70}, @CGSTValue={71}, @SGSTPerc={72}, " +
                                    "@SGSTValue={73}, @HSN={74}, @BrandID={75}, @ComplaintNature={76}, @AccLocation={77}, " +
                                    "@RepairsRequired={78}, @ExchangeRate={79}, @CessAccountID={80}, @CessPerc={81}, @CessValue={82}, " +
                                    "@AvgCost={83}, @Profit={84}, @Parent={85}, @ShortageQty={86}, @AvgCostID={87}, @Pcs={88}, " +
                                    "@NewID={89} OUTPUT",

                            criteria,
                                    transId,
                                   item.ItemId == 0 ? null : item.ItemId,
                                    slNo,
                                    null,//4-refTransId1
                                    item.Unit.Unit,//5
                                    item.Qty,//6
                                    item.FocQty == 0 ? null : item.FocQty,//7
                                    item.BasicQty == 0 ? null : item.BasicQty,//8-BasicQty
                                    item.Rate,//9
                                    null,//10-advanceRate
                                    item.OtherRate == 0 ? null : item.OtherRate,//11
                                    null,//12-mastermiscId
                                    rowType == 0 ? null : rowType,//13-RowType
                                    item.Description,//14-Description
                                    item.Remarks,//15-Remarks
                                    null,//16-isBit
                                    null,//17-InvavgCostId
                                    item.IsReturn,//18-IsReturn
                                    item.Discount == 0 ? null : item.Discount,//19
                                    item.Additional == 0 ? null : item.Additional,//20-Additional
                                    factor,//21
                                    null,//22-CommodityId
                                    null,//23-AccountId
                                    item.LengthFt == 0 ? null : item.LengthFt,//24
                                    item.LengthIn == 0 ? null : item.LengthIn,//25
                                    item.LengthCm == 0 ? null : item.LengthCm,//26
                                    item.GirthFt == 0 ? null : item.GirthFt,//27
                                    item.GirthIn == 0 ? null : item.GirthIn,//28
                                    item.GirthCm == 0 ? null : item.GirthCm,//29
                                    item.ThicknessFt == 0 ? null : item.ThicknessFt,//30
                                    item.ThicknessIn == 0 ? null : item.ThicknessIn,//31
                                    item.ThicknessCm == 0 ? null : item.ThicknessCm,//32
                                    null,//33-TransactionentryId
                                    null,//34-status
                                    null,//35-cancel
                                    null,//36-MeasuredByID
                                    refTransItemId == 0 ? null : refTransItemId,//37
                                    item.FinishDate,//38
                                    item.UpdateDate,//39
                                    null,//40-IsSameForPcs
                                    stockQty,//41
                                    item.Margin == 0 ? null : item.Margin,//42
                                    inLocId,//43
                                    outLocId,
                                     item.BatchNo,//45
                                    item.SizeMaster == null ? null : item.SizeMaster.Id == 0 ? null : item.SizeMaster.Id,//46-sizeMasterId
                                    item.DiscountPerc,//47
                                    item.TaxPerc,//48
                                    item.TaxValue,//49
                                    item.TaxTypeId == 0 ? null : item.TaxTypeId,//50
                                    item.TaxAccountId == 0 ? null : item.TaxAccountId,//51
                                    tranType,//52
                                    null,//53-CostPerc
                                    item.ManufactureDate,//54
                                    item.ExpiryDate,//55
                                    item.PriceCategory == null ? null : item.PriceCategory.Id == 0 ? null : item.PriceCategory.Id,//56
                                    null,//57
                                    item.RateDisc == 0 ? null : item.RateDisc,//58
                                    null,//59-refId
                                    tempQty,//60-TempQty
                                    item.TempRate == 0 ? null : item.TempRate,//61
                                    item.ReplaceQty == 0 ? null : item.ReplaceQty,//62
                                    item.PrintedMRP == 0 ? null : item.PrintedMRP,//63-PrintedMRP
                                    item.PrintedRate == 0 ? null : item.PrintedRate,//64
                                    item.PtsRate == 0 ? null : item.PtsRate,//65-ptsRate
                                    item.PtrRate == 0 ? null : item.PtrRate,//66-ptrRate
                                    null,//67-tempBatchNo
                                    item.StockItemId == 0 ? null : item.StockItemId,//68
                                    item.CostAccountId == 0 ? null : item.CostAccountId,//69
                                    null, null, null, null,//70,71,72,73
                                    item.Hsn,
                                    item.BrandId == 0 ? null : item.BrandId,//75
                                    null, null, //76,77
                                    string.IsNullOrEmpty(item.RepairsRequired) ? null : item.RepairsRequired,//78
                                    exchangeRate,//79
                                    null, null, null, //80,81,82
                                    item.AvgCost == 0 ? null : item.AvgCost, //83-avgcost
                                    item.Profit == 0 ? null : item.Profit, //84
                                    null, null, null,//85 ,86,87
                                    item.Pcs == 0 ? null : item.Pcs,
                                    newId
                        );
                        var transItemId = (int)newId.Value;
                        if (item.Pcs != null)
                            SaveInvBatchWiseItems(item, transItemId, item.ItemId);

                        //check whether the IsUniqueItem field of this item is true or false
                        var uniqueItem = _context.ItemMaster.Where(i => i.Id == item.ItemId).Select(i => i.IsUniqueItem).SingleOrDefault();
                        if (uniqueItem == true)
                            if (item.Qty > 0 && item.UniqueItems.Count > 0)
                            {
                                SaveUniqueItems(transId, transItemId, item.ItemId, item.UniqueItems);
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

        private decimal SetTempQty( bool temQtySameAsQty, InvTransItemDto item)
        {
            decimal tempQty=0;
            if (temQtySameAsQty)
            {
               var factor = _context.ItemUnits.Where(x => x.ItemId == item.ItemId && x.Unit == "Carton").Select(x => x.Factor).SingleOrDefault();
                if (item.Unit.Unit != "Carton")
                {
                    if (factor == null || factor == 0)
                        factor = 1;
                    tempQty = item.Qty / factor;
                }
                else
                    tempQty = item.Qty;
            }
            return tempQty;
        }

        public CommonResponse UpdateInvTransItems(List<InvTransItemDto> Items, int voucherId, int transId, decimal? exchangeRate, int? warehouse)
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
                    SaveInvTransItems(Items, voucherId, transId, exchangeRate, warehouse);
                }

                return CommonResponse.Ok("TransItems Updated Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        public CommonResponse DeleteInvTransItem(int Id)
        {
            try
            {
                var check = _context.InvTransItems.Any(i => i.Id == Id);
                if (check == false)
                    return CommonResponse.NotFound("TransItem Not Exists");
                string criteria = "DeleteInvTransItems";
                var data = _context.Database.ExecuteSqlRaw($"Exec VoucherAdditionalsSP @Criteria='{criteria}',@ID='{Id}'");
                return CommonResponse.Ok("TransItem Deleted Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        private string NextBatchNo()
        {

            string criteria = "NextBatchNo";
            var data = _context.NextBatchNoView.FromSqlRaw($"Exec VoucherAdditionalsSP @Criteria='{criteria}'").AsEnumerable();
            var batchNo = data.FirstOrDefault();
            return batchNo.nextBatchNo.ToString();

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

        /****************************************************InvBatchWiseItems**************************************************************/
        private CommonResponse SaveInvBatchWiseItems(InvTransItemDto item, int transItemId, int itemId)
        {
            try
            {
                string criteria = "InsertInvBatchWiseItems";
                int? branchId = _authService.GetBranchId();
                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                var data = _context.Database.ExecuteSqlRaw("Exec VoucherAdditionalsSP @Criteria={0},@TransItemID={1},@ItemID={2}," +
                "@BatchNo={3},@Qty={4},@Pcs={5},@BranchID={6},@NewID={7} OUTPUT",
                criteria, transItemId, itemId, item.BatchNo, item.Qty, item.Pcs, branchId, newId);
                var batchwiseItemId = (int)newId.Value;

                return CommonResponse.Ok(batchwiseItemId);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }



    }
}
