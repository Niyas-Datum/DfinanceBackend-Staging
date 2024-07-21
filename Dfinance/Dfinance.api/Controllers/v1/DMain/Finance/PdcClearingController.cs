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
    public class PdcClearingController : BaseController
    {
        private readonly IPdcClearingService _pdcClearing;
        public PdcClearingController(IPdcClearingService pdcClearing)
        {
            _pdcClearing = pdcClearing;
        }

        [HttpPost(FinRoute.PdcClearing.Save)]
        public IActionResult SavePdcClearing(PdcClearingDto PDCDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _pdcClearing.SavePdcClearing(PDCDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(FinRoute.PdcClearing.Fill)]
        public IActionResult FillChequeDetails(int BankId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _pdcClearing.FillChequeDetails(BankId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch(FinRoute.PdcClearing.Update)]
        public IActionResult UpdatePDCclearing(PdcClearingDto pdcClearingDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _pdcClearing.UpdatePDCclearing(pdcClearingDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpPatch(FinRoute.PdcClearing.Delete)]
        //public IActionResult DeletePdcClearing(PdcClearingDto pdcDto, int pageId, int voucherId, bool? cancel = false)
        //{
        //    try
        //    {
        //        var data = _pdcClearing.DeletePdcClearing(pdcDto, pageId, voucherId, cancel);
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
