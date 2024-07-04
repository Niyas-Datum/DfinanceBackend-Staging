using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Inventory.Purchase;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared;
using Dfinance.Shared.Domain;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;

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
                    .ToList();
                VoucherNo voucherNo = new VoucherNo
                {
                    Code = voucher.Code,
                    Result = result
                };
                return CommonResponse.Ok(voucherNo);
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

        //fill the voucher type dropdown in import reference
        public CommonResponse FillVoucherType(int voucherId)
        {
            string criteria = "FillPreVouchers";
            var voucherType=_context.DropDownViewName.FromSqlRaw("Exec DropDownListSP @Criteria={0},@IntParam={1}",criteria,voucherId);
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
                int BranchID= _authService.GetBranchId().Value;
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
                        criteria, voucherno, BranchID , date ?? null, OtherBranchID ?? null)
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
                if (VoucherId == 17 || VoucherId == 23 || VoucherId == 77 && VoucherId == 76)
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
                int branchId = _authService.GetBranchId().Value;
                int createdBy = _authService.GetId().Value;
                //int SerialNo = 1;
                bool Autoentry = false;
                int? RefTransId = null;
                string? ApprovalStatus = "A";

                string ReferencesId = string.Join(",", transactionDto.References.Select(popupDto => popupDto.VNo.ToString()));

                string environmentname = _environment.EnvironmentName;
                if (transactionDto.Id == null||transactionDto.Id==0)
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
        }/// <summary>
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
                int PageId = _context.MaPageMenus.Where(p=>p.VoucherId==VoucherId).Select(p=>p.Id).FirstOrDefault();
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
        public CommonResponse SaveVoucherAllocation(int transId,int transpayId, InvTransactionEntriesDto transactionAdvance)
        {
            
           // List<int> processedReferIds = new List<int>();
            try
            {
                int? refTransId = null;
                var transEntryId=_context.FiTransactionEntries.Where(e=>e.TransactionId == transId && e.TranType=="Party").FirstOrDefault();
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
                    if(transId!=transpayId)
                    {
                        var amount = transactionAdvance.Card.Sum(c=>c.Amount) + transactionAdvance.Cash.Sum(c => c.Amount) + transactionAdvance.Cheque.Sum(c => c.Amount);
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
                   SaveVoucherAllocation(transId,transpayId, transactionAdvance);
                //int Id=_context.FiVoucherAllocation .Where(x => x.RefTransId == transId)
                // .Select(x => x.Id)
                // .FirstOrDefault();

                //string criteria = "UpdateVoucherAllocation";
                //foreach (var adv in transactionAdvance.Advance)
                //{


                //    _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @VID={1}, @VEID={2}, @AccountID={3},@Amount={4},@RefTransID={5}, @ID={6} ",
                //        criteria, adv.VID, adv.VEID, adv.AccountID, adv.Amount, transId, Id);

                //}
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
        public CommonResponse DeletePurchase(int transId)
        {
            try
            {
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
