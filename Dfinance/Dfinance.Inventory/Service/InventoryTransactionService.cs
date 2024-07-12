using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Inventory.Purchase;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Enum;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using static Dfinance.Shared.Routes.v1.FinRoute;
using Voucher = Dfinance.Core.Domain.Voucher;
using System.Text;

namespace Dfinance.Inventory.Service
{
    public class InventoryTransactionService : IInventoryTransactionService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IHostEnvironment _environment;


        public InventoryTransactionService(DFCoreContext context, IAuthService authService, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _authService = authService;
            _environment = hostEnvironment;


        }

        //Fill Party Balance when selecting supplier/ customer
        public CommonResponse FillPartyBal(int partyId)
        {
            int BranchId=_authService.GetBranchId().Value;
            var result = _context.CurrentStockView.FromSqlRaw($"select dbo.GetBalance('{partyId}',null,'{BranchId}')").ToList();            
            return CommonResponse.Ok(result);
        }


        //Fill Pay Type dropdown
        public CommonResponse FillPayType()
        {
            var payType = _context.DropDownViewName.FromSqlRaw("exec DropDownListSP @Criteria='FillPartyCollection'").ToList();
            return CommonResponse.Ok(payType);
        }
        /// <summary>
        /// Purchase Frm  auto TransNo:
        /// </summary>
        /// <returns></returns>
        public CommonResponse GetAutoVoucherNo(int voucherid)
        {
            try
            {
                Voucher? voucher = _context.FiMaVouchers
                    .Where(x => x.Id == voucherid)
                    .FirstOrDefault();

                if (voucher == null)
                {
                    return CommonResponse.Error(new Exception("Voucher not found."));
                }

                int branchid = _authService.GetBranchId().Value;
                //GetPrimaryVoucherID(voucherid);

                var voucherNo = GetNextTransactionNo(voucherid, voucher, branchid);
                return CommonResponse.Ok(voucherNo);

            }
            catch (Exception ex)
            {

                return CommonResponse.Error(ex);
            }
        }

        public int GetPrimaryVoucherID(int voucherid)
        {
            return (int)(_context.FiMaVouchers.Where(v => v.Id == voucherid).Select(v => v.PrimaryVoucherId).FirstOrDefault());
        }

        private VoucherNo GetNextTransactionNo(int voucherid, Voucher? voucher, int branchid)
        {
            var result = _context.AccountCodeView
                .FromSqlRaw($"EXEC GetNextAutoEntryVoucherNoSP @VoucherID={voucherid}, @BranchID={branchid}")
                .ToList();
            VoucherNo voucherNo = new VoucherNo
            {
                Code = voucher.Code,
                Result = result
            };

            return voucherNo;
        }

        public CommonResponse GetAutoVoucherNo(int voucherid, int? payTypeId)
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
                var payType = _context.MaMisc.Where(m => m.Id == payTypeId).Select(m => m.Value).FirstOrDefault();

                var primaryVoucherId = GetPrimaryVoucherID(voucherid);
                if (partywiseVoucherNo && (VoucherType)primaryVoucherId == VoucherType.Sales_Invoice)
                {
                    if (payType != null)
                    {
                        var result = _context.AccountCodeView
                        .FromSqlRaw($"EXEC GetPartywiseVoucherNoSP @VoucherID={voucherid}, @BranchID={branchid}, @PurchaseNumberPrefix=")
                        .ToList();
                        VoucherNo voucherNo = new VoucherNo
                        {
                            Code = voucher.Code,
                            Result = result
                        };

                        return CommonResponse.Ok(voucherNo);
                    }
                    else
                    {
                        var voucherNo = GetNextTransactionNo(voucherid, voucher, branchid);
                        return CommonResponse.Ok(voucherNo);
                    }
                }
                else
                {
                    var voucherNo = GetNextTransactionNo(voucherid, voucher, branchid);
                    return CommonResponse.Ok(voucherNo.Result[0].AccountCode);
                }

            }
            catch (Exception ex)
            {

                return CommonResponse.Error(ex);
            }
        }

        /// <summary>
        /// Get Saleman =>Purchase 
        /// </summary>
        /// <returns></returns>
        public CommonResponse GetSalesman()
        {
            try
            {
                var result = _context.FiMaAccounts
                .Where(a => a.AccountCategory == 3 && a.Active)
                .Select(a => new
                {
                    Code = a.Alias,
                    Name = a.Name,
                    ID = a.Id
                })
                .ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {

                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse GetSalesman(string designationName)
        {
            try
            {
                var result = (from maAccount in _context.FiMaAccounts
                              join employee in _context.MaEmployees on maAccount.Id equals employee.AccountId
                              join designation in _context.MaDesignations on employee.DesignationId equals designation.Id
                              where maAccount.AccountCategory == 3 && maAccount.Active && designation.Name == designationName
                              select new
                              {
                                  Code = maAccount.Alias,
                                  Name = maAccount.Name,
                                  ID = maAccount.Id
                              })
                  .ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {

                return CommonResponse.Error(ex);
            }
        }

        //fill the voucher type dropdown in import reference
        public CommonResponse FillVoucherType(int voucherId)
        {
            string criteria = "FillPreVouchers";
            var voucherType = _context.DropDownViewName.FromSqlRaw("Exec DropDownListSP @Criteria={0},@IntParam={1}", criteria, voucherId);
            return CommonResponse.Ok(voucherType);
        }
        /// <summary>
        /// Get Refernce
        /// </summary>
        /// <returns></returns>
        public CommonResponse GetReference(int voucherno, DateTime? date = null)
        {
            try
            {
                int BranchID = _authService.GetBranchId().Value;
                int? OtherBranchID = null;
                string criteria = "FillImportTransactions";

                // Fetching the setting value for ReferenceImportItemTracking from the database
                string referenceImportItemTracking = _context.MaSettings
                    .Where(setting => setting.Key == "ReferenceImportItemTracking")
                    .Select(setting => setting.Value)
                    .FirstOrDefault();

                bool isReferenceImportItemTrackingTrue = !string.IsNullOrEmpty(referenceImportItemTracking) &&
                                                         (referenceImportItemTracking == "True" || referenceImportItemTracking == "1");
                var data = _context.ReferenceView
                    .FromSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0}, @VoucherID={1}, @BranchID={2}, @Date={3}, @OtherBranchID={4}",
                        criteria, voucherno, BranchID, date ?? null, OtherBranchID ?? null)
                    .ToList();


                if (!isReferenceImportItemTrackingTrue)
                {
                    //data = data.Where(item => item.PartyInvNo - item.PartyInvNo > 0).ToList();
                }

                return CommonResponse.Created(data);
            }
            catch (Exception ex)
            {

                return CommonResponse.Error("An error occurred while fetching references.");
            }
        }
        public CommonResponse FillImportItemList(int? transId, int? voucherId)
        {
            string criteria = "FillImportItemListWeb";
            var data = _context.ImportItemListView.FromSqlRaw("Exec VoucherAdditionalsSP @Criteria={0},@VoucherID={1},@TransactionID={2}",
                criteria, voucherId, transId).ToList();
            return CommonResponse.Ok(data);
        }

        public CommonResponse FillReference(List<ReferenceDto> referenceDto)
        {
            int? transId = 0;
            string criteria = "FillImportItemsWeb";
            string itemIds = null;
            List<int?>? itemId = null;
            List<RefItemsView>? refItems = null;
            foreach (var refer in referenceDto)
            {
                if (refer.Sel == true)
                {
                    transId = refer.Id;
                    // importItems = (List<ImportItemListDto>?)FillImportItemList(transId, refer.VoucherId).Data;
                    if (refer.AddItem == true)
                    {
                        itemId = refer.RefItems.Select(r => r.ItemID).ToList();
                    }
                    else
                    {
                        itemId = refer.RefItems.Where(i => i.Select == true).Select(r => r.ItemID).ToList();
                    }
                    if (itemId.Count > 0)
                        itemIds = string.Join(",", itemId.ToArray());
                    refItems = _context.RefItemsView.FromSqlRaw("exec VoucherAdditionalsSP @Criteria={0},@TransactionID={1},@ItemIdString={2}",
                        criteria, transId, itemIds).ToList();
                }
            }
            return CommonResponse.Ok(refItems);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionDto"></param>
        /// <returns></returns>
        public CommonResponse SaveTransaction(InventoryTransactionDto transactionDto, int PageId, int VoucherId, string Status)
        {
            try
            {
                SetSettings();
                var primaryVoucherId = GetPrimaryVoucherID(VoucherId);
                var totalAmt = transactionDto.Items.Sum(i => i.Total);
                var grandTotal = Convert.ToDecimal(transactionDto.TransactionEntries.GrandTotal) + Convert.ToDecimal(transactionDto.TransactionEntries.TotalDisc) - Convert.ToDecimal(transactionDto.TransactionEntries.AddCharges.Sum(a => a.Amount));
                if (grandTotalVerify && totalAmt - grandTotal >= 1 && grandTotal - totalAmt >= 1)// Grand Total Verify settings 
                {
                    return CommonResponse.Error("There is an error in GrandTotal");
                }
                //Set DueDate Settings
                if (voucherDateAsDueDate)
                {
                    if (transactionDto.TransactionEntries.DueDate == null)
                    {
                        transactionDto.TransactionEntries.DueDate = transactionDto.Date;
                    }
                }
                //Set AutoVoucher Next TransactionNo
                if (autoUpdateNewVoucherNo)
                {
                    transactionDto.VoucherNo = (string?)GetAutoVoucherNo(VoucherId, transactionDto.FiTransactionAdditional.PayType.Id).Data;
                }
                //Set SerialNo
                if (rackLocation)
                {
                    transactionDto.Items.OrderBy(i => i.Location);
                }
                //Set RoundOFF calculation
                if (inventoryToFinanceRoundOff)
                {
                    int numeric = 0;
                    if (numericFormat == "N2")
                        numeric = 2;
                    else if (numericFormat == "N3")
                        numeric = 3;
                    else if (numericFormat == "N4")
                        numeric = 4;
                    var round = transactionDto.TransactionEntries.GrandTotal;
                    transactionDto.TransactionEntries.GrandTotal = Decimal.Round(transactionDto.TransactionEntries.GrandTotal ?? 0, numeric);
                    transactionDto.TransactionEntries.Roundoff = round - transactionDto.TransactionEntries.GrandTotal;
                }

                if ((VoucherType)primaryVoucherId == VoucherType.Purchase || (VoucherType)primaryVoucherId == VoucherType.Sales_Invoice || (VoucherType)primaryVoucherId == VoucherType.Purchase_Return && (VoucherType)primaryVoucherId == VoucherType.Sales_Return)
                {
                    if (transactionDto.References.Count > 0 && transactionDto.References.Any(r => r.Id != null || r.Id != 0))
                    {
                        var refItemView = (List<RefItemsView>)FillReference(transactionDto.References).Data;
                        if (refItemView != null)
                        {
                            foreach (var item in transactionDto.Items)
                            {
                                var itemQty = refItemView.Where(i => i.ItemID == item.ItemId && i.Qty < item.Qty).FirstOrDefault() ?? null;
                                if (itemQty != null)
                                {
                                    // var itemDetails = itemQty;
                                    return CommonResponse.Ok(itemQty.ItemName + " Item quantity greater than Reference item quantity");
                                }
                            }
                        }
                    }
                }
                int? reserveVoucherId = null;
                if ((VoucherType)primaryVoucherId == VoucherType.Sales_Invoice && transactionDto.References.Select(r => r.Id) != null)
                {
                    foreach (var trans in transactionDto.References)
                    {
                        if (trans.Sel == true)
                        {
                            reserveVoucherId = trans.Id;
                        }
                    }
                    //if(dosageSystem)
                    //autoapproval
                    if (autoApproval)
                    {
                        var additional = _context.FiTransactionAdditionals.FirstOrDefault(a => a.TransactionId == transactionDto.Id);
                        if (additional != null)
                        {
                            additional.IsHigherApproval = true;
                            _context.SaveChanges();
                        }
                    }
                }



                int branchId = _authService.GetBranchId().Value;
                int createdBy = _authService.GetId().Value;
                //int SerialNo = 1;
                bool Autoentry = false;
                int? RefTransId = null;
                string? ApprovalStatus = "A";

                string ReferencesId = string.Join(",", transactionDto.References.Select(popupDto => popupDto.VNo.ToString()));

                string environmentname = _environment.EnvironmentName;
                if (transactionDto.Id == null || transactionDto.Id == 0)
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
                        criteria, transactionDto.Date, DateTime.Now, VoucherId, environmentname,
                        transactionDto.VoucherNo, false, transactionDto.Currency.Id, transactionDto.ExchangeRate, null, null,
                        ReferencesId, branchId, null, null, null,
                        null, null, transactionDto.Description, createdBy, null, DateTime.Now, null,
                        ApprovalStatus, null, null, Status, Autoentry, true, true, false, transactionDto.Party.Id,
                        null, RefTransId, transactionDto.Project.Id, PageId, newId);

                    var NewId = (int)newId.Value;
                    // transactionDto.Id = NewId;
                    return CommonResponse.Ok(NewId);
                }

                else
                {
                    int Transaction = _context.FiTransaction.Where(x => x.Id == transactionDto.Id).Select(x => x.Id).FirstOrDefault();
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
                        criteria, transactionDto.Date, DateTime.Now, VoucherId, environmentname,
                        transactionDto.VoucherNo, false, transactionDto.Currency.Id, transactionDto.ExchangeRate, null, null,
                        ReferencesId, branchId, null, null, null,
                        null, null, transactionDto.Description, createdBy, null, DateTime.Now, null,
                        ApprovalStatus, null, null, Status, Autoentry, true, true, false, transactionDto.Party.Id,
                        null, RefTransId, transactionDto.Project.Id, PageId, transactionDto.Id);

                    return CommonResponse.Ok(transactionDto.Id);

                }
            }
            catch (Exception ex)
            {

                return CommonResponse.Error(ex);
            }
        }
        private bool grandTotalVerify = false;
        private bool voucherDateAsDueDate = false;
        private bool autoUpdateNewVoucherNo = false;
        private bool partywiseVoucherNo = false;
        private bool rackLocation = false;
        private bool inventoryToFinanceRoundOff = false;
        private string numericFormat;
        private string inventoryApproval;
        private bool costCentreSystem = false;
        private bool commonCostcenterAllocationWindow = false;
        private bool autoApproval = false;
        private bool dosageSystem = false;
        private void SetSettings()
        {
            string[] keys = new string[] {"TaxBasedInvoiceAccount","MethodofAdditionalExpenseAllocationToItemCost","GrandTotalVerify","VoucherDateAsDueDate","AutoUpdateNewVoucherNo",
                    "PartywiseVoucherNo","CreditLimitCheck","CreditPeriodCheck","RackLocation","InventoryToFinanceRoundOff","ItemDiscountAccounting",
                    "IsCSTApplicable","AdditionalExpenseAccountingInInvoice","ExpenseCSTAccountEntry","DiscountAccounting","CostCentreSystem",
                    "CommonCostcenterAllocationWindow","DosageSystem","AutoApproval","SalesArabicPrint","PrintAfterSave","NumericFormat","InventoryApproval" };
            var settings = _context.MaSettings
        .Where(m => keys.Contains(m.Key))
        .Select(m => new
        {
            Key = m.Key,
            Value = m.Value,
        }).ToList();
            //var dicSettings = settings.ToDictionary(Key => Key, Value => Value);
            grandTotalVerify = Convert.ToBoolean(settings.Where(s => s.Key == "GrandTotalVerify").Select(s => s.Value).FirstOrDefault());
            voucherDateAsDueDate = Convert.ToBoolean(settings.Where(s => s.Key == "VoucherDateAsDueDate").Select(s => s.Value).FirstOrDefault());
            autoUpdateNewVoucherNo = Convert.ToBoolean(settings.Where(s => s.Key == "AutoUpdateNewVoucherNo").Select(s => s.Value).FirstOrDefault());
            partywiseVoucherNo = Convert.ToBoolean(settings.Where(s => s.Key == "PartywiseVoucherNo").Select(s => s.Value).FirstOrDefault());
            rackLocation = Convert.ToBoolean(settings.Where(s => s.Key == "RackLocation").Select(s => s.Value).FirstOrDefault());
            var round = settings.Where(s => s.Key == "InventoryToFinanceRoundOff").Select(s => s.Value).FirstOrDefault();
            if (round == "1")
                round = "true";
            else round = "false";
            inventoryToFinanceRoundOff = Convert.ToBoolean(round);
            numericFormat = settings.Where(s => s.Key == "NumericFormat").Select(s => s.Value).FirstOrDefault().ToString();
            inventoryApproval = settings.Where(s => s.Key == "InventoryApproval").Select(s => s.Value).FirstOrDefault().ToString();
            costCentreSystem = Convert.ToBoolean(settings.Where(s => s.Key == "CostCentreSystem").Select(s => s.Value).FirstOrDefault());
            commonCostcenterAllocationWindow = Converter.StringToBoolean(settings.Where(s => s.Key == "CommonCostcenterAllocationWindow").Select(s => s.Value).FirstOrDefault());
            autoApproval = Converter.StringToBoolean(settings.Where(s => s.Key == "AutoApproval").Select(s => s.Value).FirstOrDefault().ToString());
            dosageSystem = Converter.StringToBoolean(settings.Where(s => s.Key == "DosageSystem").Select(s => s.Value).FirstOrDefault());
        }
        //CostCenter Settings
        private void ShowCostCenters(int transId, List<TransCostAllocationDto> transCostAllocation)
        {
            if (costCentreSystem && commonCostcenterAllocationWindow)
            {
                if (!costCentreSystem) return;
                Boolean skipRowAddForEdit = false;
                var transEntry = _context.FiTransactionEntries.Where(e => e.TransactionId == transId).ToList();
                foreach (var entry in transEntry)
                {
                    var isCostCentre = _context.FiMaAccounts.Where(a => a.Id == entry.AccountId).Select(a => a.IsCostCentre).FirstOrDefault();
                    if (Convert.ToBoolean(isCostCentre))
                    {
                        skipRowAddForEdit = false;
                        var transCostAllocate = _context.TransCostAllocations.Any(c => c.Veid == entry.Id);
                        if (transCostAllocate)
                            skipRowAddForEdit = true;
                        if (skipRowAddForEdit)
                            continue;
                        if (transCostAllocation != null && transCostAllocation.Count > 0)
                        {
                            foreach (var transcost in transCostAllocation)
                            {
                                var transCostAllocateId = _context.TransCostAllocations.Where(c => c.Id == transcost.Id).Select(c => c.Id).FirstOrDefault();
                                if (transCostAllocateId != null)
                                {
                                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                                    {
                                        Direction = ParameterDirection.Output
                                    };
                                    var critera = "InsertTransCostAllocations";
                                    _context.Database.ExecuteSqlRaw("EXEC CostCentreSP @Criteria={0},@VEID={1},@CostCentreID={2},@Amount={3},@Description={4},@NewID={5} OUTPUT",
                                        critera, entry.Id, transcost.CostCentreId, transcost.Amount, transcost.Description ?? null, newId);
                                }
                                else
                                {
                                    var critera = "UpdateTransCostAllocations";
                                    _context.Database.ExecuteSqlRaw("EXEC CostCentreSP @Criteria={0},@VEID={1},@CostCentreID={2},@Amount={3},@Description={4},@ID={5} ",
                                        critera, entry.Id, transcost.CostCentreId, transcost.Amount, transcost.Description ?? null, transCostAllocateId);
                                }
                            }
                        }
                    }
                }
            }
        }


        //ItemWiseExpanse
        private decimal CalculateItemWiseExp(Object AccountID)
        {
            string AllocationMethod = string.Empty;
            decimal TotalExp;
            //decimal Ratio;
            decimal TotalQty;
            TotalExp = 0;
            return TotalExp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionDto"></param>
        /// <param name="PageId"></param>
        /// <param name="VoucherId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
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
                            voucherNo.Result[0].AccountCode, false, transactionDto.Currency.Id, transactionDto.ExchangeRate, null, null,
                            ReferenceId, branchId, null, null, null,
                            null, null, transactionDto.Description, createdBy, null, DateTime.Now, null,
                            ApprovalStatus, null, null, Status, Autoentry, true, true, transactionDto.Cancelled, transactionDto.Party.Id,
                            transactionDto.Description, RefTransId, transactionDto.Project.Id, PageId, newId);

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
                            "@InstrumentNo={15}, @InstrumentDate={16}, @InstrumentBank={17}, @CommonNarration={18}, @EditedBy={19}, " +
                            "@ApprovedBy={20}, @EditedDate={21}, @ApprovedDate={22}, @ApprovalStatus={23}, " +
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
        /// <summary>
        /// TransRefernce Save()
        /// </summary>
        /// <param name="transId"></param>
        /// <param name="referId"></param>
        public CommonResponse SaveTransReference(int transId, List<int?> referIds)
        {
            List<int> processedReferIds = new List<int>();

            try
            {
                string dec = null;
                string criteria = "InsertTransactionReferences";

                foreach (int referId in referIds)
                {
                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0}, @TransactionID={1}, @RefTransID={2}, @Description={3}, @NewID={4} OUTPUT",
                        criteria, transId, referId, dec, newId);

                    processedReferIds.Add(referId);
                }

                return CommonResponse.Ok();
            }
            catch (Exception ex)
            {

                throw; // Rethrow the exception to handle it in the calling code
            }
        }
        /// <summary>
        /// Save FiMaVoucher allocation
        /// </summary>
        /// <param name="transId"></param>
        /// <param name="TransEntId"></param>
        /// <param name="accountid"></param>
        /// <param name="amount"></param>
        /// <param name="referTransIds"></param>
        /// <returns></returns>
        public CommonResponse SaveVoucherAllocation(int transId, int transpayId, InvTransactionEntriesDto transactionAdvance)
        {

            // List<int> processedReferIds = new List<int>();
            try
            {
                int? refTransId = null;
                var transEntryId = _context.FiTransactionEntries.Where(e => e.TransactionId == transId && e.TranType == "Party").FirstOrDefault();
                string criteria = "InsertVoucherAllocation";

                if (transEntryId != null)
                {
                    if (transactionAdvance.Advance != null && transactionAdvance.Advance.Any(a => a.Amount > 0))
                    {
                        foreach (var adv in transactionAdvance.Advance)
                        {
                            if (adv.VID != 0)
                            {
                                refTransId = adv.VID.Value;
                            }
                            else
                            {
                                refTransId = transpayId;
                                adv.VID = transEntryId.TransactionId;
                            }
                            SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };

                            _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @VID={1}, @VEID={2}, @AccountID={3},@Amount={4},@RefTransID={5}, @NewID={6} OUTPUT",
                                criteria, adv.VID, transEntryId.Id, adv.AccountID, adv.Amount, transId, newId);
                        }
                    }
                    if (transId != transpayId)
                    {
                        var amount = transactionAdvance.Card.Sum(c => c.Amount) + transactionAdvance.Cash.Sum(c => c.Amount) + transactionAdvance.Cheque.Sum(c => c.Amount);
                        SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @VID={1}, @VEID={2}, @AccountID={3},@Amount={4},@RefTransID={5}, @NewID={6} OUTPUT",
                           criteria, transpayId, transEntryId.Id, transEntryId.AccountId, amount, transpayId, newId);
                    }
                }
                return CommonResponse.Ok();

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);

            }
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="transId"></param>
        /// <param name="TransEntId"></param>
        /// <param name="accountid"></param>
        /// <param name="amount"></param>
        /// <param name="referTransIds"></param>
        /// <returns></returns>
        public CommonResponse UpdateVoucherAllocation(int transId, int transpayId, InvTransactionEntriesDto transactionAdvance)
        {

            // List<int> processedReferIds = new List<int>();
            try
            {
                _context.FiVoucherAllocation.Where(v => v.RefTransId == transId).ExecuteDelete();
                SaveVoucherAllocation(transId, transpayId, transactionAdvance);

                return CommonResponse.Ok();

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);

            }
        }

        /// <summary>
        /// UpdateTransReference
        /// </summary>
        /// <param name="transId"></param>
        /// <param name="referId"></param>
        public CommonResponse UpdateTransReference(int? transId, List<int?> referIds)
        {
            try
            {
                var Transref = _context.TransReference.FirstOrDefault(x => x.TransactionId == transId);

                if (Transref == null)
                {
                    return CommonResponse.Error("Transaction reference not found.");

                }

                string dec = null;
                string criteria = "UpdateTransactionReferences";

                foreach (var referId in referIds)
                {
                    _context.Database.ExecuteSqlRaw("EXEC VoucherAdditionalsSP @Criteria={0}, @TransactionID={1}, @RefTransID={2}, @Description={3}, @ID={4}",
                        criteria, transId, referId, dec, Transref.Id);
                }
                return CommonResponse.Ok();
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
                //Console.WriteLine("Error in UpdateTransReference: " + ex.Message);
            }
        }
        /// <summary>
        /// Delete Transactions
        /// </summary>
        /// <param name="TransId"></param>
        /// <returns></returns>
        public CommonResponse DeleteTransactions(int transId)
        {
            try
            {

                inventoryApproval = GetSettings("InventoryApproval").ToString();
                var userRole = _authService.GetUserRole();
                if (inventoryApproval != "N" && inventoryApproval.Contains(userRole))
                {
                   var isHigherApproval=Convert.ToBoolean(_context.FiTransactionAdditionals.Where(a=>a.TransactionId == transId).Select(a=>a.IsHigherApproval).FirstOrDefault());
                    Object EditStatus = isHigherApproval;
                    if (EditStatus != DBNull.Value ? Convert.ToBoolean(EditStatus) : false)
                    {
                        return CommonResponse.Ok("You dont have permission to Cancel approved transactions");
                    }
                }
                var transid = _context.FiTransaction.Any(x => x.Id == transId);
                if (!transid)
                {
                    return CommonResponse.NotFound("Transaction Not Found");
                }
                string criteria = "DeleteTransactions";

                _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @ID={1}",
                                       criteria, transId);

                return CommonResponse.Ok("Delete successfully");
            }
            catch (Exception ex)
            {

                return CommonResponse.Error(ex);
            }
        }
        public CommonResponse CancelTransaction(int transId, string reason)
        {
            try
            {
                inventoryApproval = GetSettings("InventoryApproval").ToString();
                var userRole = _authService.GetUserRole();
                if (inventoryApproval != "N" && inventoryApproval.Contains(userRole))
                {
                    var isHigherApproval = Convert.ToBoolean(_context.FiTransactionAdditionals.Where(a => a.TransactionId == transId).Select(a => a.IsHigherApproval).FirstOrDefault());
                    Object EditStatus = isHigherApproval;
                    if (EditStatus != DBNull.Value ? Convert.ToBoolean(EditStatus) : false)
                    {
                        return CommonResponse.Ok("You dont have permission to Cancel approved transactions");
                    }
                }
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
                //SaveTransaction(inventoryTransactionDto, PageId, VoucherId, "Approve");
                return CommonResponse.Ok("Cancelled successfully");
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

        /// <summary>
        /// Inv=>report=>InventoryTransactions
        /// </summary>
        /// <param name="inventoryTransactionDto"></param>
        /// <returns></returns>

        public CommonResponse InventoryTransactions(InventoryTransactionsDto inventoryTransactionDto, int? moduleid)
        {
            int branchid = 1; // Assuming a default value for branchid. This should be fetched from the context or passed as a parameter if required.
            try
            {
                var query = new StringBuilder();
                query.Append("Exec FinTransactionsSP ");
                query.Append("@Criteria = 'FillInventoryVoucherSummary', ");
                query.AppendFormat("@BranchID = {0}, ", branchid);
                query.AppendFormat("@DateFrom = '{0}', ", inventoryTransactionDto.From.ToString("yyyy-MM-dd"));
                query.AppendFormat("@DateUpto = '{0}', ", inventoryTransactionDto.To.ToString("yyyy-MM-dd"));
                query.AppendFormat("@ModuleID = {0}, ", moduleid ?? 0);

                if (inventoryTransactionDto.Mode.Id != 0)
                {
                    query.AppendFormat("@ModeID = {0}, ", inventoryTransactionDto.Mode.Id);
                }

                if (!string.IsNullOrEmpty(inventoryTransactionDto.Machine?.Value))
                {
                    query.AppendFormat("@MachineName = '{0}', ", inventoryTransactionDto.Machine.Value);
                }

                if (inventoryTransactionDto.VoucherType.Id != 0)
                {
                    query.AppendFormat("@VTypeID = {0}, ", inventoryTransactionDto.VoucherType.Id);
                }

                // Remove the trailing comma and space
                var finalQuery = query.ToString().TrimEnd(' ', ',');

                var result = _context.InventoryTransactionsView.FromSqlRaw(finalQuery).ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }
        private object GetSettings(string key)
        {
            var settings = _context.MaSettings
        .Where(m => key.Contains(m.Key))
        .Select(m => m.Value).FirstOrDefault();
            return settings;
        }

        private List<TransExpense> FillTransexpenses(int transId)
        {
            try
            {
                var transExp = _context.TransExpense.FromSqlRaw("EXEC VoucherSP @Criteria={0},@VID={1}",
                                  "FillTransexpenses", transId).ToList();
                return transExp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
