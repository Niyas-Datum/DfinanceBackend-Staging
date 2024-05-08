﻿using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Domain;
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
        /// <summary>
         /// Purchase Frm  auto TransNo:
         /// </summary>
         /// <returns></returns>
        public CommonResponse GetAutoVoucherNo(int voucherid)
        {
            try
            {
                var voucher = _context.FiMaVouchers
                    .Where(x => x.PrimaryVoucherId == voucherid)
                    .FirstOrDefault();

                if (voucher == null)
                {
                    return CommonResponse.Error(new Exception("Voucher not found."));
                }

                int branchid = _authService.GetBranchId().Value;

                var result = _context.AccountCodeView
                    .FromSqlRaw($"EXEC GetNextAutoEntryVoucherNoSP @VoucherID={voucherid}, @BranchID={branchid}")
                    .ToList();

                return CommonResponse.Ok(new { Code = voucher.Code, Result = result });
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionDto"></param>
        /// <returns></returns>
        public CommonResponse SaveTransaction(PurchaseDto transactionDto, int PageId, int VoucherId, string Status)
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;
                int createdBy = _authService.GetId().Value;
                //int SerialNo = 1;
                bool Autoentry = false;
                int? RefTransId = null;
                string? ApprovalStatus = "A";

                string ReferencesId = string.Join(",", transactionDto.Reference.Select(popupDto => popupDto.VNo.ToString()));

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
                        ApprovalStatus, null, null, Status, Autoentry, true, true, false, transactionDto.Supplier.Id,
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
                        ApprovalStatus, null, null, Status, Autoentry, true, true, false, transactionDto.Supplier.Id,
                        null, RefTransId, transactionDto.Project.Id, PageId, Transaction);

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
        public CommonResponse SaveTransactionPayment(PurchaseDto transactionDto, int TransId, string Status)
        {
           
            try
            {
                int branchId = _authService.GetBranchId().Value;
                int createdBy = _authService.GetId().Value;
                // int SerialNo = 1;
                bool Autoentry = false;
                int? RefTransId = null;
                string? ApprovalStatus = "A";
                int VoucherId = 2;
                int PageId = 69;
                Autoentry = true;
                RefTransId = TransId;
                string ReferenceId = null;
                string environmentname = _environment.EnvironmentName;

                if (transactionDto.Id ==0 ||transactionDto.Id==null)
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
                        criteria, transactionDto.Date, DateTime.Now, VoucherId, environmentname,
                        transactionDto.VoucherNo, false, transactionDto.Currency.Id, transactionDto.ExchangeRate, null, null,
                        ReferenceId, branchId, null, null, null,
                        null, null, transactionDto.Description, createdBy, null, DateTime.Now, null,
                        ApprovalStatus, null, null, Status, Autoentry, true, true, false, transactionDto.Supplier.Id,
                        null, RefTransId, transactionDto.Project.Id, PageId, newId);

                    //transactionDto.Id = (int)newId.Value;
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
                        ApprovalStatus, null, null, Status, Autoentry, true, true, false, transactionDto.Supplier.Id,
                        null, RefTransId, transactionDto.Project.Id, PageId, PayId);

                    return CommonResponse.Ok(transactionDto.Id);
                }
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
        public CommonResponse SaveVoucherAllocation(int transId, int TransEntId, int? accountid, decimal? amount, List<int> referTransIds)
        {
            
            List<int> processedReferIds = new List<int>();
            try
            {
              
                string criteria = "InsertVoucherAllocation";

                foreach (int referId in referTransIds)
                {
                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @VID={1}, @VEID={2}, @AccountID={3},@Amount={4},@RefTransID={5}, @NewID={6} OUTPUT",
                        criteria, transId, TransEntId, accountid, amount, referId, newId);

                    processedReferIds.Add(referId);
                }
                return CommonResponse.Ok(processedReferIds);

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
        public CommonResponse UpdateVoucherAllocation(int transId, int TransEntId, int? accountid, decimal? amount, List<int> referTransIds)
        {

            List<int> processedReferIds = new List<int>();
            try
            {
                int Id=_context.FiVoucherAllocation .Where(x => x.Vid == transId)
                 .Select(x => x.Id)
                 .FirstOrDefault();

                string criteria = "UpdateVoucherAllocation";

                foreach (int referId in referTransIds)
                {

                    _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @VID={1}, @VEID={2}, @AccountID={3},@Amount={4},@RefTransID={5}, @NewID={6} OUTPUT",
                        criteria, transId, TransEntId, accountid, amount, referId, Id);

                    processedReferIds.Add(referId);
                }
                return CommonResponse.Ok(processedReferIds);

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
        public CommonResponse DeletePurchase(int TransId)
        {
            try
            {
                var transid=_context.FiTransaction.Any(x=>x.Id == TransId);
                if (!transid )
                {
                    return CommonResponse.NotFound("Transaction Not Found");
                }
                string criteria = "DeleteTransactions";

                _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @ID={1}",
                                       criteria, TransId);

                return CommonResponse.Ok("Delete successfully");
            }
            catch (Exception ex)
            {
               
                return CommonResponse.Error(ex);
            }
        }
    }
}
