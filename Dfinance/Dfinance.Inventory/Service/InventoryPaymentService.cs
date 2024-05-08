﻿using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;

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
        private const string Asset = "ASSET";
        private const string EXPENSE = "EXPENSE";
        private string? nature = null;
        private DateTime? bankDate = null;
        private int? refPageTypeId = null;
        private int? refPageTableId = null;
        private string? description = null;
        private string? criteria = null;
        private string? tranType = null;
        public InventoryPaymentService(DFCoreContext context, IAuthService authService, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _authService = authService;
            _environment = hostEnvironment;

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
        public CommonResponse SaveTransactionEntries(PurchaseDto purchaseDto, int pageId, int transactionId, int transPayId)
        {
            try
            {
                var Reference = "null";
                SetVoucherDebitCreditDetails(pageId);
                int insertedId = 0;
                //RoundOff
                if (purchaseDto.TransactionEntries.Roundoff > 0)
                {
                    tranType = "RoundOff";
                    nature = "NULL";
                    SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, purchaseDto.Supplier.Id,
                        purchaseDto.TransactionEntries.Roundoff ?? null, bankDate ?? null, refPageTypeId, purchaseDto.Currency.Id, purchaseDto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, purchaseDto.TransactionEntries.DueDate, null, null);
                }
                //Tax
                if (purchaseDto.TransactionEntries.Tax.Count > 0)
                {
                    tranType = "Tax";
                    nature = "NULL";
                    foreach (var tax in purchaseDto.TransactionEntries.Tax.Where(a => a.TransType == tranType).ToList())
                    {
                        SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, tax.AccountId,
                            tax.Amount ?? null, bankDate ?? null, refPageTypeId, purchaseDto.Currency.Id, purchaseDto.ExchangeRate,
                            refPageTableId, Reference, description, tranType, purchaseDto.TransactionEntries.DueDate, null, null);
                    }
                }
                //AddCharges
                if (purchaseDto.TransactionEntries != null && purchaseDto.TransactionEntries.AddCharges != null && purchaseDto.TransactionEntries.AddCharges.Count > 0)
                {
                    tranType = "Expense";
                    nature = "NULL";
                    foreach (var expanse in purchaseDto.TransactionEntries.AddCharges.Where(a => a.TransType == tranType).ToList())
                    {
                        SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, expanse.AccountId,
                        expanse.Amount ?? null, bankDate ?? null, refPageTypeId, purchaseDto.Currency.Id, purchaseDto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, purchaseDto.TransactionEntries.DueDate, null, null);
                    }
                }
                //GrandTotal - PartyEntry
                if (purchaseDto.TransactionEntries.GrandTotal > 0)
                {
                    tranType = "Party";
                    nature = "M";
                    insertedId = SaveTransactionEntries(transactionId, purchaseVoucherCredit, nature, purchaseDto.Supplier.Id,
                         purchaseDto.TransactionEntries.GrandTotal ?? null, bankDate ?? null, refPageTypeId, purchaseDto.Currency.Id, purchaseDto.ExchangeRate,
                         refPageTableId, Reference, description, tranType, purchaseDto.TransactionEntries.DueDate, null, null);

                }
                //Advance
                if (purchaseDto.TransactionEntries.Advance.Count > 0)
                {
                    tranType = "Normal";
                    nature = "M";
                    foreach (var advance in purchaseDto.TransactionEntries.Advance.Where(a => a.Selection == true).ToList())
                    {
                        SaveTransactionEntries(transPayId, purchaseVoucherDebit, nature, purchaseDto.Supplier.Id,
                        advance.Amount ?? null, bankDate ?? null, refPageTypeId, purchaseDto.Currency.Id, purchaseDto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, purchaseDto.TransactionEntries.DueDate, null, null);
                    }
                }
                //Cash
                if (purchaseDto.TransactionEntries.Cash.Count > 0)
                {
                    tranType = "Cash";
                    nature = "M";
                    foreach (var cash in purchaseDto.TransactionEntries.Cash.Where(a => a.TransType == tranType).ToList())
                    {
                        SaveTransactionEntries(transPayId, purchaseVoucherCredit, nature, cash.AccountId ,
                        cash.Amount, bankDate ?? null, refPageTypeId, purchaseDto.Currency.Id, purchaseDto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, purchaseDto.TransactionEntries.DueDate, null, null);
                    }
                }
                //Cheque
                if (purchaseDto.TransactionEntries.Cheque.Count > 0 && purchaseDto.TransactionEntries.Cheque.Select(x => x.AccountId).FirstOrDefault() != 0)
                {
                    tranType = "Cheque";
                    nature = "M";
                    foreach (var cheque in purchaseDto.TransactionEntries.Cheque)
                    {
                        SaveTransactionEntries(transPayId, purchaseVoucherCredit, nature, cheque.AccountId ,
                            cheque.Amount ?? null, bankDate ?? null, refPageTypeId, purchaseDto.Currency.Id, purchaseDto.ExchangeRate,
                            refPageTableId, Reference, description, tranType, purchaseDto.TransactionEntries.DueDate, null, null);
                    }
                }
                //Card
                if (purchaseDto.TransactionEntries.Card.Count > 0)
                {
                    tranType = "Card";
                    nature = "M";
                    foreach (var card in purchaseDto.TransactionEntries.Card.Where(a => a.TransType == tranType).ToList())
                    {
                        SaveTransactionEntries(transPayId, purchaseVoucherCredit, nature, card.AccountId,
                        card.Amount, bankDate ?? null, refPageTypeId, purchaseDto.Currency.Id, purchaseDto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, purchaseDto.TransactionEntries.DueDate, null, null);
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
                          where fa.IsGroup == false && fa.Active == true &&
                                sg.MajorGroup == Asset && ba.BranchId == branchId
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

        public CommonResponse SaveCheque(ChequesDto chequeDto, int VEId, int PartyId, string Status)
        {
            try
            {
                string criteria = "InsertCheques";
                SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                var data = _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria ={0},@VEID={1},@CardType={2},@Commission={3},@ChequeNo={4},@ChequeDate={5},@ClrDays={6},@BankID={7},@BankName={8},@Status={9},@PartyID={10},@NewID={11} OUTPUT", criteria,
                    VEId, chequeDto.CardType, chequeDto.Commission, chequeDto.ChequeNo, chequeDto.ChequeDate, chequeDto.ClrDays, chequeDto.BankID, chequeDto.BankName.Name, Status, PartyId, newIdparam);
                int NewIdUser = (int)newIdparam.Value;
                return CommonResponse.Ok("Inserted Succesfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        //************SaveTransactionExpenses**********************
        public CommonResponse SaveTransactionExpenses(List<AccountDetailsDto> accountDetailsDtos, int transactionId, string tranType)
        {
            try
            {
                foreach (var accountDetailsDto in accountDetailsDtos)
                {
                    int? entryId = _context.TransactionEntries.Where(i => i.TransactionId == transactionId && i.TranType == tranType).
                  Select(i => i.Id).
                  SingleOrDefault();
                    entryId = entryId == 0 ? null : entryId;
                    int? TeId = _context.TransactionExpense.Where(i => i.TransactionId == transactionId && i.AccountId == accountDetailsDto.AccountId).
                           Select(i => i.Id).
                           SingleOrDefault();
                    TeId = TeId == 0 ? null : TeId;
                    int? chargeTypeId = null;
                    // tranType = "ItemExpense";
                    if (TeId != null)
                    {
                        criteria = "UpdateTransactionExpenses";
                        _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0},@TransactionID={1},@AccountID={2}," +
                            "@Description={3},@Amount={4},@PayableAccountID={5},@ChargeTypeID={6},@VEID={7},@PreCalculatedAmt={8},@TranType={9},@ID={10} ",
                            criteria,
                            transactionId,
                            accountDetailsDto.AccountId,
                            accountDetailsDto.Description,
                            accountDetailsDto.Amount ?? 0,
                            accountDetailsDto?.PayableAccount?.ID ?? 0,
                            chargeTypeId,
                            entryId,
                            accountDetailsDto?.Amount ?? null,
                            tranType,
                            TeId);
                    }

                    else
                    {
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
                            accountDetailsDto?.PayableAccount?.ID ?? 0,
                            chargeTypeId,
                            entryId,
                            accountDetailsDto?.Amount ?? null,
                            tranType,
                            newIdParameter);

                    }
                }

                return CommonResponse.Ok("Success");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error("Not saved");
            }

        }

        public CommonResponse SaveTransCollections(TransactionEntries transactionEntries, int transactionId)
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
        public CommonResponse SaveTransCollectionsAllocation(TransactionEntries transactionEntries, int transCollectionId, int vAllocId)
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