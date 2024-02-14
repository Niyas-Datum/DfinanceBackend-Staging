using Dfinance.Application.Dto.Finance;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Common;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dfinance.Application.Services.Finance
{
    public class AccountListService : IAccountList
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;

        public AccountListService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
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
                return CommonResponse.Error(ex.Message);
            }
        }

        public CommonResponse SaveAccountsList(List<AccountsListDto> accountList)
        {
            try
            {
                int createdBranchId = _authService.GetBranchId().Value;

                foreach (var account in accountList)
                {
                    foreach (var acc in account.Accounts)
                    {
                        // Check whether the ListID already exists
                        var listExists = _context.FiAccountsList.Any(x => x.Id == account.List.Id);
                        if (!listExists)
                        {
                            return CommonResponse.Error("ListID does not exist");
                        }

                        // Check whether the AccountID already exists
                        var accountExists = _context.FiAccountsList.Any(x => x.Id == acc.ID);
                        if (accountExists)
                        {
                            return CommonResponse.Error("AccountID Already Exists");
                        }

                        if (account.Id == 0)
                        {
                            // Saving FiAccountList table
                            string criteria = "InsertAccount";
                            SqlParameter newIdAlias = new SqlParameter("@NewID", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };
                            _context.Database.ExecuteSqlRaw("EXEC AccountsListSP @Criteria={0},@ListID={1},@AccountID={2},@BranchID={3},@NewID= {4} OUTPUT ",
                                criteria,
                                account.List.Id,
                                acc.ID,
                                createdBranchId,
                                newIdAlias
                            );
                            var newId = (int)newIdAlias.Value;
                        }
                        else
                        {
                            // Updating existing record
                            string criteria = "UpdateAccount";
                            _context.Database.ExecuteSqlRaw("EXEC AccountsListSP @Criteria={0},@ListID={1},@AccountID={2},@BranchID={3},@ID= {4}  ",
                                criteria,
                                account.List.Id,
                                acc.ID,
                                createdBranchId,
                                account.Id
                            );
                        }
                    }
                }

                return CommonResponse.Created("Saved");
            }
            catch (Exception ex)
            {
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
                return CommonResponse.Error(ex.Message);
            }
        }
        //Not Used In windows
        //public CommonResponse DeleteAccountList( int Id)
        //{
        //    try
        //    {

        //        string msg = null;
        //        int CreatedBranchId = _authService.GetBranchId().Value;

        //        {
        //            string criteria = "DeleteAccount";

        //            var result = _context.Database.ExecuteSqlRaw($"Exec AccountsListSP @Criteria='{criteria}',@ID='{Id}'");
        //            msg = Id + " Deleted Successfully";
        //            return CommonResponse.Ok(msg);

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return CommonResponse.Error(ex.Message);
        //    }
        //}
    }
}
    
   

