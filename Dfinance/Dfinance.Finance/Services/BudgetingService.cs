using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Finance;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Dfinance.Shared.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Data;
using System.Transactions;

namespace Dfinance.Finance.Services
{
    public class BudgetingService : IBudgetingService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<BudgetingService> _logger;
        public BudgetingService(DFCoreContext context, IAuthService authService, ILogger<BudgetingService> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
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
        private CommonResponse FillMaVoucher(int? VoucherId, int? PageId)
        {
            VoucherView data = null;
            if (PageId == null)
            {
                var result = _context.VoucherView
                    .FromSqlRaw($"EXEC LeftGridMasterSP @Criteria ='FillMaVouchers', @Id = '{VoucherId}'").AsEnumerable()
                    .FirstOrDefault();
                if (result != null)
                    data = result;
            }
            else
            {
                var result = _context.VoucherView
                    .FromSqlRaw($"EXEC LeftGridMasterSP @Criteria ='FillMaVouchersUsingPageID', @Id = '{PageId}'").AsEnumerable()
                    .FirstOrDefault();
                data = result;
            }
            return CommonResponse.Ok(data);
        }
        //fills the left grid master atble
        public CommonResponse FillMaster(int? TransId, int? PageId, int? voucherId)
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;
                object data = null;
                var res = FillMaVoucher(voucherId, PageId);
                if (TransId == null)
                {
                    var result = _context.FillVoucher
                      .FromSqlRaw($"EXEC LeftGridMasterSP @Criteria ='FillVoucher', @BranchID = '{branchId}',@MaPageMenuID='{PageId}'").ToList();
                    data = result;
                }
                else
                {
                    var result = _context.FillVouTranId
                    .FromSqlRaw($"EXEC VoucherSP @Criteria ='FillVoucherByTransactionID', @TransactionID = '{TransId}'").ToList();
                    data = result;
                }
                return CommonResponse.Ok(new { FillMavouchers = res, TransData = data });
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }
        
        public CommonResponse FillProfitLossBalsheet(bool? profitLoss, bool? balsheet)
        {
            try
            {
                string criteria = null;
                if (profitLoss == true)
                    criteria = "PandLAccounts";
                else
                    criteria = "BalanceSheetAccounts";
                var result = _context.ReadViewAlias.FromSqlRaw("Exec AccountsSP @Criteria={0}", criteria).ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Fill Profit/Loss or BalaceSheet");
                return CommonResponse.Error(ex);
            }
        }
        private CommonResponse SaveTransactions(BudgetingDto budgetDto, int pageId, int voucherId,bool? cancel=false)
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
            if(budgetDto.TransactionId==0 || budgetDto.TransactionId==null)
            {
                var voucherNoCheck = _context.FiTransaction.Any(t => t.VoucherId == voucherId && t.TransactionNo == budgetDto.VoucherNo);
                if (voucherNoCheck)
                    return CommonResponse.Error("VoucherNo already exists");
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
                               criteria, budgetDto.VoucherDate, budgetDto.VoucherDate, voucherId, null, budgetDto.VoucherNo, IsPostDated, currencyId, exchangeRate,
                               null, null, null, branchId, null, null, budgetDto.Type.Value, budgetDto.EndDate, null, budgetDto.Narration, userId, null, AddedDate, budgetDto.StartDate,
                               ApprovalStatus, null, null, Status, IsAutoEntry, Posted, Active, Cancelled, null, budgetDto.Name, null, null, pageId, newId);

                transId = (int)newId.Value;
            }
            else
            {
                if(cancel==true)
                {
                    Cancelled= true;
                    editedBy = userId;
                }
                var voucherNoCheck = _context.FiTransaction.Any(t => t.VoucherId == voucherId && t.TransactionNo == budgetDto.VoucherNo && t.Id!=budgetDto.TransactionId);
                if (voucherNoCheck)
                    return CommonResponse.Error("VoucherNo already exists");
                criteria = "UpdateTransactions";
                _context.Database.ExecuteSqlRaw(
                    "EXEC VoucherSP @Criteria = {0}, @Date = {1}, @EffectiveDate = {2}, @VoucherID = {3}, @MachineName = {4}, " +
                    "@TransactionNo = {5}, @IsPostDated = {6}, @CurrencyID = {7}, @ExchangeRate = {8}, @RefPageTypeID = {9}, " +
                    "@RefPageTableID = {10}, @ReferenceNo = {11}, @CompanyID = {12}, @FinYearID = {13}, @InstrumentType = {14}, " +
                    "@InstrumentNo = {15}, @InstrumentDate = {16}, @InstrumentBank = {17}, @CommonNarration = {18}, @AddedBy = {19}, " +
                    "@ApprovedBy = {20}, @AddedDate = {21}, @ApprovedDate = {22}, @ApprovalStatus = {23}, @ApproveNote = {24}, @Action = {25}, @Status = {26}, " +
                    "@IsAutoEntry = {27}, @Posted = {28}, @Active = {29}, @Cancelled = {30}, @AccountID = {31}, @Description = {32}, " +
                    "@RefTransID = {33}, @CostCentreID = {34}, @PageID = {35}, @ID = {36}, @EditedBy = {37}, @EditedDate = {38}",
                    criteria,budgetDto.VoucherDate,budgetDto.VoucherDate,voucherId, null, budgetDto.VoucherNo,IsPostDated,currencyId,exchangeRate, null,null,null,
                    branchId, null,null,budgetDto.Type.Value, budgetDto.EndDate,null,budgetDto.Narration, userId,null, AddedDate, budgetDto.StartDate,ApprovalStatus,
                    null,null,Status, IsAutoEntry, Posted,Active, Cancelled,null, budgetDto.Name, null,null, pageId, budgetDto.TransactionId, editedBy,DateTime.Now
                );

                transId = (int)budgetDto.TransactionId;

                var budgetRemove=_context.BudgetMonth.Where(b=>b.TransactionId==transId).ToList();
                _context.BudgetMonth.RemoveRange(budgetRemove);
                _context.SaveChanges();

            }
            return CommonResponse.Ok(transId);
        }

        private CommonResponse SaveBudgetMonth(BudgetingDto budgetDto,int transId)
        {
            var criteria1 = "InsertBudget";
            SqlParameter newBudget = new SqlParameter("@NewID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            foreach (var account in budgetDto.BudgetAccounts)
            {
                _context.Database.ExecuteSqlRaw("EXEC BudgetingNewSP @Criteria={0},@TransactionID={1},@AccountID={2},@January={3}," +
                    "@February={4},@March={5},@April={6},@May={7},@June={8},@July={9},@August={10},@September={11},@October={12}," +
                    "@November={13},@December={14},@Amount={15},@NewID={16} OUTPUT",
                    criteria1, transId, account.Account.ID, account.January, account.February, account.March, account.April, account.May, account.June,
                    account.July, account.August, account.September, account.October, account.November, account.December, account.Amount, newBudget);
            }
            return CommonResponse.Ok();
        }

        public CommonResponse SaveBudget(BudgetingDto budgetDto, int pageId,int voucherId)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 2))
            {
                return PermissionDenied("Save Budget");
            }
            using (var transactionScope = new TransactionScope())
            {
                try
                {         
                   
                    if (budgetDto != null)                     
                    {     
                        int transId=(int)SaveTransactions(budgetDto,pageId,voucherId).Data;
                        if (budgetDto.BudgetAccounts != null)
                            SaveBudgetMonth(budgetDto, transId);                        
                    }
                    transactionScope.Complete();
                    _logger.LogInformation("Budget Saved Successfully");
                    return CommonResponse.Created("Budget Saved Successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to save Budgeting");
                    transactionScope.Dispose();
                    return CommonResponse.Error(ex);
                }
            }  
        }
      public CommonResponse DeleteBudget(BudgetingDto budgetDto,int pageId,int voucherId, bool? cancel = false)
        {
            if (!_authService.IsPageValid(pageId))
            {
                return PageNotValid(pageId);
            }
            if (!_authService.UserPermCheck(pageId, 4))
            {
                return PermissionDenied("Delete Budget");
            }
            var transid = _context.FiTransaction.Any(x => x.Id == budgetDto.TransactionId);
            if (!transid)            
                return CommonResponse.NotFound("Transaction Not Found");
            
            if (cancel==true)
            {                
                SaveTransactions(budgetDto, pageId, voucherId,true);
                return CommonResponse.Ok("Cancelled successfully");
            }
            else 
            {
                string criteria = "DeleteTransactions";
                _context.Database.ExecuteSqlRaw("EXEC VoucherSP @Criteria={0}, @ID={1}",criteria, budgetDto.TransactionId);
                _logger.LogError("Failed to Delete Budgeting");
                return CommonResponse.Ok("Budget Deleted successfully");
            }
        }
    }
}
