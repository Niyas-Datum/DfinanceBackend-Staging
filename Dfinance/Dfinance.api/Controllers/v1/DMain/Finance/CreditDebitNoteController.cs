using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{

    [ApiController]
    [Authorize]
    public class CreditDebitNoteController : BaseController
    {
        private readonly ICreditDebitNoteService _creditnote;
        public CreditDebitNoteController(ICreditDebitNoteService creditNote)
        {
            _creditnote = creditNote;
        }
        [HttpGet(FinRoute.CreditNote.Fill)]
        public IActionResult FillParty(int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _creditnote.FillParty(voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(FinRoute.CreditNote.SaveDebitCredit)]
        public IActionResult SaveCreditDebitNote(DebitCreditDto debitCreditDto, int PageId, int VoucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _creditnote.SaveCreditDebitNote(debitCreditDto, PageId, VoucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch(FinRoute.CreditNote.UpdateDebitCredit)]
        public IActionResult UpdateCreditDebitNote(DebitCreditDto debitCreditDto, int PageId, int VoucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _creditnote.UpdateCreditDebitNote(debitCreditDto, PageId, VoucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(FinRoute.CreditNote.DeleteDebitCredit)]
        public IActionResult DeleteDebitCreditNote(int TransId, int pageId)
        {
            try
            {
                var data = _creditnote.DeleteDebitCreditNote(TransId, pageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch(FinRoute.CreditNote.Cancel)]
        public IActionResult CancelDebitCreditNote(int transId, string reason)
        {
            try
            {
                var data = _creditnote.CancelDebitCreditNote(transId,reason);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
