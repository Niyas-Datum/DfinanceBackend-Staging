﻿using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Stock.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;
using System.Transactions;

namespace Dfinance.Stock.Services
{
    public class PhysicalStockService : IPhysicalStockService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<PhysicalStockService> _logger;
        private readonly IInventoryItemService _itemService;
        private readonly IUserTrackService _userTrackService;
        public PhysicalStockService(DFCoreContext context, IAuthService authService, ILogger<PhysicalStockService> logger,
             IInventoryItemService itemService, IUserTrackService userTrackService)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
            _itemService = itemService;
            _userTrackService = userTrackService;
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
        public CommonResponse SavePhysicalStock(PhysicalStockDto physicalStockDto, int pageId, int voucherId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 2))
            {
                return PermissionDenied("Physical Stock");
            }
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    string Status = "Approved";
                    int TransId = (int)SaveTransactions(physicalStockDto, pageId, voucherId).Data;
                    SaveTransactionAdditionals(physicalStockDto, TransId, voucherId);
                    _itemService.SaveInvTransItems(physicalStockDto.Items,voucherId, TransId, physicalStockDto.ExchangeRate,null);
                    transactionScope.Complete();
                    _logger.LogInformation("Physical Stock Saved Successfully");
                    return CommonResponse.Created("Physical Stock Saved Successfully");
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    _logger.LogError(ex.Message);
                    return CommonResponse.Error(ex.Message);
                }
            }
        }

        public CommonResponse UpdatePhysicalStock(PhysicalStockDto physicalStockDto, int pageId, int voucherId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 3))
            {
                return PermissionDenied("Physical Stock");
            }
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    string Status = "Approved";
                    int TransId = (int)SaveTransactions(physicalStockDto, pageId, voucherId).Data;
                    SaveTransactionAdditionals(physicalStockDto, TransId, voucherId);
                    _itemService.UpdateInvTransItems(physicalStockDto.Items, voucherId, TransId, physicalStockDto.ExchangeRate, null);
                    transactionScope.Complete();
                    _logger.LogInformation("Physical Stock Updated Successfully");
                    return CommonResponse.Created("Physical Stock Updated Successfully");
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    _logger.LogError(ex.Message);
                    return CommonResponse.Error(ex.Message);
                }
            }
        }
        public CommonResponse DeletePhystock(int TransId,int pageId, string Msg)
        {
            try
            {
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 5))
                {
                    return PermissionDenied("Delete Payment Voucher");
                }
                var transid = _context.FiTransaction.Any(x => x.Id == TransId);
                if (!transid)
                {
                    return CommonResponse.NotFound("Transaction Not Found");
                }
                string? reference = _context.FiTransaction.Where(r => r.Id == TransId).Select(r => r.TransactionNo).FirstOrDefault();
                string criteria = "DeleteTransactions";
                _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @ID={1}",
                                       criteria, TransId);
                _userTrackService.AddUserActivity(reference, TransId, 2, Msg, "FiTransactions", "Physical Stock", 0, null);
                _logger.LogInformation("Delete successfully");
                return CommonResponse.Ok("Delete successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse CancelPhysicalStock(int transId, string reason)
        {
            try
            {
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
                _userTrackService.AddUserActivity(reference, transId, 1, reason, "FiTransactions", "Physical Stock", 0, null);
                return CommonResponse.Ok("Cancelled successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        private CommonResponse SaveTransactions(PhysicalStockDto PhyStockDto, int pageId, int voucherId, bool? cancel = false)
        {
            int branchId = _authService.GetBranchId().Value;
            int userId = _authService.GetId().Value;
            DateTime AddedDate = DateTime.Now;
            string ApprovalStatus = "A";
            string Status = "Approved";
            bool IsAutoEntry = false;
            bool Active = true;
            bool Posted = true;
            bool Cancelled = false;
            bool IsPostDated = false;
            string criteria = "";
            var transId = 0;
            int? editedBy = null;
            int actionId = 0;
            string reason = "";
            int? accId = null;
            string machinename = Environment.MachineName;
            if (PhyStockDto.Id == 0 || PhyStockDto.Id == null)
            {
                reason = "Added";
                var voucherNoCheck = _context.FiTransaction.Any(t => t.VoucherId == voucherId && t.TransactionNo == PhyStockDto.VoucherNo);
                if (voucherNoCheck)
                    return CommonResponse.Error("VoucherNo already exists");
                criteria = "InsertTransactions";
                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @Date={1}, @EffectiveDate={2}, @VoucherID={3}, @MachineName={4}, " +
                               "@TransactionNo={5}, @IsPostDated={6}, @CurrencyID={7}, @ExchangeRate={8}, @RefPageTypeID={9}, " +
                               "@RefPageTableID={10}, @ReferenceNo={11}, @CompanyID={12}, @FinYearID={13}, @InstrumentType={14}, " +
                               "@InstrumentNo={15}, @InstrumentDate={16}, @InstrumentBank={17}, @CommonNarration={18}, @AddedBy={19}, " +
                               "@ApprovedBy={20}, @AddedDate={21}, @ApprovedDate={22}, @ApprovalStatus={23}, " +
                               "@ApproveNote={24}, @Action={25}, @Status={26}, @IsAutoEntry={27}, " +
                               "@Posted={28}, @Active={29}, @Cancelled={30}, @AccountID={31}, @Description={32}, " +
                               "@RefTransID={33}, @CostCentreID={34}, @PageID={35}, @NewID={36} OUTPUT",
                               criteria, PhyStockDto.Date, PhyStockDto.Date, voucherId, machinename, PhyStockDto.VoucherNo, IsPostDated, PhyStockDto.Currency.Id, PhyStockDto.ExchangeRate,
                               null, null, null, branchId, null, null, null, null, null, null, userId, null, AddedDate, null,
                               ApprovalStatus, null, null, Status, IsAutoEntry, Posted, Active, Cancelled, accId, null, null, null, pageId, newId);

                transId = (int)newId.Value;

            }
            else
            {
                actionId = 1;
                reason = "Updated";
               
                if (cancel == true)
                {
                    Cancelled = true;
                    editedBy = userId;
                }

                int Transaction = _context.FiTransaction.Where(x => x.Id == PhyStockDto.Id).Select(x => x.Id).FirstOrDefault();
                if (Transaction == null)
                {
                    return CommonResponse.NotFound();
                }
                criteria = "UpdateTransactions";

                _context.Database.ExecuteSqlRaw("EXEC VoucherSP " +
                    "@Criteria={0}, @Date={1}, @EffectiveDate={2}, @VoucherID={3}, @MachineName={4}, " +
                    "@TransactionNo={5}, @IsPostDated={6}, @CurrencyID={7}, @ExchangeRate={8}, @RefPageTypeID={9}, " +
                    "@RefPageTableID={10}, @ReferenceNo={11}, @CompanyID={12}, @FinYearID={13}, @InstrumentType={14}, " +
                    "@InstrumentNo={15}, @InstrumentDate={16}, @InstrumentBank={17}, @CommonNarration={18}, @AddedBy={19}, " +
                    "@ApprovedBy={20}, @AddedDate={21}, @ApprovedDate={22}, @ApprovalStatus={23}, " +
                    "@ApproveNote={24}, @Action={25}, @Status={26}, @IsAutoEntry={27}, " +
                    "@Posted={28}, @Active={29}, @Cancelled={30}, @AccountID={31}, @Description={32}, " +
                    "@RefTransID={33}, @CostCentreID={34}, @PageID={35}, @ID={36}",
                            criteria, PhyStockDto.Date, PhyStockDto.Date, voucherId, machinename, PhyStockDto.VoucherNo, IsPostDated, PhyStockDto.Currency.Id, PhyStockDto.ExchangeRate,
                               null, null, null, branchId, null, null, null, null, null, null, userId, null, AddedDate, null,
                               ApprovalStatus, null, null, Status, IsAutoEntry, Posted, Active, Cancelled, accId, null, null, null, pageId, PhyStockDto.Id);

                transId = (int)PhyStockDto.Id;

            }
            decimal amount = 0;
            var jsonBudget = JsonSerializer.Serialize(PhyStockDto);
            _userTrackService.AddUserActivity(PhyStockDto.VoucherNo, transId, actionId, reason, "FiTransactions", "Physical Stock", amount, jsonBudget);
            return CommonResponse.Ok(transId);
        }
        private CommonResponse SaveTransactionAdditionals(PhysicalStockDto phyDto, int TransId, int voucherId)
        {
            string criteria = "";
            var additionalId = _context.FiTransactionAdditionals.Any(x => x.TransactionId == TransId);
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
                TransId,//1
                null, null, null,//2,3,4
                null,//5
                null, null,//6,7
                null,//8
                null,//9
                phyDto.Warehouse.Id,//10
                null, null, null, null, null, null, null, null, null, null,//11,12,13,14,15,16,17,18,19,20
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
                null, phyDto.Terms, null,
                null,//69
                null, null, null, null,
                null,//74
                null,
                null,//76
                null,
                phyDto.Warehouse.Id,//78
                null,//79
                null, null,
                null,//82
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

            return CommonResponse.Ok();


        }
    }
}
