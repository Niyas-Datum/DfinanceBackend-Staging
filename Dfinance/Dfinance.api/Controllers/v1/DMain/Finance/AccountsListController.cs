using Dfinance.api.Authorization;
using Dfinance.Application.Services.Finance;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.Core.Domain;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using static Dfinance.Shared.Routes.v1.ApiRoutes;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    public class AccountsListController : Controller
    {
        private readonly IAccountList _AccountListService;

        public AccountsListController(IAccountList accountsListService)
        {
            _AccountListService = accountsListService;
        }
        [HttpPost(FinRoute.AccountsList.SaveAccountsList)]
        public IActionResult SaveAccountsList([FromBody] List<AccountsListDto> accountList)
        {
            try
            {
                var result = _AccountListService.SaveAccountsList(accountList);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpGet(FinRoute.AccountsList.FillAccountList)]
        public IActionResult FillAccountList(int ListId)
        {
            try
            {

                var data = _AccountListService.FillAccountList(ListId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(FinRoute.AccountsList.FillAccountListByID)]
        public IActionResult FillAccountListByID(int Id)
        {
            try
            {

                var data = _AccountListService.FillAccountListByID(Id);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(FinRoute.AccountsList.AccountListPopUP)]
       // [AllowAnonymous]
        public IActionResult AccountListPopUp()
        {
            try
            {

                var data = _AccountListService.AccountListPopUp();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet(ApiRoutes.AccountsList.DeleteAccountList)]
        //public IActionResult DeleteAccountList(int Id)
        //{
        //    try
        //    {

        //        var data = _AccountListService.DeleteAccountList( Id);

        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}