using AutoMapper;
using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Text.Json;

namespace Dfinance.Finance.Vouchers
{
    public class PdcClearingService : IPdcClearingService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<PdcClearingService> _logger;
        private readonly IFinancePaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly IFinanceTransactionService _transactionService;
        private readonly IUserTrackService _userTrackService;
        public PdcClearingService(DFCoreContext context, IAuthService authService, ILogger<PdcClearingService> logger, IMapper mapper, IFinancePaymentService paymentService, IFinanceTransactionService transactionService, IUserTrackService userTrackService)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _paymentService = paymentService;
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

        public CommonResponse FillChequeDetails(int BankId)
        {
            try
            {
                string criteria = "FillPDC";
                int branchId = _authService.GetBranchId().Value;
                var result = _context.CheqDetailView.FromSqlRaw("Exec ChequeRegisterSP @Criteria={0},@BankID={1},@BranchID={2}", criteria, BankId, branchId).ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }



        public CommonResponse SavePdcClearing(PdcClearingDto PDCDto, int PageId, int voucherId)
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
                        return PermissionDenied("Save PDC Clearing");
                    }

                    //foreach (var chequeDetail in PDCDto.ChequeDetails)
                    //{
                    //    if (chequeDetail.Selection == false)
                    //    {
                    //        return CommonResponse.Error("Please select any cheque and proceed");
                    //    }
                    //}
                    //bool anySelected = false;

                    //foreach (var chequeDetail in PDCDto.ChequeDetails)
                    //{
                    //    if (chequeDetail.Selection.HasValue && chequeDetail.Selection.Value)
                    //    {
                    //        anySelected = true;
                    //        break;
                    //    }
                    //}

                    //if (!anySelected)
                    //{
                    //    return CommonResponse.Error("Please select at least one cheque and proceed.");
                    //}

                    var selectedChequeDetails = PDCDto.ChequeDetails.Where(chequeDetail => chequeDetail.Selection == true);

                    
                    if (!selectedChequeDetails.Any())
                    {
                        return CommonResponse.Error("Please select at least one cheque and proceed.");
                    }


                    decimal sumOfDebit = _context.FiTransactionEntries
                                            .Where(d => d.DrCr == "D")
                                            .Sum(d => d.Amount);
                    decimal sumOfCredit = _context.FiTransactionEntries
                                            .Where(d => d.DrCr == "C")
                                            .Sum(d => d.Amount);
                    if(sumOfDebit != sumOfCredit)
                    {
                        return CommonResponse.Error("Debit and Credit side must be equal!!");
                    }


                    int transId = (int)SaveTransactions(PDCDto, PageId, voucherId).Data;
                    if (PDCDto != null)
                        SaveTransactionEntries(PDCDto, PageId, transId);
                 
                    transaction.Commit();
                    _logger.LogInformation("PDC Clearing Saved Successfully!");
                    return CommonResponse.Created("PDC Clearing Saved Successfully!");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transaction.Rollback();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }
      
        private CommonResponse SaveTransactions(PdcClearingDto PDCDto, int pageId, int voucherId, bool? cancel = false)
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
            if (PDCDto.Id == 0 || PDCDto.Id == null)
            {
                reason = "Added";
                var voucherNoCheck = _context.FiTransaction.Any(t => t.VoucherId == voucherId && t.TransactionNo == PDCDto.VoucherNo);
                if (voucherNoCheck)
                    return CommonResponse.Error("VoucherNo already exists");
                accId = PDCDto.BankName.ID.Value;
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
                               criteria, PDCDto.VoucherDate, PDCDto.VoucherDate, voucherId, null, PDCDto.VoucherNo, IsPostDated, currencyId, exchangeRate,
                               null, null, null, branchId, null, null, null, null, null, PDCDto.Narration, userId, null, AddedDate, null,
                               ApprovalStatus, null, null, Status, IsAutoEntry, Posted, Active, Cancelled, accId, null, null, null, pageId, newId);

                transId = (int)newId.Value;

            }
            else
            {
                actionId = 1;
                reason = "Updated";
                accId = PDCDto.BankName.ID.Value;
                if (cancel == true)
                {
                    Cancelled = true;
                    editedBy = userId;
                }
               
                int Transaction = _context.FiTransaction.Where(x => x.Id == PDCDto.Id).Select(x => x.Id).FirstOrDefault();
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
                           criteria, PDCDto.VoucherDate, PDCDto.VoucherDate, voucherId, null, PDCDto.VoucherNo, IsPostDated, currencyId, exchangeRate,
                               null, null, null, branchId, null, null, null, null, null, PDCDto.Narration, userId, null, AddedDate, null,
                               ApprovalStatus, null, null, Status, IsAutoEntry, Posted, Active, Cancelled, accId, null, null, null, pageId, PDCDto.Id);


                //return CommonResponse.Ok(PDCDto.Id);
                transId = (int)PDCDto.Id;

            }
            decimal amount = 0;
            var jsonBudget = JsonSerializer.Serialize(PDCDto);
            foreach (var usertrack in PDCDto.ChequeDetails)
            {
                if (usertrack.Debit.HasValue && usertrack.Debit != 0)
                {
                    amount = usertrack.Debit.Value;
                }
                else
                    amount = usertrack.Credit.Value; 

            }
            _userTrackService.AddUserActivity(PDCDto.VoucherNo, transId, actionId, reason, "FiTransactions", "PDC Clearing", amount , jsonBudget);
            return CommonResponse.Ok(transId);
        }
        private CommonResponse SaveTransactionEntries(PdcClearingDto PDCDto, int pageId, int transactionId)
        {
            try
            {
               int insertedId = 0;
                string? Reference = null;
                int BranchID = _authService.GetBranchId().Value;
                decimal? ExchangeRate = 1;
                int? CostCenterID = null;
                decimal? amt = null;
                decimal? FCAmount = null;
                string? DrCr = null;
                string? description = null;
                string? nature = "M";

                if (PDCDto.ChequeDetails.Count > 0)
                {
                    foreach (var pdc in PDCDto.ChequeDetails)
                    {

                        if (pdc.Credit.HasValue && pdc.Credit != 0)  //credit
                        {
                            DrCr = "D";
                            amt = pdc.Credit.Value;
                            FCAmount= amt;
                             
                            if (pdc.BankName != null) 
                            {
                                description = "Drop Cash from" + " " + PDCDto.BankName.Name;
                            }
                        }
                        else if (pdc.Debit.HasValue)
                        {
                            DrCr = "C";
                            amt = pdc.Debit.Value;
                            FCAmount = amt;
                            if (pdc.BankName != null) 
                            {
                                description = "Deposite Cash to" + " " + PDCDto.BankName.Name;
                            }
                        }
                        int tranentId = _context.FiTransactionEntries
                               .Where(t => t.AccountId == pdc.AccountID && t.DrCr == "D" && t.TranType == "cheque")
                                .Select(t => t.Id)
                                .FirstOrDefault();
                        insertedId = SaveTransactionEntry(transactionId, DrCr, nature, pdc.AccountID, amt, FCAmount,null,
                        null, null, null, null, null, description,
                        null, null, tranentId, null);

                        SaveCheqTran(transactionId, insertedId, pdc.ID);

                    }
                    //second
                    DrCr = "D";
                    var issue = PDCDto.ChequeDetails.Sum(c => c.Credit);
                    var receive= PDCDto.ChequeDetails.Sum(d => d.Debit);
                    amt = issue + receive;
                    string des = PDCDto.Narration;
                    SaveTransactionEntry(transactionId, DrCr, nature, PDCDto.BankName.ID, amt, amt, null,
                       null, null, null, null, null, des,
                       null, null, null, null);
                }
                return CommonResponse.Ok(insertedId);
            }
            catch (Exception ex)
            { return CommonResponse.Error(); }
        }

            private int SaveTransactionEntry(int transactionId, string drCr, string? nature, int? accountId, decimal? amt,decimal? FCAmount, DateTime? bankDate,
            int? refPageTypeId, int? currencyId, decimal? exchangeRate, int? refPageTableID, string? referenceCode, string? description,
            string? tranType, DateTime? dueDate, int? refTransID, decimal? taxPerc)
            {
            //var transEntryID = _context.FiTransactionEntries.Where(t => t.TransactionId == transactionId).Select(t => t.Id).ToList();
            //if (transEntryID.Count == 0)
            //{
            string  criteria = "InsertTransactionEntries";
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

        private CommonResponse SaveCheqTran(int VId, int VEId,int? ChequeId)
        {
            try
            {
                string criteria = "InsertFiChequesTran";
                string trantype = "Posted";
                SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

              
                    var data = _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria ={0},@VID={1},@VEID={2},@ChequeID={3},@TranType={4},@NewID={5} OUTPUT",
                    criteria, VId, VEId, ChequeId, trantype, newIdparam);
                int NewIdUser = (int)newIdparam.Value;
                return CommonResponse.Ok("Succesfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }


     

        public CommonResponse UpdatePDCclearing(PdcClearingDto pdcClearingDto,int PageId, int voucherId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!_authService.IsPageValid(PageId))
                    {
                        return PageNotValid(PageId);
                    }
                    if (!_authService.UserPermCheck(PageId, 3))
                    {
                        return PermissionDenied("Update PDC Clearing");
                    }
                    var selectedChequeDetails = pdcClearingDto.ChequeDetails.Where(chequeDetail => chequeDetail.Selection == true);


                    if (!selectedChequeDetails.Any())
                    {
                        return CommonResponse.Error("Please select at least one cheque and proceed.");
                    }


                    //decimal sumOfDebit = _context.FiTransactionEntries
                    //                        .Where(d => d.DrCr == "D")
                    //                        .Sum(d => d.Amount);
                    //decimal sumOfCredit = _context.FiTransactionEntries
                    //                        .Where(d => d.DrCr == "C")
                    //                        .Sum(d => d.Amount);
                    //if (sumOfDebit != sumOfCredit)
                    //{
                    //    return CommonResponse.Error("Debit and Credit side must be equal!!");
                    //}

                    int transId = (int)SaveTransactions(pdcClearingDto, PageId, voucherId).Data;

                    var remove = _context.FiTransactionEntries.Where(r => r.TransactionId == transId).ToList();
                    _context.FiTransactionEntries.RemoveRange(remove);
                    _context.SaveChanges();

                    SaveTransactionEntries(pdcClearingDto, PageId, transId);

                    transaction.Commit();
                    _logger.LogInformation("Updated Sucessfully");
                    return CommonResponse.Ok("Updated!");

                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex.Message);
                    transaction.Rollback();
                    return CommonResponse.Error(ex.Message);
                }
            }

        }



    }
}

// use delete and cancel api in creditdebitnote for PDC 





//public CommonResponse DeletePdcClearing(PdcClearingDto pdcDto, int pageId, int voucherId, bool? cancel = false)
//{
//    if (!_authService.IsPageValid(pageId))
//    {
//        return PageNotValid(pageId);
//    }
//    if (!_authService.UserPermCheck(pageId, 4))
//    {
//        return PermissionDenied("Delete Pdc Clearing");
//    }
//    var transid = _context.FiTransaction.Any(x => x.Id == pdcDto.Id);
//    var jsonPDC = JsonSerializer.Serialize(pdcDto);
//    if (!transid)
//        return CommonResponse.NotFound("Transaction Not Found");

//    if (cancel == true)
//    {

//        SaveTransactions(pdcDto, pageId, voucherId, true);
//        return CommonResponse.Ok("Cancelled successfully");
//    }
//    else
//    {
//        string criteria = "DeleteTransactions";
//        _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @ID={1}", criteria, pdcDto.Id);
//        decimal amount = 0;
//        foreach (var usertrack in pdcDto.ChequeDetails)
//        {
//            if (usertrack.Debit.HasValue && usertrack.Debit != 0)
//            {
//                amount = usertrack.Debit.Value;
//            }
//            else
//                amount = usertrack.Credit.Value;

//        }
//        _userTrackService.AddUserActivity(pdcDto.VoucherNo, pdcDto.Id ?? 0, 2, "Deleted", "FiTransactions", "PDC Clearing", 0, jsonPDC);
//        _logger.LogError("Failed to Delete PDC");
//        return CommonResponse.Ok("PDC Deleted successfully");
//    }
//}



