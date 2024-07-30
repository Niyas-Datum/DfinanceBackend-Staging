using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Dfinance.WareHouse.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Stock.Services
{
    public class StockTransService :IStockTransService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<StockTransService> _logger;
        private readonly IInventoryTransactionService _transactionService;
        private readonly IInventoryAdditional _additionalService;
        private readonly IInventoryItemService _itemService;
        private readonly IUserTrackService _userTrackService;
        public StockTransService(DFCoreContext context, IAuthService authService, ILogger<StockTransService> logger, IInventoryTransactionService transactionService,
           IInventoryAdditional additionalService, IInventoryItemService itemService, IUserTrackService userTrackService)
        {
            _additionalService = additionalService;
            _context = context;
            _authService = authService;
            _logger = logger;
            _transactionService = transactionService;
            _itemService = itemService;
            _userTrackService = userTrackService;
        }
        public CommonResponse SaveFiTransaction(StockTransDto transaction)
        {
            string criteria;
            int? transId=null;
            var machineName=Environment.MachineName;
            bool isPosted=false;
            var companyId=_authService.GetBranchId().Value;
            var addedBy=_authService.GetId().Value;
            var status = "Approved";
            var addedDate=DateTime.Now;
            bool isAutoEntry=false;
            bool posted=true;
            bool active=true;
            bool cancelled=false;
            if (transaction.Id == null || transaction.Id == 0)
            {
                criteria = "InsertTransactions";
                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                transId = (int)newId.Value;
                _context.Database.ExecuteSqlRaw("EXEC VoucherSP " +
                        "@Criteria={0}, @Date={1}, @EffectiveDate={2}, @VoucherID={3}, @MachineName={4}, " +
                        "@TransactionNo={5}, @IsPostDated={6}, @CurrencyID={7}, @ExchangeRate={8}, @RefPageTypeID={9}, " +
                        "@RefPageTableID={10}, @ReferenceNo={11}, @CompanyID={12}, @FinYearID={13}, @InstrumentType={14}, " +
                        "@InstrumentNo={15}, @InstrumentDate={16}, @InstrumentBank={17}, @CommonNarration={18}, @AddedBy={19}, " +
                        "@ApprovedBy={20}, @AddedDate={21}, @ApprovedDate={22}, @ApprovalStatus={23}, " +
                        "@ApproveNote={24}, @Action={25}, @Status={26}, @IsAutoEntry={27}, " +
                        "@Posted={28}, @Active={29}, @Cancelled={30}, @AccountID={31}, @Description={32}, " +
                        "@RefTransID={33}, @CostCentreID={34}, @PageID={35}, @NewID={36} OUTPUT",
                        criteria, transaction.Date, transaction.Date, transaction.VoucherId, machineName, transaction.VoucherNo,
                        isPosted, transaction.Currency.Id, transaction.ExchangeRate, null, null,
                        transaction.Reference, companyId, null, null, null,null, null, transaction.Description, addedBy, null,
                        addedDate, null, DBApprovalStatus.APPROVED, null, null,status, isAutoEntry, posted, active, cancelled, null,
                        transaction.Description, null, null, transaction.PageId, newId);
            }
            else
            {
                int Transaction = _context.FiTransaction.Where(x => x.Id == transaction.Id).Select(x => x.Id).FirstOrDefault();
                if (Transaction == null)
                {
                    return CommonResponse.NotFound();
                }
                criteria = "UpdateTransactions";
                transId = transaction.Id;            
            _context.Database.ExecuteSqlRaw("EXEC VoucherSP " +
                        "@Criteria={0}, @Date={1}, @EffectiveDate={2}, @VoucherID={3}, @MachineName={4}, " +
                        "@TransactionNo={5}, @IsPostDated={6}, @CurrencyID={7}, @ExchangeRate={8}, @RefPageTypeID={9}, " +
                        "@RefPageTableID={10}, @ReferenceNo={11}, @CompanyID={12}, @FinYearID={13}, @InstrumentType={14}, " +
                        "@InstrumentNo={15}, @InstrumentDate={16}, @InstrumentBank={17}, @CommonNarration={18}, @AddedBy={19}, " +
                        "@ApprovedBy={20}, @AddedDate={21}, @ApprovedDate={22}, @ApprovalStatus={23}, " +
                        "@ApproveNote={24}, @Action={25}, @Status={26}, @IsAutoEntry={27}, " +
                        "@Posted={28}, @Active={29}, @Cancelled={30}, @AccountID={31}, @Description={32}, " +
                        "@RefTransID={33}, @CostCentreID={34}, @PageID={35}, @ID={36}",
                         criteria, transaction.Date, transaction.Date, transaction.VoucherId, machineName, transaction.VoucherNo,
                        isPosted, transaction.Currency.Id, transaction.ExchangeRate, null, null,
                        transaction.Reference, companyId, null, null, null, null, null, transaction.Description, addedBy, null,
                        addedDate, null, DBApprovalStatus.APPROVED, null, null, status, isAutoEntry, posted, active, cancelled, null,
                        transaction.Description, null, null, transaction.PageId, Transaction);
            }
            return CommonResponse.Ok(transId);
        }
        //private string inventoryApproval;
        //private object GetSettings(string key)
        //{
        //    var settings = _context.MaSettings
        //.Where(m => key.Contains(m.Key))
        //.Select(m => m.Value).FirstOrDefault();
        //    return settings;
        //}
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
        public CommonResponse DeleteTransactions(int transId, int pageId, string reason)
        {
            try
            {

                 var moduleName = _context.MaPageMenus.Where(p => p.Id == pageId).Select(p => p.MenuText).FirstOrDefault();
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 5))
                {
                    return PermissionDenied("Delete "+ moduleName);
                }
                var transid = _context.FiTransaction.Any(x => x.Id == transId);
                if (!transid)
                {
                    return CommonResponse.NotFound("Transaction Not Found");
                }
                string? reference = _context.FiTransaction.Where(r => r.Id == transId).Select(r => r.TransactionNo).FirstOrDefault();               
                string criteria = "DeleteTransactions";
                _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @ID={1}",
                                       criteria, transId);
                _userTrackService.AddUserActivity(reference, transId, 2, reason, "FiTransactions", moduleName, 0, null);
                _logger.LogInformation("Delete successfully");
                return CommonResponse.Ok("Delete successfully");
            }
            catch (Exception ex)
            {

                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse CancelTransaction(int transId, int pageId, string reason)
        {
            try
            {
                var moduleName = _context.MaPageMenus.Where(p => p.Id == pageId).Select(p => p.MenuText).FirstOrDefault();
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 4))
                {
                    return PermissionDenied("Cancel" + moduleName );
                }
                var transid = _context.FiTransaction.Any(x => x.Id == transId);
                if (!transid)
                {
                    return CommonResponse.NotFound("Transaction Not Found");
                }
                DateTime currentDate = DateTime.Now;
                var query = "UPDATE [FiTransactions] SET [Cancelled] = @cancelled, [Description] = @description, [EditedBy] = @editedBy, [EditedDate] = @editedDate WHERE [ID] = @id";

                _context.Database.ExecuteSqlRaw(query,
                    new SqlParameter("@cancelled", true),
                    new SqlParameter("@description", reason),
                    new SqlParameter("@editedBy", _authService.GetId()),
                    new SqlParameter("@editedDate", currentDate),
                    new SqlParameter("@id", transId));
                _context.SaveChanges();
                string? reference = _context.FiTransaction.Where(r => r.Id == transId).Select(r => r.TransactionNo).FirstOrDefault();                
                _userTrackService.AddUserActivity(reference, transId, 1, reason, "FiTransactions", moduleName, 0, null);
                //SaveTransaction(inventoryTransactionDto, PageId, VoucherId, "Approve");
                return CommonResponse.Ok("Cancelled successfully");
            }
            catch (Exception ex)
            {

                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse SaveTransAdditionals(StockTransAdditional transAdditional)
        {
            try
            {
                string criteria;
                var additionalId = _context.FiTransactionAdditionals.Any(x => x.TransactionId == transAdditional.TransactionId);
                if (!additionalId)
                    criteria = "InsertFiTransactionAdditionals";
                else
                    criteria = "UpdateFiTransactionAdditionals";

                _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0},@TransactionID={1},@RefTransID1={2},@RefTransID2={3},@TypeID={4},@ModeID={5},@MeasureTypeID={6}," +
            "@LoadMeasureTypeID={7},@ConsignTermID={8},@FromLocationID={9},@ToLocationID={10},@ExchangeRate1={11}, @AdvanceExRate={12}, @CustomsExRate={13}, @ApprovalDays={14}," +
            "@WorkflowDays={15}, @PostedBranchID={16}, @ShipBerthDate={17}, @IsBit={18}, @Name={19},@Code={20}, @Address={21}, @Rate={22}, @SystemRate={23}, @Period={24}," +
            "@Days={25}, @LCOptionID={26}, @LCNo={27}, @LCAmt={28}, @AvailableLCAmt={29}, @CreditAmt={30}, @MarginAmt={31}, @InterestAmt={32}, @AvailableAmt={33}," +
            "@AllocationPerc={34}, @InterestPerc={35}, @TolerencePerc={36}, @CountryID={37}, @CountryOfOriginID={38}, @MaxDays={39}, @DocumentNo={40}, @DocumentDate={41}, @BEMaxDays={42}," +
            "@EntryDate={43}, @EntryNo={44}, @ApplicationCode={45}, @BankAddress={46}, @Unit={47}, @Amount={48}, @AcceptDate={49}, @ExpiryDate={50}, @DueDate={51}, @OpenDate={52}, @CloseDate={53}, @StartDate={54}," +
            "@EndDate={55}, @ClearDate={56}, @ReceiveDate={57}, @SubmitDate={58}, @EndTime={59}, @HandOverTime={60}, @LorryHireRate={61}, @QtyPerLoad={62}, @PassNo={63}, @ReferenceDate={64}, @ReferenceNo={65}," +
            "@AuditNote={66}, @Terms={67}, @FirmID={68}, @VehicleID={69}, @WeekDays={70}, @BankWeekDays={71}, @RecommendByID={72}, @RecommendDate={73}, @RecommendNote={74}, @RecommendStatus={75}," +
            "@IsHigherApproval={76}, @LCApplnTransID={77}, @InLocID={78}, @OutLocID={79}, @ExchangeRate2={80}, @RouteID={81}, @AccountID={82}, @AccountID2={83}, @Hours={84}, @Year={85}," +
            "@BranchID={86}, @AreaID={87}, @TaxFormID={88}, @PriceCategoryID={89}, @IsClosed={90}, @DepartmentID={91}, @PartyName={92}, @Address1={93}, @Address2={94}, @ItemID={95}, @VATNo={96}",
                    criteria,//0
                    transAdditional.TransactionId,//1
                    null, null, null,//2,3,4
                    null,//5
                    null, null,//6,7
                    null,//8
                    transAdditional.FromLocationId.Id,//9
                    transAdditional.ToLocationId.Id,//10
                    null, null, null, null, null, null, null, null, null,
                    null,//11,12,13,14,15,16,17,18,19,20
                     null,//21
                    null, null,//22,23
                    null,//24
                    null, null,
                    null,//27
                    null, null, null, null,
                    null,//32
                    null, null, null, null, null, null, null,
                    null,//40
                    null,//41
                    null,
                    null,//43
                    null,//44
                    null,
                    null,//46
                    null, null, null,
                    null,//50
                    null, null, null, null, null, null, null,
                    null,//58
                    null, null, null, null,
                    null,//63
                    null,//64
                    null,//65
                    null, transAdditional.Terms, null, null,//69
                    null, null, null, null,
                    null,//74
                    null,
                    null,//76
                    null,
                    transAdditional.InLocId.Id,//78
                    transAdditional.OutLocId.Id,//79
                    null, null,
                    transAdditional.SalesMan?.Id ?? null,//82
                    null, null, null, null,
                    null,//87
                    null, null,
                    null,//90
                    null,
                    null,//92
                    null,//93
                    null,//94
                    null, null

                    );
                return CommonResponse.Ok(transAdditional);

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse SaveStockInvTransItems(StockTransDto transDto)
        {
            try
            {
                //int itemId =  ;
                int slNo = 1;
                int refTransId1 = 0;
                string criteria = "InsertInvTransItems";
                decimal factor = 1;
                string tranType = "Normal";
                decimal tempRate = 0;
                int rowType = 0;
                bool visible = true;
                //decimal avgCost = 0;
                int refTransItemId = 0;
                int? outLocId = null;
                int? inLocId = null;
                //switch ((VoucherType)transDto.VoucherId)
                //{
                //    case VoucherType.Purchase:
                //    case VoucherType.Purchase_Order:
                //    case VoucherType.Opening_Stock:
                //        inLocId = warehouse;
                //        break;
                //    case VoucherType.Sales_Invoice:
                //        outLocId = warehouse;
                //        break;
                //}
                if (transDto.Items != null && transDto.Items.Count > 0)
                {
                    foreach (var item in transDto.Items)
                    {
                        factor = _context.ItemUnits.Where(x => x.ItemId == item.ItemId && x.Unit == item.Unit.Unit).Select(x => x.Factor).SingleOrDefault();
                        var importItems = _context.InvTransItems.Where(i => i.TransactionId == item.TransactionId).Select(i => i.ItemId).ToList();
                        foreach (var i in importItems)
                        {
                            if (i == item.ItemId)
                                refTransItemId = _context.InvTransItems.Where(t => t.TransactionId == item.TransactionId && t.ItemId == item.ItemId)
                               .Select(t => t.Id).SingleOrDefault();
                        }

                        if ((VoucherType)transDto.VoucherId == VoucherType.Opening_Stock)
                        {
                            rowType = 1;
                        }
                        
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
                                    transDto.Id,
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
                                    (item.Qty + item.FocQty) * factor,//41
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
                                    (item.Qty),//60-TempQty
                                    tempRate == 0 ? null : tempRate,//61
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
                                    transDto.ExchangeRate,//79
                                    null, null, null, //80,81,82
                                    item.AvgCost == 0 ? null : item.AvgCost, //83-avgcost
                                    item.Profit == 0 ? null : item.Profit, //84
                                    null, null, null,//85 ,86,87
                                    item.Pcs == 0 ? null : item.Pcs,
                                    newId
                        );
                        var transItemId = (int)newId.Value;
                        //if (item.Pcs != null)
                        //    SaveInvBatchWiseItems(item, transItemId, item.ItemId);

                        //check whether the IsUniqueItem field of this item is true or false
                        var uniqueItem = _context.ItemMaster.Where(i => i.Id == item.ItemId).Select(i => i.IsUniqueItem).SingleOrDefault();
                        if (uniqueItem == true)
                            if (item.Qty > 0 && item.UniqueItems.Count > 0)
                            {
                                SaveUniqueItems(transDto.Id, transItemId, item.ItemId, item.UniqueItems);
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

        public CommonResponse UpdateInvTransItems(StockTransDto transDto)
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
                    SaveStockInvTransItems(transDto);
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
