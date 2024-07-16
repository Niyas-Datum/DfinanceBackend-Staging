using AutoMapper;
using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Inventory.Purchase;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Finance.Services.Interface;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Transactions;

namespace Dfinance.Finance.Vouchers
{
    public class PaymentVoucherService : IPaymentVoucherService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<PaymentVoucherService> _logger;
        private readonly IFinanceTransactionService  _transactionService;
        private readonly IMapper _mapper;
        private readonly ISettingsService _settings;
        private readonly IFinanceAdditional _additionalService;
        private readonly IFinancePaymentService _paymentService;
        private readonly IUserTrackService _userTrackService;

        public PaymentVoucherService(DFCoreContext context, IAuthService authService, ILogger<PaymentVoucherService> logger, IFinanceTransactionService transactionService, IMapper mapper,ISettingsService settings,
                                      IFinanceAdditional additionalService, IFinancePaymentService paymentService, IUserTrackService userTrackService)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
            _transactionService = transactionService;
            _mapper = mapper;
            _settings = settings;
            _additionalService = additionalService;
            _paymentService = paymentService;
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


        public CommonResponse FillAccountCode()
        {
            try
            {
                int? branchId = _authService.GetBranchId();
                string criteria = "BranchAccounts";

                object PrimaryVoucherID = null, ItemID = null, ModeID = null, TransactionID = null, partyId = null, locId = null, voucherId = null, PageID = null;
                bool IsSizeItem = false, IsMargin = false, ISTransitLoc = false, IsFinishedGood = false, IsRawMaterial = false;

                DateTime? VoucherDate = null;
                int? userId = _authService.GetId();

                var result = _context.CommandTextView
                   .FromSqlRaw($"select dbo.GetCommandText('{criteria}','{PrimaryVoucherID}','{branchId}','{partyId}','{locId}','{IsSizeItem}','{IsMargin}','{voucherId}','{ItemID}','{ISTransitLoc}','{IsFinishedGood}','{IsRawMaterial}','{ModeID}','{PageID}','{VoucherDate}','{TransactionID}','{userId}')")
                  .ToList();

                var res = result.FirstOrDefault();

                var data = _context.AccountCodesView.FromSqlRaw(res.commandText).ToList();

                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }

        }


        public CommonResponse SavePayVou(FinanceTransactionDto paymentVoucherDto, int PageId, int voucherId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!_authService.IsPageValid(PageId))
                    {
                        return PageNotValid(PageId);
                    }
                    if (!_authService.UserPermCheck(PageId, 2))
                    {
                        return PermissionDenied("Save PaymentVoucher");
                    }
                    string Status = "Approved";




                    //var PaymentDto = _mapper.Map<PaymentVoucherDto, InventoryTransactionDto>(paymentVoucherDto);

                    //if (mapdto.TransactionEntries.Advance.Count > 0)
                    //{
                    //    foreach (var advanceEntry in mapdto.TransactionEntries.Advance)
                    //    {

                    //        //if (string.IsNullOrEmpty(mapdto.Party.Id))
                    //        //{

                    //        //}
                    //    }

                    //}
                    //int? primaryVoucherID = (from v in _context.FiMaVouchers
                    //                         join pm in _context.MaPageMenus on v.Id equals pm.VoucherId
                    //                         where pm.Id == PageId
                    //                         select v.PrimaryVoucherId).FirstOrDefault() ?? 0;
                    //if ((VoucherType)primaryVoucherID.Value == VoucherType.Payment_Voucher)
                    //{
                    //    //if()
                    //    //{

                    //    //}

                    //}

                    var normalAmount = Convert.ToDecimal(paymentVoucherDto.Cash.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(paymentVoucherDto.Cheque?.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(paymentVoucherDto.Card?.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(paymentVoucherDto.Epay?.Sum(x => x.Amount) ?? 0);
                    var sumOfDebit = paymentVoucherDto.AccountDetails.Sum(accountDetail => accountDetail.Amount);


                    if (normalAmount != sumOfDebit)
                    {
                        return CommonResponse.Error("Sum of Debit and Credit entry must be equal!!");
                    }

                    //save
                    int TransId = (int)_transactionService.SaveTransaction(paymentVoucherDto, PageId, voucherId, Status).Data;

                    if (paymentVoucherDto != null)
                    {
                        _additionalService.SaveTransactionAdditional(paymentVoucherDto, TransId, voucherId);
                    }

                    if (paymentVoucherDto != null)
                    {

                        int TransEntId = (int)_paymentService.SaveTransactionEntries(paymentVoucherDto, PageId, TransId, TransId).Data;

                        if (paymentVoucherDto.BillandRef != null && paymentVoucherDto.BillandRef.Any(a => a.VID != null || a.VID != 0))
                        {
                            _transactionService.SaveVoucherAllocation(TransId, TransId, paymentVoucherDto);
                        }
                    }
                    if (paymentVoucherDto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    _logger.LogInformation("Successfully Created");
                        transaction.Commit();
                        return CommonResponse.Created("Created Successfully");


                    }
                
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transaction.Rollback();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }

        public CommonResponse UpdatePayVoucher(FinanceTransactionDto paymentVoucherDto, int PageId, int voucherId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!_authService.IsPageValid(PageId))
                    {
                        return PageNotValid(PageId);
                    }
                    if (!_authService.UserPermCheck(PageId, 2))
                    {
                        return PermissionDenied("Save PaymentVoucher");
                    }

                    string Status = "Approved";
                    var normalAmount = Convert.ToDecimal(paymentVoucherDto.Cash.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(paymentVoucherDto.Cheque?.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(paymentVoucherDto.Card?.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(paymentVoucherDto.Epay?.Sum(x => x.Amount) ?? 0);
                    var sumOfDebit = paymentVoucherDto.AccountDetails.Sum(accountDetail => accountDetail.Amount);
                    if (normalAmount != sumOfDebit)
                    {
                        return CommonResponse.Error("Sum of Debit and Credit entry must be equal!!");
                    }
                    int TransId = (int)_transactionService.SaveTransaction(paymentVoucherDto, PageId, voucherId, Status).Data;

                    if (paymentVoucherDto != null)
                    {
                        _additionalService.SaveTransactionAdditional(paymentVoucherDto, TransId, voucherId);
                    }
                    if(paymentVoucherDto != null)
                    {

                        int TransEntId = (int)_paymentService.SaveTransactionEntries(paymentVoucherDto, PageId, TransId, TransId).Data;

                        if (paymentVoucherDto.BillandRef != null && paymentVoucherDto.BillandRef.Any(a => a.VID != null || a.VID != 0))
                        {
                            _transactionService.UpdateVoucherAllocation(TransId, TransId, paymentVoucherDto);
                        }
                    }
                    if (paymentVoucherDto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    _logger.LogInformation("Successfully Updated");
                    transaction.Commit();
                    return CommonResponse.Created("Updated Successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transaction.Rollback();
                    return CommonResponse.Error(ex.Message);
                }
            }

        }

        public CommonResponse DeletePayVoucher(int TransId, int pageId)
        {
            try
            {
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 3))
                {
                    return PermissionDenied("Delete Payment Voucher");
                }
                var transid = _context.FiTransaction.Any(x => x.Id == TransId);
                if (!transid)
                {
                    return CommonResponse.NotFound("Transaction Not Found");


                }
                string criteria = "DeleteTransactions";

                _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @ID={1}",
                                       criteria, TransId);

                _logger.LogInformation("Delete successfully");
                return CommonResponse.Ok("Delete successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse GetPayVocherSettings()
        {
            string[] keys = { "MultiCurrencySupport", "DisableVoucherNo", "CostCentreSystem", "AutoEntryVoucherNo", "LeftGridStatus" , "Display CostCentre in Voucher" ,
                                "AutoCloseCashForm" , "DefaultCashSystem" , "DefaultAccount","SelectAllRefernece","OpeningDateEditable" };   //OpeningDateEditable used in openingvou

            var settings = _settings.GetAllSettings(keys).Data;
            return CommonResponse.Ok(settings);
        }

        
    }
}














//private int SaveTransactions(InventoryTransactionDto transactionDto, int PageId, int VoucherId, string Status)
//{
//    try
//    {
//        if (VoucherId == 17 || VoucherId == 23 || VoucherId == 77 && VoucherId == 76)
//        {
//            if (transactionDto.Reference.Count > 0 && transactionDto.Reference.Any(r => r.Id != null || r.Id != 0))
//            {
//                var refItemView = (List<RefItemsView>)FillReference(transactionDto.Reference).Data;
//                if (refItemView != null)
//                {
//                    foreach (var item in transactionDto.Items)
//                    {
//                        var itemQty = refItemView.Where(i => i.ItemID == item.ItemId && i.Qty < item.Qty).FirstOrDefault() ?? null;
//                        if (itemQty != null)
//                        {
//                            // var itemDetails = itemQty;
//                            return CommonResponse.Ok(itemQty.ItemName + " Item quantity greater than Reference item quantity");
//                        }
//                    }
//                }
//            }
//        }
//        int branchId = _authService.GetBranchId().Value;
//        int createdBy = _authService.GetId().Value;
//        //int SerialNo = 1;
//        bool Autoentry = false;
//        int? RefTransId = null;
//        string? ApprovalStatus = "A";
//        string ReferencesId = null;
//        //if (transactionDto .ReferenceNo != null)
//        //{
//        //    ReferencesId = transactionDto.ReferenceNo;
//        //}
//        //else if(transactionDto.)
//        //ReferencesId = string.Join(",", transactionDto.Reference.Select(popupDto => popupDto.VNo.ToString()));
//        int? partyId = null;
//        if (transactionDto.Party == null)
//            partyId = transactionDto.TransactionEntries.AccountDetails.Select(a => a.AccountId).FirstOrDefault();
//        else
//            partyId = transactionDto.Party.Id;
//        string environmentname = _environment.EnvironmentName;
//        if (transactionDto.Id == null || transactionDto.Id == 0)
//        {

//            string criteria = "InsertTransactions";

//            SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
//            {
//                Direction = ParameterDirection.Output
//            };

//            _context.Database.ExecuteSqlRaw("EXEC VoucherSP " +
//                "@Criteria={0}, @Date={1}, @EffectiveDate={2}, @VoucherID={3}, @MachineName={4}, " +
//                "@TransactionNo={5}, @IsPostDated={6}, @CurrencyID={7}, @ExchangeRate={8}, @RefPageTypeID={9}, " +
//                "@RefPageTableID={10}, @ReferenceNo={11}, @CompanyID={12}, @FinYearID={13}, @InstrumentType={14}, " +
//                "@InstrumentNo={15}, @InstrumentDate={16}, @InstrumentBank={17}, @CommonNarration={18}, @AddedBy={19}, " +
//                "@ApprovedBy={20}, @AddedDate={21}, @ApprovedDate={22}, @ApprovalStatus={23}, " +
//                "@ApproveNote={24}, @Action={25}, @Status={26}, @IsAutoEntry={27}, " +
//                "@Posted={28}, @Active={29}, @Cancelled={30}, @AccountID={31}, @Description={32}, " +
//                "@RefTransID={33}, @CostCentreID={34}, @PageID={35}, @NewID={36} OUTPUT",
//                criteria, transactionDto.Date, DateTime.Now, VoucherId, environmentname,
//                transactionDto.VoucherNo, false, transactionDto.Currency.Id, transactionDto.ExchangeRate, null, null,
//                transactionDto.ReferenceNo, branchId, null, null, null,
//                null, null, transactionDto.CommonNarration, createdBy, null, DateTime.Now, null,
//                ApprovalStatus, null, null, Status, Autoentry, true, true, false, partyId,
//                transactionDto.Description, RefTransId, transactionDto.Project.Id, PageId, newId);

//            var NewId = (int)newId.Value;
//            // transactionDto.Id = NewId;
//            return CommonResponse.Ok(NewId);
//        }

//        else
//        {
//            int Transaction = _context.FiTransaction.Where(x => x.Id == transactionDto.Id).Select(x => x.Id).FirstOrDefault();
//            if (Transaction == null)
//            {
//                return CommonResponse.NotFound();
//            }
//            string criteria = "UpdateTransactions";

//            _context.Database.ExecuteSqlRaw("EXEC VoucherSP " +
//                "@Criteria={0}, @Date={1}, @EffectiveDate={2}, @VoucherID={3}, @MachineName={4}, " +
//                "@TransactionNo={5}, @IsPostDated={6}, @CurrencyID={7}, @ExchangeRate={8}, @RefPageTypeID={9}, " +
//                "@RefPageTableID={10}, @ReferenceNo={11}, @CompanyID={12}, @FinYearID={13}, @InstrumentType={14}, " +
//                "@InstrumentNo={15}, @InstrumentDate={16}, @InstrumentBank={17}, @CommonNarration={18}, @AddedBy={19}, " +
//                "@ApprovedBy={20}, @AddedDate={21}, @ApprovedDate={22}, @ApprovalStatus={23}, " +
//                "@ApproveNote={24}, @Action={25}, @Status={26}, @IsAutoEntry={27}, " +
//                "@Posted={28}, @Active={29}, @Cancelled={30}, @AccountID={31}, @Description={32}, " +
//                "@RefTransID={33}, @CostCentreID={34}, @PageID={35}, @ID={36}",
//                criteria, transactionDto.Date, DateTime.Now, VoucherId, environmentname,
//                transactionDto.VoucherNo, false, transactionDto.Currency.Id, transactionDto.ExchangeRate, null, null,
//                transactionDto.ReferenceNo, branchId, null, null, null,
//                null, null, transactionDto.CommonNarration, createdBy, null, DateTime.Now, null,
//                ApprovalStatus, null, null, Status, Autoentry, true, true, false, partyId,
//                transactionDto.Description, RefTransId, transactionDto.Project.Id, PageId, transactionDto.Id);

//            return CommonResponse.Ok(transactionDto.Id);

//        }
//    }
//    catch (Exception ex)
//    {

//        return CommonResponse.Error(ex);
//    }
//}
