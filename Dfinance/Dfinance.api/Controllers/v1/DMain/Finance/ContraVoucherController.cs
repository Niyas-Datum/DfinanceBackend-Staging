using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Vouchers;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [ApiController]
    [Authorize]
    public class ContraVoucherController : BaseController
    {
        private readonly IContraVoucherService _contraVoucher;
        public ContraVoucherController(IContraVoucherService contraVoucher)
        {
            _contraVoucher = contraVoucher;
        }

        [HttpGet(FinRoute.ContraVoucher.Fill)]
        public IActionResult FillAccCode()
        {
            try
            {
                var result = _contraVoucher.FillAccCode();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost(FinRoute.ContraVoucher.Save)]
        public IActionResult SaveContraVou(ContraDto contraDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _contraVoucher.SaveContraVou(contraDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch(FinRoute.ContraVoucher.Update)]
        public IActionResult UpdateContraVou(ContraDto contraDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _contraVoucher.UpdateContraVou(contraDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(FinRoute.ContraVoucher.Delete)]
        public IActionResult DeleteContraVou(int TransId, int pageId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _contraVoucher.DeleteContraVou(TransId, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
