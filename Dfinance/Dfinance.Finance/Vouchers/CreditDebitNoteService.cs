using AutoMapper;
using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Text.Json;
using System.Transactions;

namespace Dfinance.Finance.Vouchers
{
    public class CreditDebitNoteService : ICreditDebitNoteService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<CreditDebitNoteService> _logger;
        private readonly IMapper _mapper;
        private readonly IFinanceTransactionService _transactionService;
        private readonly IUserTrackService _userTrackService;
        public CreditDebitNoteService(DFCoreContext context, IAuthService authService, ILogger<CreditDebitNoteService> logger, IMapper mapper, IFinancePaymentService paymentService, IFinanceTransactionService transactionService, IUserTrackService userTrackService)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _transactionService = transactionService;
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

        public CommonResponse FillParty(int voucherId)
        {
            try
            {
                int? primaryvoucherId = _context.FiMaVouchers.Where(f => f.Id == voucherId).Select(f => f.PrimaryVoucherId).FirstOrDefault();
                if ((VoucherType)primaryvoucherId == VoucherType.Sales_Return)
                {
                    int branchId = _authService.GetBranchId().Value;
                    var query = from a in _context.FiMaAccounts
                                join b in _context.FiMaBranchAccounts on a.Id equals b.AccountId
                                into ab
                                from b in ab.DefaultIfEmpty()
                                join p in _context.Parties on a.Id equals p.AccountId
                                into ap
                                from p in ap.DefaultIfEmpty()
                                where !a.IsGroup && a.AccountCategory == 1 && a.Active && b.BranchId == branchId
                                select new PopUpDto
                                {
                                    Code = a.Alias, //acccode
                                    Name = a.Name, //accname
                                    Description = (p.AddressLineOne ?? "") + (p.AddressLineTwo != null && p.City != null && p.Pobox != null ? "," : " ")
                                              + (p.AddressLineTwo ?? "") + (p.City != null && p.Pobox != null ? "," : " ")
                                              + (p.City ?? "") + (p.Pobox != null ? "," : " ")
                                              + (p.Pobox ?? ""), //address
                                    Id = a.Id
                                };
                    return CommonResponse.Ok(query);
                }

                else if ((VoucherType)primaryvoucherId == VoucherType.Purchase_Return)
                {
                    int branchId = _authService.GetBranchId().Value;
                    var query = from a in _context.FiMaAccounts
                                join b in _context.FiMaBranchAccounts on a.Id equals b.AccountId
                                into ab
                                from b in ab.DefaultIfEmpty()
                                join p in _context.Parties on a.Id equals p.AccountId
                                into ap
                                from p in ap.DefaultIfEmpty()
                                where !a.IsGroup && a.AccountCategory == 2 && a.Active && b.BranchId == branchId
                                select new PopUpDto
                                {
                                    Code = a.Alias, //acccode
                                    Name = a.Name, //accname
                                    Description = (p.AddressLineOne ?? "") + (p.AddressLineTwo != null && p.City != null && p.Pobox != null ? "," : " ")
                                              + (p.AddressLineTwo ?? "") + (p.City != null && p.Pobox != null ? "," : " ")
                                              + (p.City ?? "") + (p.Pobox != null ? "," : " ")
                                              + (p.Pobox ?? ""), //address
                                    Id = a.Id
                                };
                    return CommonResponse.Ok(query);
                }
                return CommonResponse.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
              
        }

        public CommonResponse SaveCreditDebitNote(DebitCreditDto debitCreditDto, int PageId, int VoucherId)
        {
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    int? primaryvoucherId = _context.FiMaVouchers.Where(f => f.Id == VoucherId).Select(f => f.PrimaryVoucherId).FirstOrDefault();

                    //creditnote and creditnote9
                    if ((VoucherType)primaryvoucherId == VoucherType.Sales_Return)
                    {

                        if (!_authService.IsPageValid(PageId))
                        {
                            return PageNotValid(PageId);
                        }
                        if (!_authService.UserPermCheck(PageId, 2))
                        {
                            return PermissionDenied("Save");
                        }
                        int transpayId;
                        //save
                        int transId = (int)SaveTransactions(debitCreditDto, PageId, VoucherId).Data;


                        if (debitCreditDto != null)
                            SaveTransactionAdditional(debitCreditDto, transId, VoucherId);

                        if (debitCreditDto != null)
                        { 
                            transpayId = (int)SaveTransactionEntries(debitCreditDto, PageId, transId).Data;

                            
                            if (debitCreditDto.billandRef != null && debitCreditDto.billandRef.Any(x => x.Selection == true))
                            {
                               SaveVoucherAllocation(transId, transpayId, debitCreditDto);
                            }
                        }

                        if (debitCreditDto != null)
                        {
                            _transactionService.EntriesAmountValidation(transId);
                        }

                        //usertrack
                        var primaryvoucher = _context.FiMaVouchers.Where(f => f.Id == VoucherId).FirstOrDefault();
                        decimal amount = 0;
                        var jsonCN = JsonSerializer.Serialize(debitCreditDto);
                        foreach (var usertrack in debitCreditDto.accountDetails)
                        {
                            if (usertrack.Debit.HasValue && usertrack.Debit != 0)
                            {
                                amount = usertrack.Debit.Value;
                            }
                            else
                                amount = usertrack.Credit.Value;
                        }
                        _userTrackService.AddUserActivity(debitCreditDto.VoucherNo, transId, 0, "Added", "FiTransactions", primaryvoucher.Name, amount, jsonCN);
                         
                    }



                    //Debitnote and debitnote9
                    else if ((VoucherType)primaryvoucherId == VoucherType.Purchase_Return)
                    {
                        if (!_authService.IsPageValid(PageId))
                        {
                            return PageNotValid(PageId);
                        }
                        if (!_authService.UserPermCheck(PageId, 2))
                        {
                            return PermissionDenied("Save");
                        }
                        int transpayId;
                        //save
                        int transId = (int)SaveTransactions(debitCreditDto, PageId, VoucherId).Data;


                        if (debitCreditDto != null)
                            SaveTransactionAdditional(debitCreditDto, transId, VoucherId);

                        if (debitCreditDto != null)
                        {
                            transpayId = (int)SaveTransactionEntries(debitCreditDto, PageId, transId).Data;


                            if (debitCreditDto.billandRef != null && debitCreditDto.billandRef.Any(x => x.Selection == true))
                            {
                                SaveVoucherAllocation(transId, transpayId, debitCreditDto);
                            }
                        }

                        if (debitCreditDto != null)
                        {
                            _transactionService.EntriesAmountValidation(transId);
                        }
                        //usertrack
                        var primaryvoucher = _context.FiMaVouchers.Where(f => f.Id == VoucherId).FirstOrDefault();
                        
                            decimal amount = 0;
                            var jsonCN = JsonSerializer.Serialize(debitCreditDto);
                            foreach (var usertrack in debitCreditDto.accountDetails)
                            {
                                if (usertrack.Credit.HasValue && usertrack.Credit != 0)
                                {
                                    amount = usertrack.Credit.Value;
                                }
                                else
                                    amount = usertrack.Debit.Value;
                            }
                            _userTrackService.AddUserActivity(debitCreditDto.VoucherNo, transId, 0, "Added", "FiTransactions", primaryvoucher.Name, amount, jsonCN);
                           
                        
                    }
                    _logger.LogInformation(" Successfully Created!");
                    transactionScope.Complete();
                    return CommonResponse.Created(" Successfully Created!");

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transactionScope.Dispose();  
                    return CommonResponse.Error(ex);
                }
            }
        }

        public CommonResponse UpdateCreditDebitNote(DebitCreditDto debitCreditDto, int PageId, int VoucherId)
        {
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    int? primaryvoucherId = _context.FiMaVouchers.Where(f => f.Id == VoucherId).Select(f => f.PrimaryVoucherId).FirstOrDefault();

                    //update for credit and creditnote9
                    if ((VoucherType)primaryvoucherId == VoucherType.Sales_Return)
                    {

                        if (!_authService.IsPageValid(PageId))
                        {
                            return PageNotValid(PageId);
                        }
                        if (!_authService.UserPermCheck(PageId, 3))
                        {
                            return PermissionDenied("Update");
                        }
                        int transpayId;
                        //save
                        int transId = (int)SaveTransactions(debitCreditDto, PageId, VoucherId).Data;
                        if (debitCreditDto != null)
                            SaveTransactionAdditional(debitCreditDto, transId, VoucherId);

                        var remove = _context.FiTransactionEntries.Where(r => r.TransactionId == transId).ToList();
                        _context.FiTransactionEntries.RemoveRange(remove);
                        _context.SaveChanges();
                        var del = _context.FiVoucherAllocation.Where(r => r.Vid == transId).ToList();
                        _context.FiVoucherAllocation.RemoveRange(del);
                        _context.SaveChanges();

                        if (debitCreditDto != null)
                        {
                            transpayId = (int)SaveTransactionEntries(debitCreditDto, PageId, transId).Data;
                            if (debitCreditDto.billandRef != null && debitCreditDto.billandRef.Any(x => x.Selection == true))
                            {
                                SaveVoucherAllocation(transId, transpayId, debitCreditDto);
                            }
                        }
                        //usertrack
                        
                        var primaryvoucher = _context.FiMaVouchers.Where(f => f.Id == VoucherId).FirstOrDefault();
                        decimal amount = 0;
                        var jsonCN = JsonSerializer.Serialize(debitCreditDto);
                        foreach (var usertrack in debitCreditDto.accountDetails)
                        {
                            if (usertrack.Debit.HasValue && usertrack.Debit != 0)
                            {
                                amount = usertrack.Debit.Value;
                            }
                            else
                                amount = usertrack.Credit.Value;
                        }
                        _userTrackService.AddUserActivity(debitCreditDto.VoucherNo, transId, 1, "Updated", "FiTransactions", primaryvoucher.Name, amount, jsonCN);

                    }

                    //debit and debit note 9
                    else if ((VoucherType)primaryvoucherId == VoucherType.Purchase_Return)
                    {
                        if (!_authService.IsPageValid(PageId))
                        {
                            return PageNotValid(PageId);
                        }
                        if (!_authService.UserPermCheck(PageId, 3))
                        {
                            return PermissionDenied("Update");
                        }
                        int transpayId;
                        //save
                        int transId = (int)SaveTransactions(debitCreditDto, PageId, VoucherId).Data;


                        if (debitCreditDto != null)
                            SaveTransactionAdditional(debitCreditDto, transId, VoucherId);

                        var remove = _context.FiTransactionEntries.Where(r => r.TransactionId == transId).ToList();
                        _context.FiTransactionEntries.RemoveRange(remove);
                        _context.SaveChanges();
                        var del = _context.FiVoucherAllocation.Where(r => r.Vid == transId).ToList();
                        _context.FiVoucherAllocation.RemoveRange(del);
                        _context.SaveChanges();

                        if (debitCreditDto != null)
                        {
                            transpayId = (int)SaveTransactionEntries(debitCreditDto, PageId, transId).Data;
                            if (debitCreditDto.billandRef != null && debitCreditDto.billandRef.Any(x => x.Selection == true))
                            {
                                SaveVoucherAllocation(transId, transpayId, debitCreditDto);
                            }
                        }
                        //usertrack

                        var primaryvoucher = _context.FiMaVouchers.Where(f => f.Id == VoucherId).FirstOrDefault();
                        decimal amount = 0;
                        var jsonCN = JsonSerializer.Serialize(debitCreditDto);
                        foreach (var usertrack in debitCreditDto.accountDetails)
                        {
                            if (usertrack.Debit.HasValue && usertrack.Debit != 0)
                            {
                                amount = usertrack.Debit.Value;
                            }
                            else
                                amount = usertrack.Credit.Value;
                        }
                        _userTrackService.AddUserActivity(debitCreditDto.VoucherNo, transId, 1, "Updated", "FiTransactions", primaryvoucher.Name, amount, jsonCN);
                    }
                     _logger.LogInformation(" Successfully Updated!");
                    transactionScope.Complete();
                    return CommonResponse.Created("Successfully Updated!");
                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex.Message);
                    transactionScope.Dispose();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }


        public CommonResponse DeleteDebitCreditNote(int TransId, int pageId)
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

        public CommonResponse CancelDebitCreditNote(int transId,string reason)
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
              
                return CommonResponse.Ok("Cancelled successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }


        private CommonResponse SaveTransactions(DebitCreditDto debitCreditDto, int pageId, int voucherId, bool? cancel = false)
        {
            int branchId = _authService.GetBranchId().Value;
            int userId = _authService.GetId().Value;
            int currencyId = 1;
            decimal exchangeRate = 1;
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
            if (debitCreditDto.Id == 0 || debitCreditDto.Id == null)
            {
               
                var voucherNoCheck = _context.FiTransaction.Any(t => t.VoucherId == voucherId && t.TransactionNo == debitCreditDto.VoucherNo);
                if (voucherNoCheck)
                    return CommonResponse.Error("VoucherNo already exists");
                accId = debitCreditDto.Party.Id.Value;
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
                               criteria, debitCreditDto.VoucherDate, debitCreditDto.VoucherDate, voucherId, null, debitCreditDto.VoucherNo, IsPostDated, currencyId, exchangeRate,
                               null, null, null, branchId, null, null, null, null, null, debitCreditDto.Narration, userId, null, AddedDate, null,
                               ApprovalStatus, null, null, Status, IsAutoEntry, Posted, Active, Cancelled, accId, null, null, null, pageId, newId);

                transId = (int)newId.Value;
            }
            else
            {
                accId = debitCreditDto.Party.Id.Value;
                if (cancel == true)
                {
                    Cancelled = true;
                    editedBy = userId;
                }

                int Transaction = _context.FiTransaction.Where(x => x.Id == debitCreditDto.Id).Select(x => x.Id).FirstOrDefault();
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
                          criteria, debitCreditDto.VoucherDate, debitCreditDto.VoucherDate, voucherId, null, debitCreditDto.VoucherNo, IsPostDated, currencyId, exchangeRate,
                               null, null, null, branchId, null, null, null, null, null, debitCreditDto.Narration, userId, null, AddedDate, null,
                               ApprovalStatus, null, null, Status, IsAutoEntry, Posted, Active, Cancelled, accId, null, null, null, pageId, debitCreditDto.Id);


                //return CommonResponse.Ok(debitCreditDto.Id);
                transId = (int)debitCreditDto.Id;

            }
            return CommonResponse.Ok(transId);
        }


            private CommonResponse SaveTransactionEntries(DebitCreditDto debitCreditDto, int pageId, int transactionId)
            {
            try
            {
                int insertedId = 0;
                string? Reference = null;
                int BranchID = _authService.GetBranchId().Value;
                decimal? ExchangeRate = null;
                int? CostCenterID = null;
                decimal? amt = null;
                decimal? FCAmount = null;
                string? DrCr = null;
                string? description = null;
                string? nature = "M";
                string? tranType = null;


                var voucherName = (from pageMenu in _context.MaPageMenus
                                   join voucher in _context.FiMaVouchers on pageMenu.VoucherId equals voucher.Id
                                   where pageMenu.Id == pageId
                                   select new
                                   {
                                       primaryvoucherId = voucher.PrimaryVoucherId,
                                       VoucherId = voucher.Id,
                                       VoucherName = voucher.Name
                                   }).FirstOrDefault();
                //AccountDetails
                if (debitCreditDto.accountDetails.Count > 0 && debitCreditDto.accountDetails.Any(a => a.AccountCode.Id != 0))
                {
                    
                    if ((VoucherType)voucherName.primaryvoucherId == VoucherType.Sales_Return)
                    {
                        tranType = null;
                        nature = "M";
                        DrCr = "D";
                        foreach (var acc in debitCreditDto.accountDetails.Where(a => a.AccountCode.Id != 0 && a?.AccountCode.Id != null).ToList())
                        {
                            if (acc.Debit.HasValue) { amt = acc.Debit.Value; }
                            SaveTransactionEntry(transactionId, DrCr, nature, acc.AccountCode.Id,
                               amt, null, null, null, null, ExchangeRate,
                               null, Reference, debitCreditDto.Narration, tranType, acc.DueDate, null, null);
                        }
                        //second
                        DrCr = "C";
                        var sumofdebit = debitCreditDto.accountDetails.Sum(c => c.Debit);
                        SaveTransactionEntry(transactionId, DrCr, nature, debitCreditDto.Party.Id,
                               sumofdebit, null, null, null, null, ExchangeRate,
                               null, Reference, null, tranType, null, null, null);
                    }

                    //DebitNote
                    if ((VoucherType)voucherName.primaryvoucherId == VoucherType.Purchase_Return)
                    {
                        tranType = null;
                        nature = "M";
                        DrCr = "C";
                        foreach (var acc in debitCreditDto.accountDetails.Where(a => a.AccountCode.Id != 0 && a?.AccountCode.Id != null).ToList())
                        {
                            if (acc.Credit.HasValue) { amt = acc.Credit.Value; }
                            SaveTransactionEntry(transactionId, DrCr, nature, acc.AccountCode.Id,
                               amt, null, null, null, null, ExchangeRate,
                               null, Reference, debitCreditDto.Narration, tranType, acc.DueDate, null, null);
                        }
                        //second
                        DrCr = "D";
                        var sumofcredit = debitCreditDto.accountDetails.Sum(c => c.Credit);
                        SaveTransactionEntry(transactionId, DrCr, nature, debitCreditDto.Party.Id,
                               sumofcredit, null, null, null, null, ExchangeRate,
                               null, Reference, null, tranType, null, null, null);
                    }
                }
                return CommonResponse.Ok(insertedId);
            }
            catch (Exception ex) { return CommonResponse.Error(); }


        }

        private int SaveTransactionEntry(int transactionId, string drCr, string? nature, int? accountId, decimal? amt, decimal? FCAmount, DateTime? bankDate,
            int? refPageTypeId, int? currencyId, decimal? exchangeRate, int? refPageTableID, string? referenceCode, string? description,
            string? tranType, DateTime? dueDate, int? refTransID, decimal? taxPerc)
        {
            //var transEntryID = _context.FiTransactionEntries.Where(t => t.TransactionId == transactionId).Select(t => t.Id).ToList();
            //if (transEntryID.Count == 0)
            //{
            string criteria = "InsertTransactionEntries";
            SqlParameter newIdParameter = new SqlParameter("@NewID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0},@TransactionId={1},@DrCr={2},@Nature={3}," +
           "@AccountID={4},@Amount={5},@FCAmount={6},@BankDate={7},@RefPageTypeID={8},@CurrencyID={9},@ExchangeRate={10}," +
           "@RefPageTableID={11}, @ReferenceNo={12}, @Description={13}, @TranType={14}, @DueDate={15}, @RefTransID={16}, @TaxPerc={17},@NewID={18} OUTPUT",
           criteria,
           transactionId,
           drCr,
           nature,
           accountId,
           amt,
           FCAmount,
           bankDate,
           refPageTypeId,
           currencyId,
           exchangeRate,
           refPageTableID,
           referenceCode,
           description,
           tranType,
           dueDate,
           refTransID,
           taxPerc,
           newIdParameter);
            var newId = newIdParameter.Value;
            return (int)newId;

        }


        private CommonResponse SaveTransactionAdditional(DebitCreditDto debitCreditDto, int TransId, int voucherId)
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
                null, null, debitCreditDto.Particulars.Id,//2,3,4
                null,//5
                null, null,//6,7
                null,//8
                null,//9
                null,//10
                null, null, null, null, null, null, null, null, debitCreditDto.Party.Name, null,//11,12,13,14,15,16,17,18,19,20
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
                null,
                null,//92
                null,//93
                null,//94
                null, null

                );

            return CommonResponse.Ok();


        }


        private CommonResponse SaveVoucherAllocation(int transId, int transpayId, DebitCreditDto debitCreditDto)
        {
            try
            {
                int? refTransId = null;
                string criteria = "InsertVoucherAllocation";
              
                int? veid = null;

                if (debitCreditDto.billandRef != null && debitCreditDto.billandRef.Any(a => a.Amount > 0))
                {
                    foreach (var adv in debitCreditDto.billandRef)
                    {

                        //var veid = _context.FiTransactionEntries.Where(e => e.TranType == "Party" && e.AccountId == adv.AccountID).Select(e => e.Id).FirstOrDefault();
                       
                        veid = adv.VEID;
                        
                        if (adv.VID != 0)
                        {
                            refTransId = adv.VID.Value;
                        }
                        else
                        {
                            refTransId = transpayId;
                            adv.VID = transId;
                        }
                        SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };

                        _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @VID={1}, @VEID={2}, @AccountID={3},@Amount={4},@RefTransID={5}, @NewID={6} OUTPUT",
                            criteria, adv.VID, veid, adv.AccountID, adv.Amount, transId, newId);
                    }
                }
                return CommonResponse.Ok();
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
                
            }
        }

    }
}
                



