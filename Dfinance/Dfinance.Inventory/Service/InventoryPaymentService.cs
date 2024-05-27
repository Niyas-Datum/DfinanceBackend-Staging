using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Finance;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace Dfinance.Inventory.Service
{


    public class InventoryPaymentService : IInventoryPaymentService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;
        //  private readonly ILogger<PurchaseService> _logger;
        private string purchaseVoucherCredit = string.Empty;
        private string purchaseVoucherDebit = string.Empty;
        // private const string Asset = "ASSET";
        private const string EXPENSE = "EXPENSE";
        //private const string ROUNDOFF = "Round Off Account";
        //private const string DISCOUNT = "Discount Allowed";
        private string? nature = null;
        private DateTime? bankDate = null;
        private int? refPageTypeId = null;
        private int? refPageTableId = null;
        private string? description = null;
        private string? criteria = null;
        private string? tranType = null;
        private readonly DataRederToObj _rederToObj;
        public InventoryPaymentService(DFCoreContext context, IAuthService authService, DataRederToObj rederToObj, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _authService = authService;
            _environment = hostEnvironment;
            _rederToObj = rederToObj;
            //_logger = logger;
        }
        /// <summary>
        /// Save tranentries frm purchase
        /// </summary>
        /// <param name="purchaseDto"></param>
        /// <param name="pageId"></param>
        /// <param name="transactionId"></param>
        /// <param name="transPayId"></param>
        /// <returns></returns>
        public CommonResponse SaveTransactionEntries(InventoryTransactionDto transactionDto, int pageId, int transactionId, int transPayId)
        {
            try
            {
               
                string? Reference = null;
                SetVoucherDebitCreditDetails(pageId);
                int BranchID = _authService.GetBranchId().Value;
                int? netAmtAcId = null;
                var voucherName = (from pageMenu in _context.MaPageMenus
                                   join voucher in _context.FiMaVouchers on pageMenu.VoucherId equals voucher.Id
                                   where pageMenu.Id == pageId
                                   select new
                                   {
                                       VoucherId = voucher.Id,
                                       VoucherName = voucher.Name
                                   }).FirstOrDefault();

                int? discountId = null;
                netAmtAcId = null;
                int? roundOffId = null;
                _context.Database.OpenConnection();

                using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandText = $"EXEC FillPartyDetailsSP @BranchID={BranchID},@VoucherName='{voucherName.VoucherName}'";
                    SqlDataAdapter da=new SqlDataAdapter((SqlCommand)dbCommand);
                    DataSet dataSet = new DataSet();
                    da.Fill(dataSet);
                    var disc = dataSet.Tables[0].Rows[0];
                        discountId = Convert.ToInt32(disc["ID"]);
                    var net = dataSet.Tables[2].Rows[0];
                    netAmtAcId = Convert.ToInt32(net["ID"]);//_rederToObj.Deserialize<TransAccount>((DbDataReader)reader[2]).Select(d => d.ID).FirstOrDefault();
                    var round = dataSet.Tables[4].Rows[0];                                  //var table4 = _rederToObj.Deserialize<TransAccount>(reader).Select(d => d.ID).FirstOrDefault();
                    roundOffId = Convert.ToInt32(round["ID"]); 
                }
               
                int insertedId = 0;
                //RoundOff
                if (transactionDto.TransactionEntries.Roundoff > 0)
                {
                    tranType = "RoundOff";
                    nature = null;
                    // var roundOffId = _context.FiMaAccounts.Where(a => a.Name == ROUNDOFF).Select(a => a.Id).FirstOrDefault();
                    SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, roundOffId,
                        transactionDto.TransactionEntries.Roundoff ?? null, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);
                }
                //RoundOff
                if (transactionDto.TransactionEntries.TotalDisc > 0)
                {
                    tranType = null;
                    nature = null;
                    //var discountId = _context.FiMaAccounts.Where(a => a.Name == DISCOUNT).Select(a => a.Id).FirstOrDefault();
                    SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, discountId,
                        transactionDto.TransactionEntries.TotalDisc ?? null, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);
                }
                //Tax
                if (transactionDto.TransactionEntries.Tax.Count > 0)
                {
                    tranType = "Tax";
                    nature = null;
                    foreach (var tax in transactionDto.TransactionEntries.Tax.Where(a => a.AccountId != 0 && a?.AccountId != null).ToList())
                    {
                        SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, tax.AccountId,
                            tax.Amount ?? null, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
                            refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);
                    }
                }
                //AddCharges
                if (transactionDto.TransactionEntries != null && transactionDto.TransactionEntries.AddCharges != null && transactionDto.TransactionEntries.AddCharges.Count > 0)
                {
                    tranType = "Expense";
                    nature = null;
                    foreach (var expanse in transactionDto.TransactionEntries.AddCharges.Where(a => a.AccountId != 0 && a?.AccountId != null).ToList())
                    {
                        SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, expanse.AccountId,
                        expanse.Amount ?? null, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);
                    }
                }
                if (transactionDto.Items.Count > 0)
                {
                    tranType = null;
                    nature = "M";
                    var grossAmount = transactionDto.Items.Sum(a => a.GrossAmt);
                    SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, netAmtAcId,
                        grossAmount ?? null, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);

                }
                //GrandTotal - PartyEntry
                if (transactionDto.TransactionEntries.GrandTotal > 0)
                {
                    tranType = "Party";
                    nature = "M";
                    insertedId = SaveTransactionEntries(transactionId, purchaseVoucherCredit, nature, transactionDto.Party.Id,
                         transactionDto.TransactionEntries.GrandTotal ?? null, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
                         refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);

                }
                //Payment Transaction
                if (transactionDto.TransactionEntries.Cash.Count > 0 || transactionDto.TransactionEntries.Cheque.Count > 0 || transactionDto.TransactionEntries.Card.Count > 0)
                {
                    tranType = "Normal";
                    nature = "M";
                    var normalAmount = transactionDto.TransactionEntries.Cash.Sum(x => x.Amount) ?? 0 + transactionDto?.TransactionEntries?.Cheque?.Sum(x => x.Amount) ?? 0 + transactionDto?.TransactionEntries?.Card?.Sum(x => x.Amount) ?? 0;

                    SaveTransactionEntries(transPayId, purchaseVoucherDebit, nature, transactionDto.Party.Id,
                    normalAmount
                    , bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
                    refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);

                }
                //Cash

                if (transactionDto.TransactionEntries.Cash.Count > 0)
                {
                    tranType = "Cash";
                    nature = "M";
                    foreach (var cash in transactionDto.TransactionEntries.Cash.Where(a => a.AccountId != 0 && a?.AccountId != null).ToList())
                    {
                        SaveTransactionEntries(transPayId, purchaseVoucherCredit, nature, cash.AccountId,
                        cash.Amount, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);
                    }
                }
                //Cheque
                if (transactionDto.TransactionEntries.Cheque.Count > 0 && transactionDto.TransactionEntries.Cheque.Select(x => x.PDCPayable.ID).FirstOrDefault() != 0)
                {
                    tranType = "Cheque";
                    nature = "M";
                    foreach (var cheque in transactionDto.TransactionEntries.Cheque.Where(a => a.PDCPayable.ID != 0 && a?.PDCPayable.ID != null))
                    {
                        var veId = SaveTransactionEntries(transPayId, purchaseVoucherCredit, nature, cheque.PDCPayable.ID,
                            cheque.Amount ?? null, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
                            refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);
                        SaveCheque(cheque, veId, transactionDto.Party.Id);
                    }
                }
                //Card
                if (transactionDto.TransactionEntries.Card.Count > 0)
                {

                    tranType = "Card";
                    nature = "M";
                    foreach (var card in transactionDto.TransactionEntries.Card.Where(a => a.AccountId != 0 && a?.AccountId != null).ToList())
                    {
                        SaveTransactionEntries(transPayId, purchaseVoucherCredit, nature, card.AccountId,
                        card.Amount, bankDate ?? null, refPageTypeId, transactionDto.Currency.Id, transactionDto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, transactionDto.TransactionEntries.DueDate, null, null);
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
            var teId = _context.FiTransactionEntries.Where(te => te.TransactionId == transactionId && te.TranType == tranType).Select(t => t.Id).FirstOrDefault();
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
                _context.FiTransactionEntries.Where(e => e.Id == teId).ExecuteDelete();
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
                case VoucherType.Purchase:
                case VoucherType.Sales_Return:
                case VoucherType.Service:
                    purchaseVoucherCredit = "C";
                    purchaseVoucherDebit = "D";
                    break;
                case VoucherType.Sales_Invoice:
                case VoucherType.Purchase_Return:
                case VoucherType.Delivery:
                case VoucherType.Service_Invoice:
                case VoucherType.Service_Delivery_Out:
                    purchaseVoucherCredit = "D";
                    purchaseVoucherDebit = "C";
                    break;
                case VoucherType.Opening_Stock:
                    break;
            }
        }
        /// <summary>
        /// Fill Tax
        /// </summary>
        /// <returns></returns>
        public CommonResponse FillTax()
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;
                var tax = from fa in _context.FiMaAccounts
                          join ba in _context.FiMaBranchAccounts on fa.Id equals ba.AccountId
                          join sg in _context.FiMaSubGroups on fa.SubGroup equals sg.Id
                          where fa.IsGroup == false && fa.Active == true && sg.Description == "Duties And Taxes" &&
                              ba.BranchId == branchId
                          select new
                          {
                              Id = fa.Id,
                              Alias = fa.Alias,
                              Name = fa.Name
                          };

                return CommonResponse.Ok(tax);
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
        public CommonResponse FillAddCharge()
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;
                var addcharge = from fa in _context.FiMaAccounts
                                join ba in _context.FiMaBranchAccounts on fa.Id equals ba.AccountId
                                join sg in _context.FiMaSubGroups on fa.SubGroup equals sg.Id
                                where fa.IsGroup == false && fa.Active == true &&
                                sg.MajorGroup == EXPENSE && ba.BranchId == branchId
                                select new AccountNamePopUpDto
                                {
                                    Alias = fa.Alias,
                                    Name = fa.Name,
                                    ID = fa.Id
                                };
                return CommonResponse.Ok(addcharge.ToList());
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        public CommonResponse SaveCheque(InvChequesDto chequeDto, int VEId, int? PartyId)
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
                        VEId, chequeDto.CardType, chequeDto.Commission, chequeDto.ChequeNo, chequeDto.ChequeDate, chequeDto.ClrDays, chequeDto.BankID, chequeDto.BankName.Name, chequeDto.Status, PartyId, newIdparam);
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

        //************SaveTransactionExpenses**********************
        public CommonResponse SaveTransactionExpenses(List<InvAccountDetailsDto> accountDetailsDtos, int transactionId, string tranType)
        {
            try
            {
                foreach (var accountDetailsDto in accountDetailsDtos)
                {
                    int? entryId = _context.FiTransactionEntries.Where(i => i.TransactionId == transactionId && i.TranType == tranType).
                  Select(i => i.Id).
                  SingleOrDefault();
                    entryId = entryId == 0 ? null : entryId;
                    int? TeId = _context.TransExpense.Where(i => i.TransactionId == transactionId && i.AccountId == accountDetailsDto.AccountId).
                           Select(i => i.Id).
                           SingleOrDefault();
                    TeId = TeId == 0 ? null : TeId;
                    int? chargeTypeId = null;


                    criteria = "InsertTransactionExpenses";
                    SqlParameter newIdParameter = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0},@TransactionID={1},@AccountID={2}," +
                        "@Description={3},@Amount={4},@PayableAccountID={5},@ChargeTypeID={6},@VEID={7},@PreCalculatedAmt={8},@TranType={9},@NewID={10} OUTPUT",
                        criteria,
                        transactionId,
                        accountDetailsDto.AccountId,
                        accountDetailsDto.Description,
                        accountDetailsDto.Amount ?? 0,
                        accountDetailsDto?.PayableAccount?.ID == 0 ? null : accountDetailsDto?.PayableAccount?.ID,
                        chargeTypeId,
                        entryId == 0 ? null : entryId,
                        accountDetailsDto.Amount ?? 0,
                        tranType,
                        newIdParameter);

                    //  }
                }

                return CommonResponse.Ok("Success");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error("Not saved");
            }
        }
        public CommonResponse UpdateTransactionExpenses(List<InvAccountDetailsDto> accountDetailsDtos, int transactionId, string tranType)
        {
            try
            {
                var removeExp = _context.TransExpense.Where(t => t.TransactionId == transactionId && t.TranType == tranType).ToList();
                _context.TransExpense.RemoveRange(removeExp);
                _context.SaveChanges();
                SaveTransactionExpenses(accountDetailsDtos, transactionId, tranType);
                return CommonResponse.Ok();
            }
            catch (Exception ex)
            {
                return CommonResponse.Error("Not saved");
            }
        }
        public CommonResponse SaveTransCollections(InvTransactionEntriesDto transactionEntries, int transactionId)
        {
            int? tcId = _context.TransCollections.Where(t => t.TransactionId == transactionId).Select(t => t.Id).FirstOrDefault();
            tcId = tcId == 0 ? null : tcId;
            var InstrumentType = _context.MaMisc.Where(m => m.Key == "InstrumentType" && m.Active == true).Select(m => m.Id).FirstOrDefault();

            if (tcId == null)
            {
                criteria = "InsertTransCollections";
                SqlParameter newIdParameter = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0},@TransactionID={1},@PayTypeID={2}," +
                    "@Amount={3},@Description={4},@DueDate={5},@InstrumentBank={6},@InstrumentDate={7},@InstrumentNo={8},@InstrumentTypeID={9}, @NewID={10} OUTPUT",
                    criteria,
                    transactionId,
                    transactionEntries.PayType.Id,
                    transactionEntries.Amt,
                    null,//Description
                    transactionEntries.DueDate,
                    null,//InstrumentBank
                    null,//InstrumentDate
                    null,//InstrumentNo
                    InstrumentType,
                    newIdParameter);
            }
            else
            {
                criteria = "UpdateTransCollections";
                _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0},@TransactionID={1},@PayTypeID={2}," +
                    "@Amount={3},@Description={4},@DueDate={5},@InstrumentBank={6},@InstrumentDate={7},@InstrumentNo={8},@InstrumentTypeID={9},@ID={10} ",
                    criteria,
                    transactionId,
                    transactionEntries.PayType.Id,
                    transactionEntries.Amt,
                    null,//Description
                    transactionEntries.DueDate,
                    null,//InstrumentBank
                    null,//InstrumentDate
                     null,//InstrumentNo
                    InstrumentType,
                    tcId);
            }
            return CommonResponse.Ok("Success");
        }
        public CommonResponse SaveTransCollectionsAllocation(InvTransactionEntriesDto transactionEntries, int transCollectionId, int vAllocId)
        {
            int? tcId = _context.TransCollnAllocations.Where(t => t.TransCollectionId == transCollectionId && t.VallocationId == vAllocId).Select(t => t.Id).FirstOrDefault();
            tcId = tcId == 0 ? null : tcId;
            if (tcId == null)
            {
                criteria = "InsertTransCollnAllocations";
                SqlParameter newIdParameter = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0},@TransCollectionID={1},@VAllocationID={2},@Amount={3},@Description={4},@NewID={5} OUTPUT",
                    criteria,
                    transCollectionId,
                    vAllocId,
                    transactionEntries.Amt,
                    null,//Description
                    newIdParameter);
            }
            else
            {
                criteria = "UpdateTransCollnAllocations";
                _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0},@TransCollectionID={1},@VAllocationID={2},@Amount={3},@Description={4},@ID={5}",
                    criteria,
                    transCollectionId,
                    vAllocId,
                    transactionEntries.Amt,
                    null,//Description
                    tcId);
            }
            return CommonResponse.Ok("Success");
        }
    }
}