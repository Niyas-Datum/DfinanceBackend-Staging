using Dfinance.Application.Services.Finance.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Common;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Application.Services.Finance
{
    public class AccountListService : IAccountListService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<AccountListService> _logger;
        public AccountListService(DFCoreContext context, IAuthService authService, ILogger<AccountListService> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }
        public CommonResponse FillAccountListByID(int Id)
        {
            try
            {
                string criteria = "FillAccountListByID";
                var result = _context.FillAccountList.FromSqlRaw($"Exec AccountsListSP @Criteria='{criteria}',@ID='{Id}'").ToList();
                return CommonResponse.Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to Fill AccountList by ID");
                return CommonResponse.Error(ex.Message);
            }
        }

        public CommonResponse FillAccountList(int ListId)
        {
            try
            {
                if (ListId > 0)
                {
                    string criteria = "FillAccountList";
                    var result = _context.FillAccountList.FromSqlRaw($"Exec AccountsListSP @Criteria='{criteria}',@ListID='{ListId}'").ToList();
                    return CommonResponse.Ok(result);
                }
                else
                {
                    string criteria = "FillAccountsList";
                    var result = _context.DropDownViewDesc.FromSqlRaw($"Exec DropDownListSP @Criteria='{criteria}'").ToList();
                    return CommonResponse.Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to Fill AccountList");
                return CommonResponse.Error(ex.Message);
            }
        }

        public CommonResponse SaveAccountsList(AccountsListDto accountList)
        {
            try
            {
                int createdBranchId = _authService.GetBranchId().Value;
                string criteria = "InsertAccount";
                SqlParameter newIdAlias = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                var listRemove = _context.FiAccountsList.Where(b => b.ListId == accountList.List.Id).ToList();
                _context.FiAccountsList.RemoveRange(listRemove);
                _context.SaveChanges();
                foreach (var account in accountList.Accounts)
                {                                      
                    // Saving FiAccountList table                    
                    _context.Database.ExecuteSqlRaw("EXEC AccountsListSP @Criteria={0},@ListID={1},@AccountID={2},@BranchID={3},@NewID= {4} OUTPUT ",
                        criteria,
                        accountList.List.Id,
                        account.ID,
                        createdBranchId,
                        newIdAlias
                    );
                    var newId = (int)newIdAlias.Value;
                }
                _logger.LogInformation("Successfullt saved AccountList");
                return CommonResponse.Created("Saved");
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to Save AccountList");
                return CommonResponse.Error(ex.Message);
            }
        }
       
        public CommonResponse AccountListPopUp()
        {
            try
            {

                var data = _context.FiMaAccounts
               .Where(i => i.IsGroup == false && i.Active == true)
               .Select(accountsList => new ReadViewAlias
               {
                   Alias = accountsList.Alias,
                   Name = accountsList.Name,
                   ID = accountsList.Id
               })
               .ToList();

                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to Popup Accounts in AccountList");
                return CommonResponse.Error(ex.Message);
            }
        }       
    }
}



