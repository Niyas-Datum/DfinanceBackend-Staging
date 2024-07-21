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
    public class OpeningVoucherController : BaseController
    {
        private readonly IOpeningVoucherService _openingVoucher;
        public OpeningVoucherController(IOpeningVoucherService openingVoucher)
        {
            _openingVoucher = openingVoucher;
        }

        [HttpPost(FinRoute.OpeningVoucher.Save)]
        public IActionResult SaveOpeningVou(OpeningVoucherDto openVouDto, int PageId, int VoucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _openingVoucher.SaveOpeningVoucher(openVouDto, PageId, VoucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(FinRoute.OpeningVoucher.Update)]
        public IActionResult UpdateOpeningVoucher(OpeningVoucherDto openVouDto, int PageId, int VoucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _openingVoucher.UpdateOpeningVoucher(openVouDto, PageId, VoucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(FinRoute.OpeningVoucher.Delete)]
        public IActionResult DeleteOpeningVou(OpeningVoucherDto openVouDto, int pageId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _openingVoucher.DeleteOpeningVou(openVouDto,pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
