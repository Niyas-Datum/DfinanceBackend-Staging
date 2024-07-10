using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Finance.Vouchers;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [ApiController]
    [Authorize]
    public class AccountReconciliationController : BaseController
    {
       private readonly IAccountReconciliationService _accountReconciliationService;
        public AccountReconciliationController(IAccountReconciliationService accountReconciliationService)
        {
            _accountReconciliationService = accountReconciliationService;
            
        }
        [HttpGet(FinRoute.AccountReconciliation.AccPopup)]
        public IActionResult AccountPopup()
        {
            try
            {
                var result = _accountReconciliationService.AccountPopUp();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet(FinRoute.AccountReconciliation.FillAccountReconcilation)]
        public IActionResult FillAccountReconcilation(DateOnly FromDate, DateOnly ToDate, int? AccountID)
        {
            try
            {
                var result = _accountReconciliationService.FillAccountReconcilation(FromDate,ToDate, AccountID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet(FinRoute.AccountReconciliation.UpdateAccountReconcilation)]
        public IActionResult UpdateAccountReconcilation(int TranEntryId, DateTime BankDate)
        {
            try
            {
                var result = _accountReconciliationService.UpdateAccountReconcilation(TranEntryId, BankDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
