using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Item.Services.Inventory.Interface;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Dfinance.Warehouse.Services.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dfinance.Purchase.Services
{
    public class PurchaseWithoutTaxService : IPurchaseWithoutTaxService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<PurchaseOrderService> _logger;
        private readonly IInventoryTransactionService _transactionService;
        private readonly IInventoryAdditional _additionalService;
        private readonly IInventoryItemService _itemService;
        private readonly IInventoryPaymentService _paymentService;
        private readonly DataRederToObj _rederToObj;
        private readonly IItemMasterService _item;
        private readonly IWarehouseService _warehouse;
        private readonly ICostCentreService _costCentre;
        private readonly IUserTrackService _userTrack;
        private readonly IHostEnvironment _environment;

        private bool grandTotalVerify = false;
        private bool autoUpdateNewVoucherNo = false;
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

        private int? discId = null;
        public PurchaseWithoutTaxService(DFCoreContext context, IAuthService authService, ILogger<PurchaseOrderService> logger, IInventoryTransactionService transactionService, IInventoryAdditional inventoryAdditional,
             IInventoryItemService inventoryItemService, IInventoryPaymentService inventoryPaymentService, DataRederToObj rederToObj, IItemMasterService item,
             IWarehouseService warehouse, ICostCentreService costCentre, IUserTrackService userTrack, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
            _transactionService = transactionService;
            _additionalService = inventoryAdditional;
            _itemService = inventoryItemService;
            _paymentService = inventoryPaymentService;
            _rederToObj = rederToObj;
            _item = item;
            _warehouse = warehouse;
            _costCentre = costCentre;
            _userTrack = userTrack;
            _environment = hostEnvironment;
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
        //public CommonResponse FillMaVouchers(int pageId,int voucherId)
        //{

        //}

        //fills Type dropdown, PriceCategory popup, Account popup
        public CommonResponse GetData(int voucherId)
        {
            string criteria = "FillSalePurchaseMode";
            var type = _context.DropDownViewValue.FromSqlRaw("exec DropDownListSP @Criteria={0},@IntParam={1}", criteria, voucherId).ToList();
            var priceCat = _context.MaPriceCategory.Where(p => p.Active == true).Select(p => new { p.Id, p.Name, p.Perc }).ToList();
            var taxFormDet = _context.DropDownViewName.FromSqlRaw("Exec  DropDownListSP @Criteria='FillTaxFormDetails'").ToList();
            var account = _context.FiMaAccounts
                          .Join(_context.FiMaSubGroups,
                                a => a.SubGroup,
                                sg => sg.Id,
                                (a, sg) => new { Account = a, SubGroup = sg })
                          .Join(_context.FiMaBranchAccounts,
                                a => a.Account.Id,
                                b => b.AccountId,
                                (a, b) => new { Account = a.Account, SubGroup = a.SubGroup, BranchAccount = b })
                          .Where(joined => joined.Account.IsGroup == false &&
                                           joined.Account.Active == true &&
                                           joined.BranchAccount.BranchId == 1)
                          .Select(joined => new
                          {
                              AccountCode = joined.Account.Alias,
                              AccountName = joined.Account.Name,
                              ID = joined.Account.Id
                          })
                          .ToList();
            return CommonResponse.Ok(new { Type = type, PriceCategory = priceCat, TaxFormDetails = taxFormDet, Account = account });
        }


        public CommonResponse SavePurchaseWithoutTax(PurchaseWithoutTaxDto dto, int pageId, int voucherId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 2))
            {
                return PermissionDenied("Save Purchase Without Tax");
            }
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    int TransId = (int)SaveTransactions(dto, pageId, voucherId).Data;
                    //int TransId = 0;
                    int? transpayId = 0;
                    if (dto.TransactionEntries.Cash.Count > 0 || dto.TransactionEntries.Cheque.Count > 0 || dto.TransactionEntries.Card.Count > 0)
                    {
                        transpayId = (int)SaveTransactionPayment(dto, TransId, 2).Data;
                    }
                    if (dto.TransactionAdditional != null)
                    {
                        SaveTransactionAdditional(dto.TransactionAdditional , TransId, voucherId);
                    }
                    if (dto.References.Count > 0 && dto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = dto.References.Select(x => x.Id).ToList();

                        _transactionService.SaveTransReference(TransId, referIds);
                    }
                    if (dto.Items != null)
                    {
                        _itemService.SaveInvTransItems(dto.Items, voucherId, TransId, dto.ExchangeRate, dto.TransactionAdditional.Warehouse.Id);
                    }
                    if (dto.TransactionEntries != null)
                    {
                        int TransEntId = (int)SaveTransactionEntries(dto, pageId, TransId, transpayId ?? 0).Data;

                        if (dto.TransactionEntries.Advance != null && dto.TransactionEntries.Advance.Any(a => a.VID != null || a.VID != 0) || transpayId != TransId)
                        {
                            _transactionService.SaveVoucherAllocation(TransId, transpayId ?? 0, dto.TransactionEntries);
                        }
                    }
                    if (dto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    transactionScope.Complete();
                    _logger.LogInformation("Purchase Without Tax Saved Successfully");
                    return CommonResponse.Created("Purchase Without Tax Saved Successfully");
                }
                catch
                {
                    transactionScope.Dispose();
                    _logger.LogError("Failed to Save Purchase Without Tax");
                    transactionScope.Dispose();
                    return CommonResponse.Error("Failed to Save Purchase Without Tax");
                }
            }           
        }
        public CommonResponse UpdatePurchaseWithoutTax(PurchaseWithoutTaxDto dto, int pageId, int voucherId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 3))
            {
                return PermissionDenied("Save Purchase Without Tax");
            }
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    int TransId = (int)SaveTransactions(dto, pageId, voucherId).Data;
                    //int TransId = 0;
                    int? transpayId = 0;
                    if (dto.TransactionEntries.Cash.Count > 0 || dto.TransactionEntries.Cheque.Count > 0 || dto.TransactionEntries.Card.Count > 0)
                    {
                        transpayId = (int)SaveTransactionPayment(dto, TransId, 2).Data;
                    }
                    if (dto.TransactionAdditional != null)
                    {
                        SaveTransactionAdditional(dto.TransactionAdditional, TransId, voucherId);
                    }
                    if (dto.References.Count > 0 && dto.References.Select(x => x.Id).FirstOrDefault() != 0)
                    {
                        List<int?> referIds = dto.References.Select(x => x.Id).ToList();

                        _transactionService.UpdateTransReference(TransId, referIds);
                    }
                    if (dto.Items != null)
                    {
                        _itemService.UpdateInvTransItems(dto.Items, voucherId, TransId, dto.ExchangeRate, dto.TransactionAdditional.Warehouse.Id);
                    }
                    if (dto.TransactionEntries != null)
                    {
                        int TransEntId = (int)SaveTransactionEntries(dto, pageId, TransId, transpayId ?? 0).Data;

                        if (dto.TransactionEntries.Advance != null && dto.TransactionEntries.Advance.Any(a => a.VID != null || a.VID != 0) || transpayId != TransId)
                        {
                            _transactionService.UpdateVoucherAllocation(TransId, transpayId ?? 0, dto.TransactionEntries);
                        }
                    }
                    if (dto != null)
                    {
                        _transactionService.EntriesAmountValidation(TransId);
                    }
                    transactionScope.Complete();
                    _logger.LogInformation("Purchase Without Tax Updated Successfully");
                    return CommonResponse.Created("Purchase Without Tax Updated Successfully");
                }
                catch
                {
                    transactionScope.Dispose();
                    _logger.LogError("Failed to Update Purchase Without Tax");
                    transactionScope.Dispose();
                    return CommonResponse.Error("Failed to Update Purchase Without Tax");
                }
            }            
        }
        public CommonResponse SaveTransactions(PurchaseWithoutTaxDto dto, int pageId, int voucherId)
        {
            var primaryVoucherId = _transactionService.GetPrimaryVoucherID(voucherId);
            var totalAmt = dto.Items.Sum(i => i.Total);
            if (dto.TransactionEntries != null)
            {
                var grandTotal = Convert.ToDecimal(dto.TransactionEntries.GrandTotal) + Convert.ToDecimal(dto.TransactionEntries.TotalDisc) - Convert.ToDecimal(dto.TransactionEntries.AddCharges.Sum(a => a.Amount));
                if (grandTotalVerify && totalAmt - grandTotal >= 1 && grandTotal - totalAmt >= 1)// Grand Total Verify settings 
                {
                    return CommonResponse.Error("There is an error in GrandTotal");
                }
            }
            if (autoUpdateNewVoucherNo && dto.Id == null)
            {
                if (dto.FiTransactionAdditional.PayType != null)
                    dto.VoucherNo = (string?)_transactionService.GetAutoVoucherNo(voucherId, dto.FiTransactionAdditional.PayType.Id).Data;
            }
            int branchId = _authService.GetBranchId().Value;
            int createdBy = _authService.GetId().Value;
            var jsonData = JsonSerializer.Serialize(dto);
           
            bool Autoentry = false;
            int? RefTransId = null;
            string? ApprovalStatus = "A";
            string Status = "Approved";
            string? ReferencesId = null;
            if (dto.References != null)
                ReferencesId = string.Join(",", dto.References.Select(popupDto => popupDto.VNo.ToString()));
            string environmentname = _environment.EnvironmentName;
            if (dto.Id == null || dto.Id == 0)
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
                    criteria, dto.Date, DateTime.Now, voucherId, environmentname,
                    dto.VoucherNo, false, dto.Currency?.Id ?? null, dto.ExchangeRate, null, null,
                    ReferencesId, branchId, null, null, null,
                        null, null, dto.Description, createdBy, null, DateTime.Now, null,
                        ApprovalStatus, null, null, Status, Autoentry, true, true, false, dto.Party.Id,
                        null, RefTransId, dto.Project?.Id ?? null, pageId, newId);

                var NewId = (int)newId.Value;
                _userTrack.AddUserActivity(dto.VoucherNo, NewId, 0, "Added", "FiTransactions", "Purchase", dto.TransactionEntries.GrandTotal??0, jsonData);              
                return CommonResponse.Ok(NewId);
            }
            else
            {
                int Transaction = _context.FiTransaction.Where(x => x.Id == dto.Id).Select(x => x.Id).FirstOrDefault();
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
                    criteria, dto.Date, DateTime.Now, voucherId, environmentname,
                    dto.VoucherNo, false, dto.Currency?.Id ?? null, dto.ExchangeRate, null, null,
                    ReferencesId, branchId, null, null, null,
                    null, null, dto.Description, createdBy, null, DateTime.Now, null,
                    ApprovalStatus, null, null, Status, Autoentry, true, true, false, dto.Party.Id,
                    null, RefTransId, dto.Project?.Id ?? null, pageId, dto.Id);

                _userTrack.AddUserActivity(dto.VoucherNo, dto.Id??0, 1, "Updated", "FiTransactions", "Purchase", dto.TransactionEntries.NetAmount ?? 0, jsonData);
                return CommonResponse.Ok(dto.Id);
            }
        }
        public CommonResponse SaveTransactionPayment(PurchaseWithoutTaxDto dto, int TransId, int voucherId)
        {
            int branchId = _authService.GetBranchId().Value;
            int createdBy = _authService.GetId().Value;           
            bool Autoentry = false;
            int? RefTransId = null;
            string? ApprovalStatus = "A";            
            int PageId = _context.MaPageMenus.Where(p => p.VoucherId == voucherId).Select(p => p.Id).FirstOrDefault();
            Autoentry = true;
            RefTransId = TransId;
            string ReferenceId = null;
            string Status = "Approved";
            string environmentname = _environment.EnvironmentName;
            var jsonData = JsonSerializer.Serialize(dto);
            var payType = _context.MaMisc.Where(p => p.Id == dto.TransactionAdditional.PayType.Id).Select(p => p.Value).FirstOrDefault();
            dto.Party.Name = _context.FiMaAccounts.Where(a => a.Id == dto.Party.Id).Select(a => a.Name).FirstOrDefault();
            if (dto.Party.Name != Constants.CASHCUSTOMER && dto.Party.Name != Constants.CASHSUPPLIER || payType == Constants.CREDIT)
            {
                VoucherNo voucherNo = (VoucherNo)_transactionService.GetAutoVoucherNo(voucherId).Data;

                if (dto.Id == 0 || dto.Id == null)
                {
                    string criteria = "InsertTransactions";


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
                           criteria, dto.Date, DateTime.Now, voucherId, environmentname,
                           dto.VoucherNo, false, dto.Currency?.Id ?? null, dto.ExchangeRate, null, null,
                           ReferenceId, branchId, null, null, null,
                           null, null, dto.Description, createdBy, null, DateTime.Now, null,
                           ApprovalStatus, null, null, Status, Autoentry, true, true, dto.Cancelled, dto.Party.Id,
                           dto.Description, RefTransId, dto.Project?.Id ?? null, PageId, newId);

                   // dto.Id = (int)newId.Value;
                    _userTrack.AddUserActivity(dto.VoucherNo, (int)newId.Value, 0, "Added", "FiTransactions", "Purchase", dto.TransactionEntries.GrandTotal ?? 0, jsonData);
                    return CommonResponse.Ok((int)newId.Value);
                }
                else
                {
                    int PayId = _context.FiTransaction
                 .Where(x => x.RefTransId == dto.Id)
                 .Select(x => x.Id)
                 .FirstOrDefault();
                    string criteria = "UpdateTransactions";

                    RefTransId = dto.Id;
                    _context.Database.ExecuteSqlRaw("EXEC VoucherSP " +
                        "@Criteria={0}, @Date={1}, @EffectiveDate={2}, @VoucherID={3}, @MachineName={4}, " +
                        "@TransactionNo={5}, @IsPostDated={6}, @CurrencyID={7}, @ExchangeRate={8}, @RefPageTypeID={9}, " +
                        "@RefPageTableID={10}, @ReferenceNo={11}, @CompanyID={12}, @FinYearID={13}, @InstrumentType={14}, " +
                        "@InstrumentNo={15}, @InstrumentDate={16}, @InstrumentBank={17}, @CommonNarration={18}, @EditedBy={19}, " +
                        "@ApprovedBy={20}, @EditedDate={21}, @ApprovedDate={22}, @ApprovalStatus={23}, " +
                        "@ApproveNote={24}, @Action={25}, @Status={26}, @IsAutoEntry={27}, " +
                        "@Posted={28}, @Active={29}, @Cancelled={30}, @AccountID={31}, @Description={32}, " +
                        "@RefTransID={33}, @CostCentreID={34}, @PageID={35}, @ID={36}",
                        criteria, dto.Date, DateTime.Now, voucherId, environmentname,
                        dto.VoucherNo, false, dto.Currency?.Id ?? null, dto.ExchangeRate, null, null,
                        ReferenceId, branchId, null, null, null,
                        null, null, dto.Description, createdBy, null, DateTime.Now, null,
                        ApprovalStatus, null, null, Status, Autoentry, true, true, false, dto.Party.Id,
                        null, RefTransId, dto.Project?.Id ?? null, PageId, PayId);

                    dto.Id = PayId;
                    _userTrack.AddUserActivity(dto.VoucherNo, PayId , 1, "Updated", "FiTransactions", "Purchase", dto.TransactionEntries.NetAmount ?? 0, jsonData);
                }
            }
            else
            {
                dto.Id = TransId;
            }
            return CommonResponse.Ok(dto.Id);
        }
        private bool SetVoucherDebitCreditDetails(int pageId)
        {
            int? primaryVoucherID = (from v in _context.FiMaVouchers
                                     join pm in _context.MaPageMenus on v.Id equals pm.VoucherId
                                     where pm.Id == pageId
                                     select v.PrimaryVoucherId).FirstOrDefault() ?? 0;
            switch ((VoucherType)primaryVoucherID)
            {
                case VoucherType.Purchase:               
                    purchaseVoucherCredit = "C";
                    purchaseVoucherDebit = "D";                  
                    break;
                case VoucherType.Sales_Invoice:               
                    purchaseVoucherCredit = "D";
                    purchaseVoucherDebit = "C";                   
                    break;
                
            }
            var financeUpdate = _context.FiMaVouchers.Where(v => v.PrimaryVoucherId == primaryVoucherID).Select(v => v.FinanceUpdate).FirstOrDefault();
            return Convert.ToBoolean(financeUpdate);
        }
        private void DeleteTransEntries(int transactionId, int transPayId)
        {
            var transexp = _context.TransExpense.Where(t => t.TransactionId == transactionId).ToList();
            if (transexp.Any())
            {
                _context.TransExpense.RemoveRange(transexp);
                _context.SaveChanges();
            }

            var transItemexp = _context.TransItemExpenses.Where(t => t.TransactionId == transactionId).ToList();
            if (transItemexp.Any())
            {
                _context.TransItemExpenses.RemoveRange(transItemexp);
                _context.SaveChanges();
            }

            var teidVoucher = _context.FiTransactionEntries.Where(t => t.TransactionId == transactionId && t.TranType == "Party").Select(c => c.Id).FirstOrDefault();
            var voucherAlloc = _context.FiVoucherAllocation.Where(c => c.Vid == transactionId || c.Vid == transPayId).ToList();
            if (voucherAlloc.Any())
            {
                _context.FiVoucherAllocation.RemoveRange(voucherAlloc);
                _context.SaveChanges();
            }
            var transEntry = _context.FiTransactionEntries.Where(e => e.TransactionId == transactionId).ToList();
            _context.FiTransactionEntries.RemoveRange(transEntry);
            _context.SaveChanges();

            if (transPayId != transactionId)
            {
                var teid = _context.FiTransactionEntries.Where(t => t.TransactionId == transPayId && t.TranType == "Cheque").Select(c => c.Id).ToList();
                var cheques = _context.fiCheques.Where(c => teid.Contains(c.Veid)).ToList();
                if (cheques.Any())
                {
                    _context.fiCheques.RemoveRange(cheques);
                    _context.SaveChanges();
                }

                var transPayEntry = _context.FiTransactionEntries.Where(e => e.TransactionId == transPayId).ToList();
                _context.FiTransactionEntries.RemoveRange(transPayEntry);
                _context.SaveChanges();
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
            }
        }

        public CommonResponse SaveTransactionEntries(PurchaseWithoutTaxDto dto, int pageId, int transactionId, int transPayId)
        {
            try
            {
                string? Reference = null;
                string? VoucherName = "";
                var financeUpdate = SetVoucherDebitCreditDetails(pageId);

                int BranchID = _authService.GetBranchId().Value;
                int? netAmtAcId = dto.Account.Id;
                var voucherName = (from pageMenu in _context.MaPageMenus
                                   join voucher in _context.FiMaVouchers on pageMenu.VoucherId equals voucher.Id
                                   where pageMenu.Id == pageId
                                   select new
                                   {
                                       VoucherId = voucher.Id,
                                       VoucherName = voucher.Name,
                                       PrimaryVoucherId=voucher.PrimaryVoucherId
                                   }).FirstOrDefault();
                if ((VoucherType)voucherName.PrimaryVoucherId == VoucherType.SalesB2B || (VoucherType)voucherName.PrimaryVoucherId == VoucherType.SalesB2C)
                    VoucherName = "Sales Invoice";
                if ((VoucherType)voucherName.PrimaryVoucherId == VoucherType.Purchase_Without_Tax)
                    VoucherName = "Purchase";
               
                int? discountId = null;
                //netAmtAcId = null;
                int? roundOffId = null;
                _context.Database.OpenConnection();
                using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandText = $"EXEC FillPartyDetailsSP @BranchID={BranchID},@VoucherName='{VoucherName}'";
                    SqlDataAdapter da = new SqlDataAdapter((SqlCommand)dbCommand);
                    DataSet dataSet = new DataSet();
                    da.Fill(dataSet);
                    var disc = dataSet.Tables[0].Rows[0];
                    discountId = Convert.ToInt32(disc["ID"]);
                    var net = dataSet.Tables[2].Rows[0];
                    if (netAmtAcId == null || netAmtAcId == 0)
                        netAmtAcId = Convert.ToInt32(net["ID"]);
                    var round = dataSet.Tables[4].Rows[0];
                    roundOffId = Convert.ToInt32(round["ID"]);
                }
                int insertedId = 0;
                var transEntry = _context.FiTransactionEntries.Any(t => t.TransactionId == transactionId || t.TransactionId == transPayId);
                if (transEntry)//Delete Transaction Entries
                {
                    DeleteTransEntries(transactionId, transPayId);
                }
                //RoundOff
                if (dto.TransactionEntries.Roundoff != 0)
                {
                    tranType = "RoundOff";
                    nature = null;
                    // var roundOffId = _context.FiMaAccounts.Where(a => a.Name == ROUNDOFF).Select(a => a.Id).FirstOrDefault();
                    SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, roundOffId,
                        dto.TransactionEntries.Roundoff ?? null, bankDate ?? null, refPageTypeId, dto.Currency.Id, dto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, dto.TransactionEntries.DueDate, null, null);
                }
                //RoundOff
                if (dto.TransactionEntries.TotalDisc > 0)
                {
                    tranType = null;
                    nature = null;
                    //var discountId = _context.FiMaAccounts.Where(a => a.Name == DISCOUNT).Select(a => a.Id).FirstOrDefault();
                    discId = SaveTransactionEntries(transactionId, purchaseVoucherCredit, nature, discountId,
                        dto.TransactionEntries.TotalDisc ?? null, bankDate ?? null, refPageTypeId, dto.Currency.Id, dto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, dto.TransactionEntries.DueDate, null, null);
                }
                //Tax
                if (dto.TransactionEntries.Tax.Count > 0)
                {
                    tranType = "Tax";
                    nature = null;
                    foreach (var tax in dto.TransactionEntries.Tax.Where(a => a.AccountCode.ID != 0 && a?.AccountCode.ID != null).ToList())
                    {
                        SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, tax.AccountCode.ID,
                            tax.Amount ?? null, bankDate ?? null, refPageTypeId, dto.Currency.Id, dto.ExchangeRate,
                            refPageTableId, Reference, tax.Description, tranType, dto.TransactionEntries.DueDate, null, null);
                    }
                }
                //AddCharges
                if (dto.TransactionEntries != null && dto.TransactionEntries.AddCharges != null && dto.TransactionEntries.AddCharges.Count > 0)
                {
                    tranType = "Expense";
                    nature = null;
                    foreach (var expanse in dto.TransactionEntries.AddCharges.Where(a => a.AccountCode.ID != 0 && a?.AccountCode.ID != null).ToList())
                    {
                        SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, expanse.AccountCode.ID,
                        expanse.Amount ?? null, bankDate ?? null, refPageTypeId, dto.Currency.Id, dto.ExchangeRate,
                        refPageTableId, Reference, expanse.Description, tranType, dto.TransactionEntries.DueDate, null, null);
                    }
                }
                if (dto.Items.Count > 0)
                {
                    tranType = null;
                    nature = "M";
                    var grossAmount = dto.Items.Sum(a => a.GrossAmt);
                    SaveTransactionEntries(transactionId, purchaseVoucherDebit, nature, netAmtAcId,
                        grossAmount ?? null, bankDate ?? null, refPageTypeId, dto.Currency.Id, dto.ExchangeRate,
                        refPageTableId, Reference, description, tranType, dto.TransactionEntries.DueDate, null, null);

                }
                var payType = _context.MaMisc.Where(p => p.Id == dto.TransactionAdditional.PayType.Id).Select(p => p.Value).FirstOrDefault();
                dto.Party.Name = _context.FiMaAccounts.Where(a => a.Id == dto.Party.Id).Select(a => a.Name).FirstOrDefault();
                if (dto.Party.Name != Constants.CASHCUSTOMER && dto.Party.Name != Constants.CASHSUPPLIER || payType == Constants.CREDIT)
                {
                    //GrandTotal - PartyEntry
                    if (dto.TransactionEntries.GrandTotal > 0)
                    {
                        tranType = "Party";
                        nature = "M";
                        insertedId = SaveTransactionEntries(transactionId, purchaseVoucherCredit, nature, dto.Party.Id,
                         dto.TransactionEntries.GrandTotal ?? null, bankDate ?? null, refPageTypeId, dto.Currency.Id, dto.ExchangeRate,
                         refPageTableId, Reference, description, tranType, dto.TransactionEntries.DueDate, null, null);

                    }
                    //Payment Transaction
                    if (dto.TransactionEntries.Cash.Count > 0 || dto.TransactionEntries.Cheque.Count > 0 || dto.TransactionEntries.Card.Count > 0)
                    {
                        tranType = "Normal";
                        nature = "M";
                        //var normalAmount = Convert.ToDecimal(transactionDto.TransactionEntries.Cash.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(transactionDto?.TransactionEntries?.Cheque?.Sum(x => x.Amount) ?? 0) + Convert.ToDecimal(transactionDto?.TransactionEntries?.Card?.Sum(x => x.Amount) ?? 0);
                        if (dto.TransactionEntries.TotalPaid > 0)
                            SaveTransactionEntries(transPayId, purchaseVoucherDebit, nature, dto.Party.Id,
                                           dto.TransactionEntries.TotalPaid, bankDate ?? null, refPageTypeId, dto.Currency.Id, dto.ExchangeRate,
                                           refPageTableId, Reference, description, tranType, dto.TransactionEntries.DueDate, null, null);
                    }
                }
                //Cash
                if (dto.TransactionEntries.Cash.Count > 0)
                {
                    tranType = "Cash";
                    nature = "M";
                    foreach (var cash in dto.TransactionEntries.Cash.Where(a => a.AccountCode.ID != 0 && a?.AccountCode.ID != null).ToList())
                    {
                        SaveTransactionEntries(transPayId, purchaseVoucherCredit, nature, cash.AccountCode.ID,
                        cash.Amount, bankDate ?? null, refPageTypeId, dto.Currency.Id, dto.ExchangeRate,
                        refPageTableId, Reference, cash.Description, tranType, dto.TransactionEntries.DueDate, null, null);
                    }
                }
                //Cheque
                if (dto.TransactionEntries.Cheque.Count > 0 && dto.TransactionEntries.Cheque.Select(x => x.PDCPayable.ID).FirstOrDefault() != 0)
                {
                    tranType = "Cheque";
                    nature = "M";
                    foreach (var cheque in dto.TransactionEntries.Cheque.Where(a => a.PDCPayable.ID != 0 && a?.PDCPayable.ID != null))
                    {
                        var veId = SaveTransactionEntries(transPayId, purchaseVoucherCredit, nature, cheque.PDCPayable.ID,
                            cheque.Amount ?? null, bankDate ?? null, refPageTypeId, dto.Currency.Id, dto.ExchangeRate,
                            refPageTableId, Reference, cheque.Description, tranType, dto.TransactionEntries.DueDate, null, null);
                        _paymentService.SaveCheque(cheque, veId, dto.Party.Id);
                    }
                }
                //Card
                if (dto.TransactionEntries.Card.Count > 0)
                {

                    tranType = "Card";
                    nature = "M";
                    foreach (var card in dto.TransactionEntries.Card.Where(a => a.AccountCode.ID != 0 && a?.AccountCode.ID != null).ToList())
                    {
                        SaveTransactionEntries(transPayId, purchaseVoucherCredit, nature, card.AccountCode.ID,
                        card.Amount, bankDate ?? null, refPageTypeId, dto.Currency.Id, dto.ExchangeRate,
                        refPageTableId, Reference, card.Description, tranType, dto.TransactionEntries.DueDate, null, null);
                    }
                }
                return CommonResponse.Ok(insertedId);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }

        public CommonResponse SaveTransactionAdditional(AdditionalDto dto, int TransId, int voucherId)
        {
            int? fromLocId = null, toLocId = null, inLocId = null, outLocId = null;
            string criteria = "";
            switch ((VoucherType)voucherId)
            {
                case VoucherType.Purchase:            

                    toLocId = dto.Warehouse?.Id ?? null;
                    inLocId = dto.Warehouse?.Id ?? null;

                    break;
                case VoucherType.Sales_Invoice:               
                    fromLocId = dto.Warehouse?.Id ?? null;
                    outLocId = dto.Warehouse?.Id ?? null;
                    break;

                case VoucherType.Purchase_Enquiry:
                    inLocId = dto.Warehouse?.Id ?? null;
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
                null, null,//2,3
                dto.Type.Id??null,//4

                dto.PayType?.Id ?? null,//5
                null, null,//6,7
                dto.DelivaryLocation?.Id ?? null,//8

                fromLocId,//9
                toLocId,//10
                null, null, null, null, null, null, null,
                null,
                dto.PartyNameandAddress ?? null,
                dto.Code ?? null,
                dto.TermsOfDelivery ?? null,//21
                null, null,//22,23
                dto.CreditPeriod ?? null,//24
                dto.Days ?? null, null,
                dto.MobileNo ?? null,//27
                null, null, null, null,
                dto.StaffIncentives ?? null,//32
                null,
                dto.OtherDiscAmt??null,//34
                null, null, null, null, null,
                dto.DespatchNo ?? null,//40
                dto.DespatchDate ?? null,//41
                null,
                dto.PartyDate ?? null,//43
                dto.PartyInvoiceNo ?? null,//44
                null,
                dto.Attention ?? null,//46
                null, null, null,
                 dto.ExpiryDate ?? null,//50
                null, null, null, null, null, null, null,
                dto.DeliveryDate ?? null,//58
                null, null, null, null,
                dto.DeliveryNote ?? null,//63
                dto.OrderDate ?? null,//64
                dto.OrderNo ?? null,//65
                null, null, null,

                dto.VehicleNo?.Id ?? null,//69
                null, null, null, null,
                dto.DelivaryLocation?.Name ?? null,//74

                null,
                dto.Approve ?? null,//76
                dto.TransPortationType.Id,
                inLocId,//78
                outLocId,//79
                null, null,

                dto.SalesMan?.Id ?? null,//82
                null, null, null, null,
                dto.SalesArea?.Id ?? null,//87
                null,
                dto.PriceCategory.Id??null,
                dto.CloseVoucher ?? null,//90
                null,
                dto.PartyName ?? null,//92
                dto.AddressLine1 ?? null,//93
                dto.AddressLine2 ?? null,//94

                null, null

                );

            return CommonResponse.Ok();
        }
    }
}
