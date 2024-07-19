using Dfinance.api.Authorization;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [Authorize]
    [ApiController]
    public class JournalVoucherController : ControllerBase
    {
        private readonly IJournalVoucherService _journalVoucher;
        public JournalVoucherController(IJournalVoucherService journalVoucher)
        {
            _journalVoucher = journalVoucher;
        }
        [HttpGet(FinRoute.JournalVoucher.fillAcc)]
        [SwaggerOperation(Summary = "Fills the Account Popup. (Input String 'BranchAccounts' for Journal Voucher")]
        public IActionResult FillAccounts(string? criteria = null)
        {
            try
            {
                var result = _journalVoucher.FillAccounts(criteria);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(FinRoute.JournalVoucher.save)]
        [SwaggerOperation(Summary = "Saves Journal Voucher(PageId=68, VoucherId=6)")]
        public IActionResult SaveJournalVoucher(int pageId, int voucherId, JournalDto journalDto)
        {
            var result = _journalVoucher.SaveJournalVoucher(pageId,voucherId,journalDto);
            return Ok(result);
        }
        [HttpPost(FinRoute.JournalVoucher.update)]
        [SwaggerOperation(Summary = "Updates Journal Voucher(PageId=68, VoucherId=6)")]
        public IActionResult UpdateJournalVoucher(int pageId, int voucherId, JournalDto journalDto)
        {
            var result = _journalVoucher.UpdateJournalVoucher(pageId, voucherId, journalDto);
            return Ok(result);
        }
    }
}
