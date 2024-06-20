using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Dfinance.Finance.Services
{
    public class FinanceAdditional : IFinanceAdditional
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;

        public FinanceAdditional(DFCoreContext context, IAuthService authService, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _authService = authService;
            _environment = hostEnvironment;
        }


        //save and update
        public CommonResponse SaveTransactionAdditional(PaymentVoucherDto paymentVoucherDto, int TransId, int voucherId)
        {
            //int? fromLocId = null, toLocId = null, inLocId = null, outLocId = null;
            string criteria = "";
            //switch ((VoucherType)voucherId)
            //{
            //    case VoucherType.Payment_Voucher:
            //    case VoucherType.Receipt_Voucher:
            //}
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
                null,//10
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
                null, null, null,
                null,//69
                null, null, null, null,
                null,//74
                null,
                null,//76
                null,
                null,//78
                null,//79
                null, null,
                null,//82
                null, null, null, null,
                null,//87
                null, null,
                null,//90
                paymentVoucherDto.Department.Id,
                null,//92
                null,//93
                null,//94
                null, null

                );

            return CommonResponse.Ok();


        }

    }
}
