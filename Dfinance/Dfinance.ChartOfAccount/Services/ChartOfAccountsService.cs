using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.ChartOfAccount.Services.Finance.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;

namespace Dfinance.ChartOfAccount.Services.Finance
{
    public class ChartOfAccountsService : IChartOfAccountsService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private static int reqCount =0;

        public ChartOfAccountsService()
        {
            
        }
        public ChartOfAccountsService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        //called by ChartOfAccountsController/DropdownAccounts
        public CommonResponse DropdownAccounts()
        {
            try
            {
                string criteria = "FillAllAccountGroup";
                var result = _context.DropDownViewName.FromSqlRaw($"Exec DropDownListSP @Criteria='{criteria}'").ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        //called by ChartOfAccountsController/FillAccounts
        public CommonResponse FillAccounts(int Id, bool tree)
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;
                if (tree)
                {

                    string criteria = Id == 0 ? "FillFirstLevelAccounts" : "FillAccountRowsByParent";

                    var result = _context.ChartofAccView
                        .FromSqlRaw($"Exec AccountsSP @Criteria='{criteria}',@BranchID={branchId},@ID={Id}")
                        .ToList();

                    return CommonResponse.Ok(result);
                }
                else
                {
                    string criterialed = "FillAcoountMaster";
                    var result1 = _context.FillLedgers
                       .FromSqlRaw($"Exec AccountsSP @Criteria='{criterialed}',@BranchID={branchId}")
                       .ToList();

                    return CommonResponse.Ok(result1);
                }

            }
            catch (Exception ex)

            {
                return CommonResponse.Error(ex.Message);
            }
        }
        //called by ChartOfAccountsController/FillAccounts&NextCode
        public CommonResponse DropDownSubGroup(int id, string Keyword = "")
        {
            try
            {
                string criteria = "FillSubgroups";
                var result = _context.DropDownViewDesc.FromSqlRaw($"Exec AccountsSP @Criteria='{criteria}', @AccountID='{id}'").ToList();

                //string nextcodecriteria = "NextAccountCode";
                //var nextcode = _context.NextCodeAccountCode.FromSqlRaw($"EXEC AccountsSP @Criteria='{nextcodecriteria}', @ID={id}").ToList();
                string nextcode = GetNextCode(id);
             
                var response = new
                {
                    Subgroup = result,
                    NextCode = nextcode
                };

                return CommonResponse.Ok(response);


            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        //called by ChartOfAccountsController/DropDownAccountCategory
        public CommonResponse DropDownAccountCategory()
        {
            try
            {
                string criteria = "FillAccountCategory";
                var result = _context.DropDownViewDesc.FromSqlRaw($"Exec AccountsSP @Criteria='{criteria}'").ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        //called by ChartOfAccountsController/SaveAcccounts

        public CommonResponse SaveAccount(ChartOfAccountsDto accountsDto)
        {
            try
            {
                //int BranchId = _authService.GetBranchId().Value;
                //int CreatedBy = _authService.GetId().Value;
                int BranchId = 1;
                int CreatedBy = 118;
                int? CreatedBranchId = null;
                if (accountsDto.SubGroup != null )
                {
                  var c =  _context.FiMaSubGroups.Any(u => u.Id == accountsDto.SubGroup.Id);
                    if (!c)
                    {
                        return CommonResponse.Error("Error: Sub group not exist");
                    }
                }
               


                if (!_context.FiMaAccountCategory.Any(u => u.Id == accountsDto.AccountCategory.Id)
                    || !_context.FiMaAccountGroup.Any(u => u.Id == accountsDto.Group.Id)
                    )
                    return CommonResponse.Error("Error: Kindly check account details");

                //CODE Validation
                var already = _context.FiMaAccounts.FirstOrDefault(x => x.Alias == accountsDto.AccountCode);

                if (already != null)
                {
                    return CommonResponse.Error("Error: Account code already exist.");
                }

                var numeric = new Regex("^[0-9]+$");
                if (numeric.IsMatch(accountsDto.AccountCode))
                {
                    var newcode = GetNextCode(accountsDto.Group?.Id);
                    int lenDiff = Math.Abs(accountsDto.AccountCode.Length - newcode.Length);
                    if (lenDiff > 2)
                    {
                        if (reqCount == 1)
                        {
                            reqCount = 0;
                        }
                        else
                        {
                            reqCount = 1;
                            return CommonResponse.Error("Warning: Account code exceeding min length");
                        }
                    }
                }

                if (accountsDto.IsGroup == false)
                {
                    CreatedBranchId = _authService.GetBranchId().Value;
                }

                string criteria = "InsertAccounts";
                int? AccountTypeID = null;
                int? level = null;

                int ParentId = 0;
                if (accountsDto.Group.Id != null)
                {

                    var fimaAccount = _context.FiMaAccounts
                        .Where(i => i.Id == accountsDto.Group.Id)
                        .FirstOrDefault();

                    if (fimaAccount != null)
                    {

                        level = fimaAccount.Level + 1;
                    }
                    else
                    {
                        return CommonResponse.Error("Account not found in FimaAccounts.");
                    }
                    ParentId = fimaAccount.Id;
                }


                SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                _context.Database.ExecuteSqlRaw("Exec AccountsSP @Criteria={0},@Name={1}," +
                    "@Alias={2},@Narration={3},@AccountTypeID={4},@IsBillwise={5}," +
                    "@IsItemwise={6},@Active={7},@IsCostCentre={8},@IsGroup={9}," +
                    "@CreatedBranchID={10},@CreatedBy={11},@Parent={12}, @Level={13}," +
                    "@AccountGroup={14},@SubGroup={15},@AccountCategory={16}," +
                    "@PreventExtraPay={17},@AlternateName={18},@BranchID={19},@NewID={20} OUTPUT",
                                                                  criteria,
                                                                  accountsDto.AccountName,
                                                                  accountsDto.AccountCode,
                                                                  accountsDto.Narration,
                                                                  AccountTypeID,
                                                                  accountsDto.MaintainBillwise,
                                                                  accountsDto.MaintainIteamWise,
                                                                  accountsDto.Active,
                                                                  accountsDto.MaintainCostCentre,
                                                                  accountsDto.IsGroup,
                                                                  CreatedBranchId,
                                                                  CreatedBy,
                                                                  ParentId, level,
                                                                  accountsDto.Group.Id, accountsDto.SubGroup.Id, accountsDto.AccountCategory.Id, accountsDto.PreventExtraPay, accountsDto.AlternateName, BranchId, newId);
                int AccountId = (int)newId.Value;
                return CommonResponse.Created( new { msg = "Account " + accountsDto.AccountName + " is Created Successfully", Id = AccountId });
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }
        //called by ChartOfAccountsController/UpdateAcccounts
        public CommonResponse UpdateAccounts(ChartOfAccountsDto accountsDto, int Id)
        {
            try
            {
                string mesg = null;
                int? CreatedBranchId = null;


                // Validate account categories, groups, and subgroups
                if (!_context.FiMaAccountCategory.Any(u => u.Id == accountsDto.AccountCategory.Id) ||
                    !_context.FiMaAccountGroup.Any(u => u.Id == accountsDto.Group.Id) ||
                    !_context.FiMaSubGroups.Any(u => u.Id == accountsDto.SubGroup.Id))
                {
                    return CommonResponse.Error("Error: Account Category does not exist.");
                }

                // Check if the account code already exists
                var already = _context.FiMaAccounts.FirstOrDefault(x => x.Alias == accountsDto.AccountCode);
                if (already != null)
                {
                    return CommonResponse.Error("Error: Account code already exists.");
                }

                var numeric = new Regex("^[0-9]+$");
                if (numeric.IsMatch(accountsDto.AccountCode))
                {
                    var newcode = GetNextCode(accountsDto.Group.Id);
                    int lenDiff = Math.Abs(accountsDto.AccountCode.Length - newcode.Length);
                    if (lenDiff > 2)
                    {
                        if (reqCount == 1)
                        {
                            reqCount = 0;
                        }
                        else
                        {
                            reqCount = 1;
                            return CommonResponse.Error("Warning: Account code exceeding min length");
                        }
                    }
                }
                if (!accountsDto.IsGroup)
                {
                    CreatedBranchId = _authService.GetBranchId().Value;
                }
                var accounts = _context.FiMaAccounts.Find(Id);
                if (accounts == null)
                {
                    return CommonResponse.NotFound("Accounts Not Found");
                }
                bool finalaccount = accounts.FinalAccount;


                if (accounts == null)
                {
                    mesg = "Accounts Not Found";
                    return CommonResponse.NotFound(mesg);
                }

                DateTime date = DateTime.Now;
                int? AccountTypeID = null;
                int CreatedBy = _authService.GetId().Value;


                if (accountsDto.IsGroup == false)
                {
                    CreatedBranchId = _authService.GetBranchId().Value;
                }

                int? level = null;
                int ParentId = 0;

                if (accountsDto.Group.Id != null)
                {
                    var fimaAccount = _context.FiMaAccounts
                        .Where(i => i.Id == accountsDto.Group.Id)
                        .FirstOrDefault();

                    if (fimaAccount != null)
                    {
                        level = fimaAccount.Level + 1;
                    }
                    else
                    {
                        return CommonResponse.Error("Account not found in FimaAccounts.");
                    }

                    ParentId = fimaAccount.Id;
                }

                var criteria = "UpdateAccounts";
                _context.Database.ExecuteSqlRaw("Exec AccountsSP @Criteria={0},@Name={1},@Alias={2}," +
                    "@Narration={3},@AccountTypeID={4},@IsBillwise={5},@IsItemwise={6},@Active={7}," +
                    "@IsCostCentre={8},@IsGroup={9},@CreatedBranchID={10},@CreatedBy={11}," +
                    "@Parent={12}, @Level={13},@AccountGroup={14},@SubGroup={15}," +
                    "@AccountCategory={16},@PreventExtraPay={17},@AlternateName={18}," +
                    "@ID={19},@Date={20},@CreatedOn={21},@BranchID={22},@FinalAccount={23}",
                    criteria, accountsDto.AccountName, accountsDto.AccountCode,
                    accountsDto.Narration, AccountTypeID, accountsDto.MaintainBillwise,
                    accountsDto.MaintainIteamWise, accountsDto.Active, accountsDto.MaintainCostCentre,
                    accountsDto.IsGroup, CreatedBranchId, CreatedBy, ParentId, level, accountsDto.Group.Id,
                    accountsDto.SubGroup.Id, accountsDto.AccountCategory.Id, accountsDto.PreventExtraPay,
                    accountsDto.AlternateName, Id, date, date, CreatedBranchId, finalaccount);

                return CommonResponse.Created("Account " + accountsDto.AccountName + " is Updated Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }

        //called by ChartOfAccountsController/FillAcccountsById
        public CommonResponse FillAccountsById(int Id)
        {
            try
            {
                string msg = null;
                var accounts = _context.FiMaAccounts.Find(Id);
                if (accounts == null)
                {
                    msg = "Accounts Not Found";
                    return CommonResponse.NotFound(msg);
                }
                string criteria = "FillAccounts";
                var result = _context.ChartofAccViewById.FromSqlRaw($"Exec AccountsSP @Criteria='{criteria}', @ID='{Id}'").ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse DeleteAccount(int Id)
        {
            try
            {
                string msg = null;
                var acc = _context.FiMaAccounts.Where(i => i.Id == Id).
                    Select(i => i.Name).
                    SingleOrDefault();
                if (acc == null)
                {
                    msg = "Account Not Found";
                    return CommonResponse.NotFound(msg);
                }

                string criteria = "DeleteAccounts";
                msg = "Accounts " + acc + " is Deleted Successfully";
                var result = _context.Database.ExecuteSqlRaw($"EXEC AccountsSP @Criteria='{criteria}', @CurrencyID={Id}");
                return CommonResponse.Ok(msg);
            }

            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }



        private string GetNextCode(int? Id)
        {
            try
            {
                var data = _context.FiMaAccounts
                    .Where(x=>x.AccountGroup == Id)
                    .Select(x => stringToNullableDecimal(x.Alias)).AsEnumerable().Where(x => x.HasValue).Max();


            //var result = _context.AccountCodeView
            //                .FromSqlRaw($"EXEC AccountsSP @Criteria='NextAccountCode', @ID={Id}")
            //                .AsEnumerable().FirstOrDefault().AccountCode;
                return data.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        Func<string, decimal?> stringToNullableDecimal = s =>
        {
            decimal temp;
            if (decimal.TryParse(s, out temp))
            {
                return temp;
            }
            return null;
        };
    }
}

    
