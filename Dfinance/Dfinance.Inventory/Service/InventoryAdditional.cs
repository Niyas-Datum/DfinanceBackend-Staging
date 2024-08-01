using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Dfinance.Inventory.Service
{
    public class InventoryAdditional : IInventoryAdditional
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;

        public InventoryAdditional(DFCoreContext context, IAuthService authService, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _authService = authService;
            _environment = hostEnvironment;


        }
        /// <summary>
        /// PopupVechicleNo in Additionaldetails
        /// </summary>
        /// <returns></returns>
        public CommonResponse PopupVechicleNo()
        {
            try
            {
                var result = _context.MaVehicles.Where(x => !string.IsNullOrEmpty(x.CostCenterId.ToString()) && x.ActiveFlag == 1).Select(x => new { VehicleNo = x.RegistrationNo, Name = x.Name, ID = x.Id }).ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        /// <summary>
        /// Fill Popup Delivary Location(from customer&Suppliyer)
        /// </summary>
        /// <returns></returns>
        public CommonResponse PopupDelivaryLocations(int salesManId)
        {
            try
            {
                var result = _context.DeliveryDetails.Where(x => x.PartyId == (_context.Parties.Where(p => p.AccountId == salesManId).Select(p => p.Id).FirstOrDefault())).Select(x => new { Location = x.LocationName, ProjectName = x.ProjectName, ContactPerson = x.ContactPerson, ContactNo = x.ContactNo, Address = x.Address, Party = x.Party.Name, ID = x.Id }).ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        /************* Fill all TransactionAdditionals  *******************/
        public CommonResponse FillTransactionAdditionals(int transactionId)
        {
            try
            {
                string criteria = "FillFiTransactionAdditionals";
                var result = _context.SpGetTransactionAdditionals.FromSqlRaw($"EXEC VoucherAdditionalsSP @Criteria='{criteria}',@TransactionID='{transactionId}'").ToList();

                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }
        /////************* Fill TransPortation Type By Criteria *******************/
        public CommonResponse GetTransPortationType()
        {
            try
            {

                string criteria = "FillMaMisc";
                string key = "Transportation Mode";
                var result = _context.DropDownViewValue.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}',@StrParam='{key}'").ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }
        /// <summary>
        ///  /////************* Fill SalesArea By Criteria *******************/
        public CommonResponse GetSalesArea()
        {
            try
            {

                string criteria = "FillArea";
                var result = _context.DropDownViewName.FromSqlRaw($"EXEC DropDownListSP @Criteria='{criteria}'").ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }
        /// </summary>
        /// <param name="fiTransactionAdditionalDto"></param>
        /// <returns></returns>
        /****************** Save & Update TransactionAdditional  *******************/
        public CommonResponse SaveTransactionAdditional(InvTransactionAdditionalDto fiTransactionAdditionalDto, int TransId, int voucherId)
        {
            int? fromLocId = null, toLocId = null, inLocId = null, outLocId = null;
            string criteria = "";
            switch ((VoucherType)voucherId)
            {
                case VoucherType.Purchase:
                case VoucherType.Sales_Return:
                case VoucherType.Purchase_Order:
                case VoucherType.Purchase_Request:
                case VoucherType.Purchase_Quotation:

                case VoucherType.Opening_Stock:
                   
                    toLocId = fiTransactionAdditionalDto.Warehouse?.Id ?? null;
                    inLocId = fiTransactionAdditionalDto.Warehouse?.Id??null;

                    break;
                case VoucherType.Sales_Invoice:
                case VoucherType.Purchase_Return:
                case VoucherType.RestaurantInvoice:
                case VoucherType.RestaurantKOT:
                    fromLocId = fiTransactionAdditionalDto.Warehouse?.Id??null;
                    outLocId = fiTransactionAdditionalDto.Warehouse?.Id ?? null;
                    break;

                case VoucherType.Purchase_Enquiry:
                    inLocId = fiTransactionAdditionalDto.Warehouse?.Id ?? null;
                    break;

            }
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

                fiTransactionAdditionalDto.PayType?.Id??null,//5
                null, null,//6,7
                fiTransactionAdditionalDto.DelivaryLocation?.Id ?? null,//8

                fromLocId,//9
                toLocId,//10
                null, null, null, null, null, null, null,
                null,
                fiTransactionAdditionalDto.PartyNameandAddress ?? null, 
                fiTransactionAdditionalDto.Code?? null,
                fiTransactionAdditionalDto.TermsOfDelivery??null,//21
                null, null,//22,23
                fiTransactionAdditionalDto.CreditPeriod??null,//24
                fiTransactionAdditionalDto.Days??null, null,
                fiTransactionAdditionalDto.MobileNo??null,//27
                null, null, null, null,
                fiTransactionAdditionalDto.StaffIncentives ?? null,//32
                null, null, null, null, null, null, null,
                fiTransactionAdditionalDto.DespatchNo?? null,//40
                fiTransactionAdditionalDto.DespatchDate ?? null,//41
                null,
                fiTransactionAdditionalDto.PartyDate??null,//43
                fiTransactionAdditionalDto.PartyInvoiceNo ?? null,//44
                null,
                fiTransactionAdditionalDto.Attention??null,//46
                null, null, null,
                fiTransactionAdditionalDto.ExpiryDate ?? null,//50
                null, null, null, null, null, null, null,
                fiTransactionAdditionalDto.DeliveryDate??null,//58
                null, null, null, null,
                fiTransactionAdditionalDto.DeliveryNote ?? null,//63
                fiTransactionAdditionalDto.OrderDate??null,//64
                fiTransactionAdditionalDto.OrderNo ?? null,//65
                null, null, null,

                fiTransactionAdditionalDto.VehicleNo?.Id ?? null,//69
                null, null, null, null,
                fiTransactionAdditionalDto.DelivaryLocation?.Name??null,//74

                null,
                fiTransactionAdditionalDto.Approve??null,//76
                fiTransactionAdditionalDto.TransPortationType.Id,
                inLocId,//78
                outLocId,//79
                null, null,

                fiTransactionAdditionalDto.SalesMan?.Id ?? null,//82
                null, null, null, null,
                fiTransactionAdditionalDto.SalesArea?.Id??null,//87
                null, null,
                fiTransactionAdditionalDto.CloseVoucher??null,//90
                null,
                fiTransactionAdditionalDto.PartyName ?? null,//92
                fiTransactionAdditionalDto.AddressLine1??null,//93
                fiTransactionAdditionalDto.AddressLine2 ?? null,//94

                null, null

                );

            return CommonResponse.Ok();


        }


        /// <summary>
        /// deleteTransactionAdditional
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CommonResponse DeleteTransactionAdditional(int Id)
        {
            try
            {
                string msg = null;
                var name = _context.FiTransactionAdditionals.Where(i => i.TransactionId == Id).
                    Select(i => i.Name).
                    SingleOrDefault();
                if (name == null)
                {
                    msg = "TransactionAdditional Not Found";
                }
                else
                {
                    string criteria = "DeleteFiTransactionAdditionals";
                    var result = _context.Database.ExecuteSqlRaw($"EXEC VoucherAdditionalsSP @Criteria='{criteria}',@TransactionID='{Id}'");
                    msg = name + " Is Deleted Successfully";
                }
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }
    }
}
