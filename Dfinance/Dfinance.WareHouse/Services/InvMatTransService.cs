using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Dfinance.WareHouse.Services.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dfinance.WareHouse.Services
{
    public class InvMatTransService : IInvMatTransService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public InvMatTransService(DFCoreContext context, IAuthService authService)
        {
            _authService = authService;
            _context = context;
        }
        public CommonResponse SaveMatTransaction(MaterialTransferDto materialDto, int pageId, int voucherId)
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;
                int createdBy = _authService.GetId().Value;
                bool Autoentry = false;
                string? ApprovalStatus = "A", status = "Approved", InstrumentBank = null, ApproveNote = null, environmentname = null;
                bool isPostDated = false, Posted = true, Active = true, Cancelled = false;
                int? refPageTypeId = null, ApprovedBy = null, refPageTableID = null, finYearID = null, RefTransId = null, currency = null;
                char? InstrumentType = null, Action = null;
                string? InstrumentNo = null;
                DateTime? InstrumentDate = null, ApprovedDate = null;
                decimal excRate = 1;
                if (materialDto.TransactionId == null || materialDto.TransactionId == 0)
                {
                    string criteria = "InsertTransactions";
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
                       criteria, materialDto.VoucherDate, DateTime.Now, voucherId, environmentname,
                       materialDto.VoucherNo, isPostDated, currency, excRate, refPageTypeId, refPageTableID, materialDto.Reference,
                       branchId, finYearID, InstrumentType, InstrumentNo, InstrumentDate, InstrumentBank, null,//common Narration
                       createdBy, ApprovedBy, DateTime.Now, ApprovedDate, ApprovalStatus, ApproveNote, Action, status, Autoentry, Posted,
                       Active, Cancelled, materialDto.BranchAccount.Id, materialDto.Description, RefTransId, null, pageId, newId);
                    var NewId = (int)newId.Value;
                    return CommonResponse.Ok(NewId);
                }
                else
                {
                    var transId = _context.FiTransaction.Any(i => i.Id == materialDto.TransactionId);
                    if (!transId)
                        return CommonResponse.NotFound("Transaction Not Found");
                    string criteria = "UpdateTransactions";

                    _context.Database.ExecuteSqlRaw("EXEC VoucherSP " +
                        "@Criteria={0}, @Date={1}, @EffectiveDate={2}, @VoucherID={3}, @MachineName={4}, " +
                        "@TransactionNo={5}, @IsPostDated={6}, @CurrencyID={7}, @ExchangeRate={8}, @RefPageTypeID={9}, " +
                        "@RefPageTableID={10}, @ReferenceNo={11}, @CompanyID={12}, @FinYearID={13}, @InstrumentType={14}, " +
                        "@InstrumentNo={15}, @InstrumentDate={16}, @InstrumentBank={17}, @CommonNarration={18}, @AddedBy={19}, " +
                        "@ApprovedBy={20}, @AddedDate={21}, @ApprovedDate={22}, @ApprovalStatus={23}, " +
                        "@ApproveNote={24}, @Action={25}, @Status={26}, @IsAutoEntry={27}, " +
                        "@Posted={28}, @Active={29}, @Cancelled={30}, @AccountID={31}, @Description={32}, " +
                        "@RefTransID={33}, @CostCentreID={34}, @PageID={35}, @ID={36}",
                        criteria, materialDto.VoucherDate, DateTime.Now, voucherId, environmentname,
                       materialDto.VoucherNo, isPostDated, currency, excRate, refPageTypeId, refPageTableID, materialDto.Reference,
                       branchId, finYearID, InstrumentType, InstrumentNo, InstrumentDate, InstrumentBank, null,//common Narration
                       createdBy, ApprovedBy, DateTime.Now, ApprovedDate, ApprovalStatus, ApproveNote, Action, status, Autoentry, Posted,
                       Active, Cancelled, materialDto.BranchAccount.Id, materialDto.Description, RefTransId, null, pageId, materialDto.TransactionId);
                    return CommonResponse.Ok(materialDto.TransactionId);
                }
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse SaveMatTransAdditional(MaterialTransAddDto materialTransAddDto, int transId, int voucherId)
        {
            try
            {
                string criteria = "";
                int? fromLocId = null, toLocId = null, inLocId = null, outLocId = null, otherBranch = null;
                switch ((VoucherType)voucherId)
                {
                    case VoucherType.Material_Request:
                        inLocId = materialTransAddDto.FromWarehouse.Id;
                        outLocId = materialTransAddDto.ToWarehouse.Id;
                        fromLocId = materialTransAddDto.ToWarehouse.Id;
                        toLocId = materialTransAddDto.FromWarehouse.Id;
                        otherBranch = materialTransAddDto.MainBranch.Id;
                        break;
                    case VoucherType.Material_Receive:
                        toLocId = materialTransAddDto.ToWarehouse.Id;
                        fromLocId = materialTransAddDto.FromWarehouse.Id;
                        inLocId = materialTransAddDto.ToWarehouse.Id;
                        outLocId = null;
                        otherBranch = materialTransAddDto.MainBranch.Id;
                        break;
                }
                var additionalId = _context.FiTransactionAdditionals.Any(x => x.TransactionId == transId);
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
                   transId,//1
                   null, null, null, null, null, null, null,
                   fromLocId,//9
                   toLocId,//10
                   null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,//27
                   null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
                   null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
                   null, null, null, null, null, null, null, null, null,
                   materialTransAddDto.Terms,
                   null, null, null, null, null, null, null, null, null, null,
                   inLocId,//78
                   outLocId,//79
                   null, null, null, null, null, null,
                   otherBranch,
                   null, null, null, null, null, null, null, null, null, null
                   );
                return CommonResponse.Ok();
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse SaveMaterialReference(int transId, List<int?> referIds)
        {
            try
            {
                List<int> processedReferIds = new List<int>();
                string dec = null;
                string criteria = "InsertTransactionReferences";

                foreach (int referId in referIds)
                {
                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0}, @TransactionID={1}, @RefTransID={2}, @Description={3}, @NewID={4} OUTPUT",
                        criteria, transId, referId, dec, newId);

                    processedReferIds.Add(referId);
                }
                return CommonResponse.Ok();
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse UpdateMaterialReference(int transId, List<int?> referIds)
        {
            try
            {
                var Transref = _context.TransReference.FirstOrDefault(x => x.TransactionId == transId);

                if (Transref == null)
                {
                    return CommonResponse.Error("Transaction reference not found.");

                }

                string dec = null;
                string criteria = "UpdateTransactionReferences";

                foreach (var referId in referIds)
                {
                    _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0}, @TransactionID={1}, @RefTransID={2}, @Description={3}, @ID={4}",
                        criteria, transId, referId, dec, Transref.Id);
                }
                return CommonResponse.Ok();
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }
        public CommonResponse SaveMaterialTransItems(List<InvTransItemDto> Items, int voucherId, int transId, int? toWh)
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
                switch ((VoucherType)voucherId)
                {
                    case VoucherType.Material_Receive:
                        inLocId = toWh;
                        outLocId = _context.Locations.Where(i => i.DevCode == 3).Select(i => i.Id).FirstOrDefault();
                        rowType = 1;
                        break;
                }
                if (Items != null && Items.Count > 0)
                {
                    foreach (var item in Items)
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
                                    (item.Qty + item.FocQty) * factor,//41
                                    item.Margin == 0 ? null : item.Margin,//42
                                    inLocId,//43
                                    outLocId,
                                     item.BatchNo,//45
                                    null,//46-sizeMasterId
                                    item.DiscountPerc,//47
                                    item.TaxPerc,//48
                                    item.TaxValue,//49
                                    item.TaxTypeId == 0 ? null : item.TaxTypeId,//50
                                    item.TaxAccountId == 0 ? null : item.TaxAccountId,//51
                                    tranType,//52
                                    null,//53-CostPerc
                                    item.ManufactureDate,//54
                                    item.ExpiryDate,//55
                                    item.PriceCategory.Id == 0 ? null : item.PriceCategory.Id,//56
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
                                    null,//79
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
        public CommonResponse UpdateMaterialTransItems(List<InvTransItemDto> Items, int voucherId, int transId, int? toWh)
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
                    SaveMaterialTransItems(Items, voucherId, transId, toWh);
                }

                return CommonResponse.Ok("TransItems Updated Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
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
        public CommonResponse SaveMatTransEntries(int transId,int? account, int? branchAccount, decimal? amount,int voucherId)
        {
            try
            {
                
                string DrCr = null;
                switch ((VoucherType)voucherId)
                {
                    case VoucherType.Material_Receive:
                        DrCr = "C";
                        break;
                    case VoucherType.Material_Issue:
                        DrCr = "D";
                        break;
                }
                string nature = "M";
                string criteria = "InsertTransactionEntries";
                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                if (branchAccount != null)
                {
                    _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0},@TransactionId={1},@DrCr={2},@Nature={3}," +
              "@AccountID={4},@Amount={5},@FCAmount={6},@BankDate={7},@RefPageTypeID={8},@CurrencyID={9},@ExchangeRate={10}," +
              "@RefPageTableID={11}, @ReferenceNo={12}, @Description={13}, @TranType={14}, @DueDate={15}, @RefTransID={16}, @TaxPerc={17},@NewID={18} OUTPUT",
              criteria, transId, "C", nature, branchAccount, amount, amount, null, null, null, null, null, "Party", null, null, null, null, null, null, null, newId);
                }
               if(account!=null)
                {
                    _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0},@TransactionId={1},@DrCr={2},@Nature={3}," +
              "@AccountID={4},@Amount={5},@FCAmount={6},@BankDate={7},@RefPageTypeID={8},@CurrencyID={9},@ExchangeRate={10}," +
              "@RefPageTableID={11}, @ReferenceNo={12}, @Description={13}, @TranType={14}, @DueDate={15}, @RefTransID={16}, @TaxPerc={17},@NewID={18} OUTPUT",
              criteria, transId, "D", nature, account, amount, amount, null, null, null, null, null, null, null, null, null, null, null, null, null, newId);

                }
                return CommonResponse.Ok();            
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse UpdateMatTransEntries(int transId,int? account, int? branchAccount, decimal? amount, int voucherId)
        {
            try
            {
                var remove = _context.FiTransactionEntries.Where(x => x.TransactionId == transId);
                _context.FiTransactionEntries.RemoveRange(remove);
                _context.SaveChanges();
                SaveMatTransEntries(transId,account,branchAccount,amount,voucherId);
                return CommonResponse.Ok();
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
    }
}
