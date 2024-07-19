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
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;
using System.Transactions;

namespace Dfinance.Finance.Vouchers
{
    public class JournalVoucherService : IJournalVoucherService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<ContraVoucherService> _logger;
        private readonly IFinanceTransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly IUserTrackService _userTrack;
        
        public JournalVoucherService(DFCoreContext context, IAuthService authService, ILogger<ContraVoucherService> logger, IFinanceTransactionService transactionService, IMapper mapper,
            IUserTrackService userTrack)
                                     
        {
            _authService = authService;
            _context = context;
            _logger = logger;
            _transactionService = transactionService;
            _mapper = mapper;
          _userTrack = userTrack;
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
        }//fills the accountdetails in grid
        public CommonResponse FillAccounts(string? criteria = null)
        {
            try
            {
                int? branchId = _authService.GetBranchId();
                //string criteria = "BranchAccounts";

                object PrimaryVoucherID = null, ItemID = null, ModeID = null, TransactionID = null, partyId = null, locId = null, voucherId = null, PageID = null;
                bool IsSizeItem = false, IsMargin = false, ISTransitLoc = false, IsFinishedGood = false, IsRawMaterial = false;

                DateTime? VoucherDate = null;
                int? userId = _authService.GetId();
                var result = _context.CommandTextView.FromSqlRaw($"select dbo.GetCommandText('{criteria}','{PrimaryVoucherID}','{branchId}','{partyId}','{locId}','{IsSizeItem}','{IsMargin}','{voucherId}','{ItemID}','{ISTransitLoc}','{IsFinishedGood}','{IsRawMaterial}','{ModeID}','{PageID}','{VoucherDate}','{TransactionID}','{userId}')")
                             .ToList();

                var res = result.FirstOrDefault();

                var data = _context.ContraAccCode.FromSqlRaw(res.commandText).ToList();

                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse SaveJournalVoucher(int pageId, int voucherId, JournalDto journalDto)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    if (!_authService.IsPageValid(pageId))
                    {
                        return PageNotValid(pageId);
                    }
                    if (!_authService.UserPermCheck(pageId, 2))
                    {
                        return PermissionDenied("Save Journal Voucher");
                    }

                    string Status = "Approved";

                    var jrnlvou = _mapper.Map<JournalDto, FinanceTransactionDto>(journalDto);

                    var TotalDebit = Convert.ToDecimal(journalDto.AccountData.Sum(x => x.Debit) ?? 0);
                    var TotalCredit = Convert.ToDecimal(journalDto.AccountData.Sum(x => x.Credit) ?? 0);
                    if (TotalDebit != TotalCredit) { return CommonResponse.Error("Debit and Credit must be tally!!"); }

                    int TransId = (int)_transactionService.SaveTransaction(jrnlvou, pageId, voucherId, Status).Data;
                    if (journalDto.AccountData != null)
                    {
                        SaveTransactionEntries(journalDto, TransId);
                    }
                    if (jrnlvou != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    var json=JsonSerializer.Serialize(journalDto);
                    _userTrack.AddUserActivity(journalDto.VoucherNo, TransId, 1, "Added", "FiTransactions", "Journal", TotalDebit, json);
                    _logger.LogInformation("Successfully Created Journal Voucher");
                    transaction.Complete();
                    return CommonResponse.Created("Successfully Created Journal Voucher");

                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to Save Journal Voucher");
                    transaction.Dispose();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }
        private CommonResponse SaveTransactionEntries(JournalDto journalDto, int transId)
        {
            if (journalDto.AccountData == null)
                return CommonResponse.NoContent();
            string tranType = null;
            string nature = "M";
            string criteria = "InsertTransactionEntries";
            string DrCr;

            foreach (var acc in journalDto.AccountData)
            {
                if (acc.Debit > 0)
                    DrCr = "D";
                else
                    DrCr = "C";

                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0},@TransactionId={1},@DrCr={2},@Nature={3}," +
               "@AccountID={4},@Amount={5},@FCAmount={6},@BankDate={7},@RefPageTypeID={8},@CurrencyID={9},@ExchangeRate={10}," +
               "@RefPageTableID={11}, @ReferenceNo={12}, @Description={13}, @TranType={14}, @DueDate={15}, @RefTransID={16}, @TaxPerc={17},@NewID={18} OUTPUT",
               criteria, transId, DrCr, nature, acc.AccountCode.Id, acc.Amount, null, null, null, journalDto.Currency.Id, journalDto.ExchangeRate,
               null, null, acc.Description, tranType, acc.DueDate, null, null, newId);

                string criteria1 = "InsertVoucherAllocation";
                int veid = (int)newId.Value;
                if (acc.BillandRef != null)
                {
                    foreach (var billRef in acc.BillandRef)
                    {
                        if (billRef.Selection == true)
                        {
                            int? vid = billRef.VID;
                            int? AccountId = billRef.AccountID;
                            SqlParameter voucherAlId = new SqlParameter("@NewID", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };

                            _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @VID={1}, @VEID={2}, @AccountID={3},@Amount={4},@RefTransID={5}, @NewID={6} OUTPUT",
                                criteria1, vid, veid, AccountId, billRef.Allocated, transId, voucherAlId);
                        }
                    }
                }
            }
            return CommonResponse.Ok();
        }
        public CommonResponse UpdateJournalVoucher(int pageId, int voucherId, JournalDto journalDto)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    if (!_authService.IsPageValid(pageId))
                    {
                        return PageNotValid(pageId);
                    }
                    if (!_authService.UserPermCheck(pageId, 3))
                    {
                        return PermissionDenied("Update Journal Voucher");
                    }
                    string Status = "Approved";

                    var jrnlvou = _mapper.Map<JournalDto, FinanceTransactionDto>(journalDto);

                    var TotalDebit = Convert.ToDecimal(journalDto.AccountData.Sum(x => x.Debit) ?? 0);
                    var TotalCredit = Convert.ToDecimal(journalDto.AccountData.Sum(x => x.Credit) ?? 0);
                    if (TotalDebit != TotalCredit) { return CommonResponse.Error("Debit and Credit must be tally!!"); }

                    int TransId = (int)_transactionService.SaveTransaction(jrnlvou, pageId, voucherId, Status).Data;
                    
                        var entryRemove = _context.FiTransactionEntries.Where(e => e.TransactionId == journalDto.Id).ToList();
                        var allocationRemove = _context.FiVoucherAllocation.Where(a => a.RefTransId == journalDto.Id).ToList();
                        if(entryRemove.Any())
                        {
                            _context.FiTransactionEntries.RemoveRange();
                            _context.SaveChanges(); 
                        } 
                        if(allocationRemove.Any())
                        {
                            _context.FiVoucherAllocation.RemoveRange();
                            _context.SaveChanges();
                        }
                    if (journalDto.AccountData != null)
                    {
                        SaveTransactionEntries(journalDto, TransId);
                    }
                    var json = JsonSerializer.Serialize(journalDto);
                    _userTrack.AddUserActivity(journalDto.VoucherNo, TransId, 1, "Updated", "FiTransactions", "Journal", TotalDebit, json);
                    _logger.LogInformation("Successfully Updated Journal Voucher");
                    transaction.Complete();
                    return CommonResponse.Ok("Journal Voucher Updated Successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to Update Journal Voucher");
                    transaction.Dispose();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }        
    }
}
