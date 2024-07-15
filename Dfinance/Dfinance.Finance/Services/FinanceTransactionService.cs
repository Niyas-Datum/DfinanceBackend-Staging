using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Inventory.Purchase;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Services
{
    public class FinanceTransactionService : IFinanceTransactionService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;


        public FinanceTransactionService(DFCoreContext context, IAuthService authService, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _authService = authService;
            _environment = hostEnvironment;


        }

      

        //save and update
        public CommonResponse SaveTransaction(FinanceTransactionDto paymentVoucherDto, int PageId, int VoucherId, string Status)
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;
                int createdBy = _authService.GetId().Value;
                //int SerialNo = 1;
                bool Autoentry = false;
                int? RefTransId = null;
                string? ApprovalStatus = "A";
                decimal? ExchangeRate = 1;
                int? CurrencyId = null;
                int? CostCenterID = null;

                //string ReferencesId = string.Join(",", paymentVoucherDto.Reference.Select(popupDto => popupDto.VNo.ToString()));

                string environmentname = _environment.EnvironmentName;
                int? accId = null;
                DateTime? effectivedate = null;

                //  if (VoucherId == 6 || VoucherId == 1)

                int? primaryVoucherID = (from v in _context.FiMaVouchers
                                         join pm in _context.MaPageMenus on v.Id equals pm.VoucherId
                                         where pm.Id == PageId
                                         select v.PrimaryVoucherId).FirstOrDefault() ?? 0;


                if((VoucherType)primaryVoucherID == VoucherType.Contra || (VoucherType)primaryVoucherID == VoucherType.Journal )
                    accId = null;
                else
                    accId = paymentVoucherDto.AccountDetails.Select(a => a.AccountCode.Id).FirstOrDefault();
                   
                //openingvoucher
                if((VoucherType)primaryVoucherID == VoucherType.Opening_Balance)
                {
                    accId = null;
                    var date = _context.TblMaFinYear
                                .Where(d => d.Status == "R")
                                .Select(d => d.StartDate)
                                .FirstOrDefault();
                    paymentVoucherDto.VoucherDate = date;
                    CurrencyId = 1; 
                    effectivedate = date;
                    CostCenterID = null;
                }
                else { 
                    effectivedate = DateTime.Now;
                    CurrencyId = paymentVoucherDto.Currency.Id;
                    CostCenterID = paymentVoucherDto.CostCentre.Id;

                }


                if (paymentVoucherDto.Id == null || paymentVoucherDto.Id == 0)
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
                        criteria, paymentVoucherDto.VoucherDate, effectivedate, VoucherId, environmentname,
                        paymentVoucherDto.VoucherNo, false, CurrencyId, ExchangeRate, null, null,
                        paymentVoucherDto.ReferenceNo, branchId, null, null, null,
                        null, null, paymentVoucherDto.Narration, createdBy, null, DateTime.Now, null,
                        ApprovalStatus, null, null, Status, Autoentry, true, true, false, accId,
                        null, RefTransId, CostCenterID, PageId, newId);

                    var NewId = (int)newId.Value;
                    // transactionDto.Id = NewId;
                    return CommonResponse.Ok(NewId);
                }

                else
                {
                    int Transaction = _context.FiTransaction.Where(x => x.Id == paymentVoucherDto.Id).Select(x => x.Id).FirstOrDefault();
                    if (Transaction == null)
                    {
                        return CommonResponse.NotFound();
                    }
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
                        criteria, paymentVoucherDto.VoucherDate, effectivedate, VoucherId, environmentname,
                        paymentVoucherDto.VoucherNo, false, CurrencyId, ExchangeRate, null, null,
                        paymentVoucherDto.ReferenceNo, branchId, null, null, null,
                        null, null, paymentVoucherDto.Narration, createdBy, null, DateTime.Now, null,
                        ApprovalStatus, null, null, Status, Autoentry, true, true, false, accId,
                        null, RefTransId, CostCenterID, PageId, paymentVoucherDto.Id);

                    return CommonResponse.Ok(paymentVoucherDto.Id);

                }
            }
            catch (Exception ex)
            {

                return CommonResponse.Error(ex);
            }
        }
        //secondpayment
        public CommonResponse SaveTransactionPayment(InventoryTransactionDto transactionDto, int TransId, string Status, int VoucherId)
        {

            try
            {
                int branchId = _authService.GetBranchId().Value;
                int createdBy = _authService.GetId().Value;
                // int SerialNo = 1;
                bool Autoentry = false;
                int? RefTransId = null;
                string? ApprovalStatus = "A";
                //int VoucherId = 2;
                int PageId = _context.MaPageMenus.Where(p => p.VoucherId == VoucherId).Select(p => p.Id).FirstOrDefault();
                Autoentry = true;
                RefTransId = TransId;
                string ReferenceId = null;
                string environmentname = _environment.EnvironmentName;
                var payType = _context.MaMisc.Where(p => p.Id == transactionDto.FiTransactionAdditional.PayType.Id).Select(p => p.Value).FirstOrDefault();
                transactionDto.Party.Name = _context.FiMaAccounts.Where(a => a.Id == transactionDto.Party.Id).Select(a => a.Name).FirstOrDefault();
                if (transactionDto.Party.Name != Constants.CASHCUSTOMER && transactionDto.Party.Name != Constants.CASHSUPPLIER || payType == Constants.CREDIT)
                {
                    VoucherNo voucherNo = (VoucherNo)GetAutoVoucherNo(VoucherId).Data;
                    // var transaction = _mapper.Map<InventoryTransactionDto, FiTransaction>(transactionDto);

                    if (transactionDto.Id == 0 || transactionDto.Id == null)
                    {
                        string criteria = "InsertTransactions";
                        //transaction.RefTransId = RefTransId;
                        //var transDto = Converter.ToDictionary(transaction);
                        //transactionDto.Id = (int?)_repository.Save(transSpName, criteria, transDto).Data;

                        SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        // Second Execution
                        _context.Database.ExecuteSqlRaw("EXEC VoucherSP " +
                            "@Criteria={0}, @Date={1}, @EffectiveDate={2}, @VoucherID={3}, @MachineName={4}, " +
                            "@TransactionNo={5}, @IsPostDated={6}, @CurrencyID={7}, @ExchangeRate={8}, @RefPageTypeID={9}, " +
                            "@RefPageTableID={10}, @ReferenceNo={11}, @CompanyID={12}, @FinYearID={13}, @InstrumentType={14}, " +
                            "@InstrumentNo={15}, @InstrumentDate={16}, @InstrumentBank={17}, @CommonNarration={18}, @AddedBy={19}, " +
                            "@ApprovedBy={20}, @AddedDate={21}, @ApprovedDate={22}, @ApprovalStatus={23}, " +
                            "@ApproveNote={24}, @Action={25}, @Status={26}, @IsAutoEntry={27}, " +
                            "@Posted={28}, @Active={29}, @Cancelled={30}, @AccountID={31}, @Description={32}, " +
                            "@RefTransID={33}, @CostCentreID={34}, @PageID={35}, @NewID={36} OUTPUT",
                            criteria, transactionDto.Date, DateTime.Now, VoucherId, environmentname,
                            voucherNo.Result.ToString(), false, transactionDto.Currency.Id, transactionDto.ExchangeRate, null, null,
                            ReferenceId, branchId, null, null, null,
                            null, null, transactionDto.Description, createdBy, null, DateTime.Now, null,
                            ApprovalStatus, null, null, Status, Autoentry, true, true, false, transactionDto.Party.Id,
                            null, RefTransId, transactionDto.Project.Id, PageId, newId);

                        transactionDto.Id = (int)newId.Value;
                        return CommonResponse.Ok((int)newId.Value);

                    }
                    else
                    {
                        int PayId = _context.FiTransaction
                     .Where(x => x.RefTransId == transactionDto.Id)
                     .Select(x => x.Id)
                     .FirstOrDefault();
                        string criteria = "UpdateTransactions";

                        RefTransId = transactionDto.Id;
                        _context.Database.ExecuteSqlRaw("EXEC VoucherSP " +
                            "@Criteria={0}, @Date={1}, @EffectiveDate={2}, @VoucherID={3}, @MachineName={4}, " +
                            "@TransactionNo={5}, @IsPostDated={6}, @CurrencyID={7}, @ExchangeRate={8}, @RefPageTypeID={9}, " +
                            "@RefPageTableID={10}, @ReferenceNo={11}, @CompanyID={12}, @FinYearID={13}, @InstrumentType={14}, " +
                            "@InstrumentNo={15}, @InstrumentDate={16}, @InstrumentBank={17}, @CommonNarration={18}, @AddedBy={19}, " +
                            "@ApprovedBy={20}, @AddedDate={21}, @ApprovedDate={22}, @ApprovalStatus={23}, " +
                            "@ApproveNote={24}, @Action={25}, @Status={26}, @IsAutoEntry={27}, " +
                            "@Posted={28}, @Active={29}, @Cancelled={30}, @AccountID={31}, @Description={32}, " +
                            "@RefTransID={33}, @CostCentreID={34}, @PageID={35}, @ID={36}",
                            criteria, transactionDto.Date, DateTime.Now, VoucherId, environmentname,
                            transactionDto.VoucherNo, false, transactionDto.Currency.Id, transactionDto.ExchangeRate, null, null,
                            ReferenceId, branchId, null, null, null,
                            null, null, transactionDto.Description, createdBy, null, DateTime.Now, null,
                            ApprovalStatus, null, null, Status, Autoentry, true, true, false, transactionDto.Party.Id,
                            null, RefTransId, transactionDto.Project.Id, PageId, PayId);

                        transactionDto.Id = PayId;
                    }
                }
                else
                {
                    transactionDto.Id = TransId;
                }
                return CommonResponse.Ok(transactionDto.Id);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
      

        public CommonResponse SaveVoucherAllocation(int transId, int transpayId, FinanceTransactionDto paymentVoucherDto)
        {
            
            // List<int> processedReferIds = new List<int>();
            try
            {
                int? refTransId = null;
                var transEntryId = _context.FiTransactionEntries.Where(e => e.TransactionId == transId && e.TranType == "Party").FirstOrDefault();
                string criteria = "InsertVoucherAllocation";
             
                    if (paymentVoucherDto.BillandRef != null && paymentVoucherDto.BillandRef.Any(a => a.Amount > 0))
                    {
                        foreach (var adv in paymentVoucherDto.BillandRef)
                        {
                        var veid = _context.FiTransactionEntries.Where(e => e.TransactionId == transId && e.AccountId == adv.AccountID).Select(e=>e.Id).FirstOrDefault();
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

        public CommonResponse UpdateVoucherAllocation(int transId, int transpayId, FinanceTransactionDto paymentVoucherDto)
        {

            // List<int> processedReferIds = new List<int>();
            try
            {
                _context.FiVoucherAllocation.Where(v => v.RefTransId == transId).ExecuteDelete();
                SaveVoucherAllocation(transId, transpayId, paymentVoucherDto);

                return CommonResponse.Ok();

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);

            }
        }
        public CommonResponse GetAutoVoucherNo(int voucherid)
        {
            try
            {
                var voucher = _context.FiMaVouchers
                    .Where(x => x.Id == voucherid)
                    .FirstOrDefault();

                if (voucher == null)
                {
                    return CommonResponse.Error(new Exception("Voucher not found."));
                }

                int branchid = _authService.GetBranchId().Value;

                var result = _context.AccountCodeView
                    .FromSqlRaw($"EXEC GetNextAutoEntryVoucherNoSP @VoucherID={voucherid}, @BranchID={branchid}")
                    .FirstOrDefault();
                VoucherNo voucherNo = new VoucherNo
                {
                    Code = voucher.Code,
                    Result = result.ToString()
                };
                return CommonResponse.Ok(voucherNo);
            }
            catch (Exception ex)
            {

                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse EntriesAmountValidation(int TransId)
        {
            _context.Database.ExecuteSqlRaw("EXEC EntriesAmountCheck @TransactionID={0}",
                                   TransId);

            return CommonResponse.Ok();
        }
        public CommonResponse InventoryAmountValidation(int TransId)
        {
            _context.Database.ExecuteSqlRaw("EXEC InventoryAmountCheck @TransactionID={0}",
                                   TransId);

            return CommonResponse.Ok();
        }






    }
}
