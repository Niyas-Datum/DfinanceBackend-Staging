using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Finance.Services
{
    public class FinancePaymentService : IFinancePaymentService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        private string purchaseVoucherCredit = string.Empty;
        private string purchaseVoucherDebit = string.Empty;
        private const string EXPENSE = "EXPENSE";
        private string? nature = null;
        private DateTime? bankDate = null;
        private int? refPageTypeId = null;
        private int? refPageTableId = null;
        private string? description = null;
        private string? criteria = null;
        private string? tranType = null;
        private readonly DataRederToObj _rederToObj;
        private int? discId = null;
        public FinancePaymentService(DFCoreContext context, IAuthService authService, DataRederToObj rederToObj, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _authService = authService;
            _environment = hostEnvironment;
            _rederToObj = rederToObj;
            
        }
        public CommonResponse SaveTransactionEntries(PaymentVoucherDto paymentVoucherDto, int pageId, int transactionId, int transPayId)
        {
            try
            {

                string? Reference = null;
                SetVoucherDebitCreditDetails(pageId);
                int BranchID = _authService.GetBranchId().Value;
              
                int insertedId = 0;
                var transEntry = _context.FiTransactionEntries.Any(t => t.Id == transactionId || t.Id == transPayId);
                if (transEntry)//Delete Transaction Entries
                {
                    DeleteTransEntries(transactionId, transPayId);
                }

                var partyID = paymentVoucherDto.AccountDetails.Select(a => a.AccountCode.Id).FirstOrDefault();
                decimal? ExchangeRate = 1;

                //Cash

                if (paymentVoucherDto.Cash.Count > 0)
                {
                    tranType = "Cash";
                    nature = "M";
                    foreach (var cash in paymentVoucherDto.Cash.Where(a => a.AccountCode.ID != 0 && a?.AccountCode.ID != null).ToList())
                    {
                        SaveTransactionEntries(transPayId, purchaseVoucherCredit, nature, cash.AccountCode.ID,
                        cash.Amount, bankDate ?? null, refPageTypeId, paymentVoucherDto.Currency.Id, paymentVoucherDto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, null, null, null);
                    }
                }
                //Cheque
                if (paymentVoucherDto.Cheque.Count > 0 && paymentVoucherDto.Cheque.Select(x => x.PDCPayable.ID).FirstOrDefault() != 0)
                {
                    tranType = "Cheque";
                    nature = null;
                    foreach (var cheque in paymentVoucherDto.Cheque.Where(a => a.PDCPayable.ID != 0 && a?.PDCPayable.ID != null))
                    {
                        var veId = SaveTransactionEntries(transPayId, purchaseVoucherCredit, nature, cheque.PDCPayable.ID,
                            cheque.Amount ?? null, bankDate ?? null, refPageTypeId, paymentVoucherDto.Currency.Id, paymentVoucherDto.ExchangeRate,
                            refPageTableId, Reference, description, tranType, null, null, null);
                        SaveCheque(cheque, veId, partyID);
                    }
                }
                //Card
                if (paymentVoucherDto.Card.Count > 0)
                {

                    tranType = "Card";
                    nature = "";
                    foreach (var card in paymentVoucherDto.Card.Where(a => a.AccountCode.ID != 0 && a?.AccountCode.ID != null).ToList())
                    {
                        SaveTransactionEntries(transPayId, purchaseVoucherCredit, nature, card.AccountCode.ID,
                        card.Amount, bankDate ?? null, refPageTypeId, paymentVoucherDto.Currency.Id, paymentVoucherDto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, null, null, null);
                    }
                }
                //Epay
                if (paymentVoucherDto.Epay.Count > 0)
                {
                    tranType = "Online";
                    nature = "";
                    foreach (var Epay in paymentVoucherDto.Epay.Where(a => a.AccountCode.ID != 0 && a?.AccountCode.ID != null).ToList())
                    {
                        SaveTransactionEntries(transPayId, purchaseVoucherCredit, nature, Epay.AccountCode.ID,
                       Epay.Amount, bankDate ?? null, refPageTypeId, paymentVoucherDto.Currency.Id, paymentVoucherDto.ExchangeRate,
                       refPageTableId, Reference, description, tranType, null, null, null);

                    }
                }
                //Accountdetails from payvoucher
                if (paymentVoucherDto.AccountDetails.Count > 0 && (paymentVoucherDto.AccountDetails.Any(a => a.AccountCode.Id != 0)))
                {
                    tranType = null;
                    nature = "M";
                    foreach (var acc in paymentVoucherDto.AccountDetails.Where(a => a.AccountCode.Id != 0 && a?.AccountCode.Id != null).ToList())
                    {
                        SaveTransactionEntries(transPayId, purchaseVoucherDebit, nature, acc.AccountCode.Id,
                       acc.Amount, bankDate ?? null, refPageTypeId, paymentVoucherDto.Currency.Id, ExchangeRate,
                       refPageTableId, Reference, description, tranType, null, null, null);

                    }
                }


                //var veId=_context.TransactionEntries.Where(e=>e.TransactionId == transactionId && e.TranType=="Party").Select(e=>e.Id).FirstOrDefault();
                //return veId;
                return CommonResponse.Ok(insertedId);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }

        private int SaveTransactionEntries(int transactionId, string drCr, string? nature, int? accountId, decimal? grandTotal, DateTime? bankDate,
            int? refPageTypeId, int? currencyId, decimal? exchangeRate, int? refPageTableID, string? referenceCode, string? description,
            string? tranType, DateTime? dueDate, int? refTransID, decimal? taxPerc)
        {
            var teId = _context.FiTransactionEntries.Where(te => te.TransactionId == transactionId && te.TranType == tranType && te.Id != discId).Select(t => t.Id).FirstOrDefault();
            if (teId == null || teId == 0)
            {
                criteria = "InsertTransactionEntries";
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
               grandTotal,
               grandTotal,
               bankDate,
               this.refPageTypeId,
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
            else
            {
                // _context.FiTransactionEntries.Where(e => e.Id == teId).ExecuteDelete();
                criteria = "DeleteTransactionEntries";
                _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0},@ID={1}", criteria, teId);
                var cheq = _context.fiCheques.Where(c => c.Veid == teId);
                if (cheq.Any())
                {
                    _context.fiCheques.RemoveRange(cheq);
                    _context.SaveChanges();
                }
                return SaveTransactionEntries(transactionId, drCr, nature, accountId, grandTotal, bankDate,
            refPageTypeId, currencyId, exchangeRate, refPageTableID, referenceCode, description,
            tranType, dueDate, refTransID, taxPerc);
                // criteria = "UpdateTransactionEntries";
                // _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0},@TransactionId={1},@DrCr={2},@Nature={3}," +
                //"@AccountID={4},@Amount={5},@FCAmount={6},@BankDate={7},@RefPageTypeID={8},@CurrencyID={9},@ExchangeRate={10}," +
                //"@RefPageTableID={11}, @ReferenceNo={12}, @Description={13}, @TranType={14}, @DueDate={15}, @RefTransID={16}, @TaxPerc={17},@ID={18}",
                //criteria,
                //transactionId,
                //drCr,
                //nature,
                //accountId,
                //grandTotal,
                //grandTotal,
                //bankDate,
                //this.refPageTypeId,
                //currencyId,
                //exchangeRate,
                //refPageTableID,
                //referenceCode,
                //description,
                //tranType,
                //dueDate,
                //refTransID,
                //taxPerc,
                //teId);                
                // return (int)teId;
            }


        }

        private void SetVoucherDebitCreditDetails(int pageId)
        {
            int? primaryVoucherID = (from v in _context.FiMaVouchers
                                     join pm in _context.MaPageMenus on v.Id equals pm.VoucherId
                                     where pm.Id == pageId
                                     select v.PrimaryVoucherId).FirstOrDefault() ?? 0;
            switch ((VoucherType)primaryVoucherID)
            {
                
                case VoucherType.Payment_Voucher:
                    purchaseVoucherCredit = "C";
                    purchaseVoucherDebit = "D";
                    break;
                
            }
        }

        private void DeleteTransEntries(int transactionId, int transPayId)
        {
            var transexp = _context.TransExpense.Where(t => t.TransactionId == transactionId);
            if (transexp.Any())
            {
                _context.TransExpense.RemoveRange(transexp);
                _context.SaveChanges();
            }

            var transItemexp = _context.TransItemExpenses.Where(t => t.TransactionId == transactionId);
            if (transItemexp.Any())
            {
                _context.TransItemExpenses.RemoveRange(transItemexp);
                _context.SaveChanges();
            }

            var transEntry = _context.FiTransactionEntries.Where(e => e.TransactionId == transactionId);
            _context.FiTransactionEntries.RemoveRange(transEntry);
            _context.SaveChanges();
            if (transPayId != transactionId)
            {
                var transPayEntry = _context.FiTransactionEntries.Where(e => e.TransactionId == transPayId);
                _context.FiTransactionEntries.RemoveRange(transPayEntry);
                _context.SaveChanges();
            }
        }


        public CommonResponse SaveCheque(ChequeDto chequeDto, int VEId, int? PartyId)
        {
            try
            {
                var checkId = _context.fiCheques.Any(c => c.Veid == VEId);
                if (checkId == false)
                {
                    string criteria = "InsertCheques";
                    SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    var data = _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria ={0},@VEID={1},@CardType={2},@Commission={3},@ChequeNo={4},@ChequeDate={5},@ClrDays={6},@BankID={7},@BankName={8},@Status={9},@PartyID={10},@NewID={11} OUTPUT", criteria,
                        VEId, chequeDto.CardType, chequeDto.Commission, chequeDto.ChequeNo, chequeDto.ChequeDate, chequeDto.ClearingDays, chequeDto.BankID, chequeDto.BankName.Name, chequeDto.Status, PartyId, newIdparam);
                    int NewIdUser = (int)newIdparam.Value;
                }
                else
                {
                    _context.fiCheques.Where(c => c.Veid == VEId).ExecuteDelete();
                    SaveCheque(chequeDto, VEId, PartyId);
                }
                return CommonResponse.Ok("Succesfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse FillCheque()
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;
                var cheque = from a in _context.FiMaAccounts
                             join b in _context.FiMaBranchAccounts on a.Id equals b.AccountId
                             join l in _context.FiAccountsList on a.Id equals l.AccountId
                             join ml in _context.FiMaAccountsLists on l.ListId equals ml.Id
                             where b.BranchId == branchId && a.IsGroup == false && a.Active == true && (ml.Description == "PDC Received" || ml.Description == "PDC Issued" || ml.Description == "Bank")
                             select new AccountNamePopUpDto
                             {
                                 Alias = a.Alias,
                                 Name = a.Name,
                                 ID = a.Id
                             };

                return CommonResponse.Ok(cheque.ToList());
            }
            catch (Exception ex) { return CommonResponse.Error(ex); }

        }
        public CommonResponse FillBankName()
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;
                var query = from a in _context.FiMaAccounts
                            join b in _context.FiMaBranchAccounts on a.Id equals b.AccountId
                            join l in _context.FiAccountsList on a.Id equals l.AccountId
                            join ml in _context.FiMaAccountsLists on l.ListId equals ml.Id
                            where b.BranchId == branchId && a.IsGroup == false && a.Active == true && (ml.Description == "Bank")
                            select new AccountNamePopUpDto
                            {
                                Alias = a.Alias,
                                Name = a.Name,
                                ID = a.Id
                            };

                return CommonResponse.Ok(query.ToList());
            }
            catch (Exception ex) { return CommonResponse.Error(ex); }

        }
        public CommonResponse FillCash()
        {
            try
            {
                var cash = from fa in _context.FiMaAccounts
                           join al in _context.FiAccountsList on fa.Id equals al.AccountId
                           join mal in _context.FiMaAccountsLists on al.ListId equals mal.Id
                           where fa.IsGroup == false && fa.Active == true &&
                           mal.Description == "CASH"
                           select new AccountNamePopUpDto
                           {
                               Alias = fa.Alias,
                               Name = fa.Name,
                               ID = fa.Id
                           };

                return CommonResponse.Ok(cash.ToList());

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public CommonResponse FillCard()
        {
            try
            {
                var card = from fa in _context.FiMaAccounts
                           join al in _context.FiAccountsList on fa.Id equals al.AccountId
                           join mal in _context.FiMaAccountsLists on al.ListId equals mal.Id
                           where fa.IsGroup == false && fa.Active == true &&
                           mal.Description == "CARD"
                           select new AccountNamePopUpDto
                           {
                               Alias = fa.Alias,
                               Name = fa.Name,
                               ID = fa.Id
                           };

                return CommonResponse.Ok(card.ToList());

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CommonResponse FillEpay()
        {
            try
            {
                var Epay = from fa in _context.FiMaAccounts
                           join al in _context.FiAccountsList on fa.Id equals al.AccountId
                           join mal in _context.FiMaAccountsLists on al.ListId equals mal.Id
                           where fa.IsGroup == false && fa.Active == true &&
                           mal.Description == "EPAY"
                           select new AccountNamePopUpDto
                           {
                               Alias = fa.Alias,
                               Name = fa.Name,
                               ID = fa.Id
                           };

                return CommonResponse.Ok(Epay.ToList());

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }

        }

        public CommonResponse FillAdvance(int AccountId, string Drcr, DateTime? date)
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;
                string Criteria = "GetBillsAndRefs";
                var data = _context.FillAdvanceView
                    .FromSqlRaw($"Exec VoucherSP @Criteria='{Criteria}', @AccountID='{AccountId}',@BranchID='{branchId}',@DrCr='{Drcr}',@Date='{date}'")
                    .ToList();
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return CommonResponse.Error("");
            }

        }

    }
}












//int? netAmtAcId = null;
////var voucherName = (from pageMenu in _context.MaPageMenus
//                   join voucher in _context.FiMaVouchers on pageMenu.VoucherId equals voucher.Id
//                   where pageMenu.Id == pageId
//                   select new
//                   {
//                       VoucherId = voucher.Id,
//                       VoucherName = voucher.Name
//                   }).FirstOrDefault();

//int? discountId = null;
//netAmtAcId = null;
//int? roundOffId = null;
//_context.Database.OpenConnection();

//using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
//{
//    dbCommand.CommandText = $"EXEC FillPartyDetailsSP @BranchID={BranchID},@VoucherName='{voucherName.VoucherName}'";
//    SqlDataAdapter da = new SqlDataAdapter((SqlCommand)dbCommand);
//    DataSet dataSet = new DataSet();
//    da.Fill(dataSet);
//    var disc = dataSet.Tables[0].Rows[0];
//    discountId = Convert.ToInt32(disc["ID"]);
//    var net = dataSet.Tables[2].Rows[0];
//    netAmtAcId = Convert.ToInt32(net["ID"]);//_rederToObj.Deserialize<TransAccount>((DbDataReader)reader[2]).Select(d => d.ID).FirstOrDefault();
//    var round = dataSet.Tables[4].Rows[0];                                  //var table4 = _rederToObj.Deserialize<TransAccount>(reader).Select(d => d.ID).FirstOrDefault();
//    roundOffId = Convert.ToInt32(round["ID"]);
//}






////RoundOff
//if (transactionDto.TransactionEntries.Roundoff > 0)
//{
//    tranType = "RoundOff";
//    nature = null;
//    // var roundOffId = _context.FiMaAccounts.Where(a => a.Name == ROUNDOFF).Select(a => a.Id).FirstOrDefault();
//    SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, roundOffId,
//        transactionDto.TransactionEntries.Roundoff ?? null, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
//        refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);
//}
////RoundOff
//if (transactionDto.TransactionEntries.TotalDisc > 0)
//{
//    tranType = null;
//    nature = null;
//    //var discountId = _context.FiMaAccounts.Where(a => a.Name == DISCOUNT).Select(a => a.Id).FirstOrDefault();
//    discId = SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, discountId,
//        transactionDto.TransactionEntries.TotalDisc ?? null, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
//        refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);
//}
////Tax
//if (transactionDto.TransactionEntries.Tax.Count > 0)
//{
//    tranType = "Tax";
//    nature = null;
//    foreach (var tax in transactionDto.TransactionEntries.Tax.Where(a => a.AccountCode.ID != 0 && a?.AccountCode.ID != null).ToList())
//    {
//        SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, tax.AccountCode.ID,
//            tax.Amount ?? null, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
//            refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);
//    }
//}
////AddCharges
//if (transactionDto.TransactionEntries != null && transactionDto.TransactionEntries.AddCharges != null && transactionDto.TransactionEntries.AddCharges.Count > 0)
//{
//    tranType = "Expense";
//    nature = null;
//    foreach (var expanse in transactionDto.TransactionEntries.AddCharges.Where(a => a.AccountCode.ID != 0 && a?.AccountCode.ID != null).ToList())
//    {
//        SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, expanse.AccountCode.ID,
//        expanse.Amount ?? null, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
//        refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);
//    }
//}
//if (transactionDto.Items.Count > 0)
//{
//    tranType = null;
//    nature = "M";
//    var grossAmount = transactionDto.Items.Sum(a => a.GrossAmt);
//    SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, netAmtAcId,
//        grossAmount ?? null, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
//        refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);

//}
//var payType = _context.MaMisc.Where(p => p.Id == transactionDto.FiTransactionAdditional.PayType.Id).Select(p => p.Value).FirstOrDefault();
//transactionDto.Party.Name = _context.FiMaAccounts.Where(a => a.Id == transactionDto.Party.Id).Select(a => a.Name).FirstOrDefault();
//if (transactionDto.Party.Name != Constants.CASHCUSTOMER && transactionDto.Party.Name != Constants.CASHSUPPLIER || payType == Constants.CREDIT)
//{
//    //GrandTotal - PartyEntry
//    if (transactionDto.TransactionEntries.GrandTotal > 0)
//    {
//        tranType = "Party";
//        nature = "M";
//        insertedId = SaveTransactionEntries(transactionId, purchaseVoucherCredit, nature, transactionDto.Party.Id,
//         transactionDto.TransactionEntries.GrandTotal ?? null, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
//         refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);

//    }
//    //Payment Transaction
//    if (transactionDto.TransactionEntries.Cash.Count > 0 || transactionDto.TransactionEntries.Cheque.Count > 0 || transactionDto.TransactionEntries.Card.Count > 0)
//    {
//        tranType = "Normal";
//        nature = "M";
//        //var normalAmount = Convert.ToDecimal(transactionDto.TransactionEntries.Cash.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(transactionDto?.TransactionEntries?.Cheque?.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(transactionDto?.TransactionEntries?.Card?.Sum(x => x.Amount) ?? 0);
//        if (transactionDto.TransactionEntries.TotalPaid > 0)
//            SaveTransactionEntries(transPayId, purchaseVoucherDebit, nature, transactionDto.Party.Id,
//                           transactionDto.TransactionEntries.TotalPaid, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
//                           refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);
//    }
//}