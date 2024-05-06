using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.ChartOfAccount.Services.Finance.Interface;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [ApiController]
    [Authorize]
    public class ChartOfAccountsController : BaseController
    {
        private readonly IChartOfAccountsService _chartofaccounts;
        public ChartOfAccountsController(IChartOfAccountsService chartofaccounts)
        {
           _chartofaccounts = chartofaccounts;
        }
        /// <summary>
        /// @windows: -Finance/masters/ChartOfAccount   
        /// @field:  Account group
        /// </summary>
        ///  <returns> DropdownAccounts</returns>
        /// <summary>
        [HttpGet(FinRoute.Coa.AccountGroup)]
        public IActionResult DropdownAccounts()
        {
            try
            {
                var data = _chartofaccounts.DropdownAccounts();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// @windows: -Finance/masters/ChartOfAccount  
        /// @field:  load all accounts 
        /// </summary>
        ///  <returns> FillAccounts</returns>
        [HttpGet(FinRoute.Coa.Accountlist)]
        public IActionResult FillAccounts(int Id, bool tree)
        {
            try
            {
                var data = _chartofaccounts.FillAccounts(Id, tree);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        ///// @windows: -Finance/masters/ChartOfAccount 
        /////  @field:  edit and preview 
        ///// </summary>
        /////  <returns> FillAccountsById</returns>
        [HttpGet(FinRoute.Coa.AccountsById)]
        public IActionResult FillAccountsById(int Id)
        {
            try
            {
                var data = _chartofaccounts.FillAccountsById(Id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        ///// <summary>
        ///// @windows: -Finance/masters/ChartOfAccount     
        /////  @field:  account subgroup
        ///// </summary>
        /////  <returns> DropDownSubGroup&NextAccountCode</returns>
        [HttpGet(FinRoute.Coa.SubGroup)]
        public IActionResult DropDownSubGroup(int id, string Keyword = "")
        {
            try
            {
                var data = _chartofaccounts.DropDownSubGroup(id, Keyword);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// @windows: -Finance/masters/ChartOfAccount     
        ///  @field:  account category
        /// </summary>
        ///  <returns>DropDownAccountCategory </returns>
        [HttpGet(FinRoute.Coa.AccountCategory)]
        public IActionResult DropdownAccountCategory()
        {
            try
            {
                var data = _chartofaccounts.DropDownAccountCategory();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// @windows: -Finance/masters  
        ///  @field:  add new account 
        /// </summary>    
        /// <param name="ChartOfAccountsDto"> []</param> 
        [HttpPost(FinRoute.Coa.SaveAccount)]
        public IActionResult SaveAccount([FromBody] ChartOfAccountsDto accountsDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var data = _chartofaccounts.SaveAccount(accountsDto);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// @windows: -Finance/masters    
        ///  @field:  Update account
        /// </summary>    
        /// <param name="ChartOfAccountsDto"> []</param> 
        [HttpPatch(FinRoute.Coa.UpdateAccount)]
        public IActionResult UpdateAccounts([FromBody] ChartOfAccountsDto accountsDto, int Id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var data = _chartofaccounts.UpdateAccounts(accountsDto, Id);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// delete account 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete(FinRoute.Coa.DeleteAccount)]
        public IActionResult DeleteAccount(int Id)
        {
            try
            {
                var result = _chartofaccounts.DeleteAccount(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
