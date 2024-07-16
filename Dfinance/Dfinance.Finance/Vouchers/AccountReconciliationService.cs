using AutoMapper;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Finance;
using Dfinance.Core.Views.General;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Services.Interface;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dfinance.Shared.Routes.v1.ApiRoutes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dfinance.Finance.Vouchers
{
    public class AccountReconciliationService : IAccountReconciliationService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<AccountReconciliationService> _logger;
        private readonly IMapper _mapper;
        private readonly IFinancePaymentService _paymentService;
        private readonly DataRederToObj _rederToObj;
        public AccountReconciliationService(DFCoreContext context, IAuthService authService, ILogger<AccountReconciliationService> logger, IMapper mapper, IFinancePaymentService paymentService, DataRederToObj rederToObj)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _paymentService = paymentService;
            _rederToObj = rederToObj;
        }

        public CommonResponse AccountPopUp()
        {
            try
            {
                int BranchID = _authService.GetBranchId().Value;
                var accounts = (from acc in _context.FiMaAccounts
                                join BrAcc in _context.FiMaBranchAccounts on
                                acc.Id equals BrAcc.AccountId
                                where acc.IsGroup == false && BrAcc.BranchId == BranchID
                                select new
                                {
                                    AccountCode = acc.Alias,
                                    AccountName = acc.Name,
                                    ID = acc.Id,
                                }).ToList();
                return CommonResponse.Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error("An error occurred while fetching accounts.");
            }
        }

        //search
        public CommonResponse FillAccountReconcilation(DateOnly FromDate, DateOnly ToDate, int? AccountID)
        {
            try
            {
                string criteria = "FillAccountReconcilation";
                int BranchID = _authService.GetBranchId().Value;
               
                bool pending = true;
                AccountReconcilationView fill = new AccountReconcilationView();
                _context.Database.GetDbConnection().Open();

                using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandText = $"Exec AccountReconcilationSP @Criteria='{criteria}',@FromDate='{FromDate}',@ToDate='{ToDate}',@AccountID={(AccountID.HasValue ? AccountID.ToString() : "null")} ,@VTypeID=null,@BranchID={BranchID} ,@Pending={pending}";
                    using (var reader = dbCommand.ExecuteReader())
                    {
                        fill.accountReconcilViews = _rederToObj.Deserialize<AccountReconcilView>(reader).ToList();
                        reader.NextResult();
                        fill.accountRecoView = _rederToObj.Deserialize<AccountRecoView>(reader).FirstOrDefault();
                        reader.NextResult();
                    }

                    _logger.LogInformation("Successfully filled both");
                    return CommonResponse.Ok(fill);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }

        public CommonResponse UpdateAccountReconcilation(int TranEntryId,DateTime BankDate)
        {
            try
            {
                string criteria = "UpdateAccountReconcilation";
                int BranchID = _authService.GetBranchId().Value;
                var formattedDate = BankDate.ToString("yyyy-MM-dd HH:mm:ss");
                var TranId = _context.FiTransactionEntries.Where(i=>i.Id == TranEntryId).Select(i => i.Id).SingleOrDefault();
                if (TranId ==0 ) { return CommonResponse.Error("TranId not found"); }
                else
                {
                    var result = _context.Database.ExecuteSqlRaw("EXEC AccountReconcilationSP @Criteria ={0},@ID ={1},@BankDate ={2},@BranchID ={3}",
                                    criteria, TranEntryId, formattedDate, BranchID);
                        _logger.LogInformation("AccountReconcilation Updated successfully");
                        return CommonResponse.Ok("AccountReconcilation Updated successfully");
                        
                }

            }

            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        
    }
}
