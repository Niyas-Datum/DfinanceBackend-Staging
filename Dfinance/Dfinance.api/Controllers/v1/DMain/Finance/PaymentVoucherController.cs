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
    public class PaymentVoucherController : BaseController
    {
       
        private readonly IPaymentVoucherService _paymentVoucherService;
        public PaymentVoucherController(IPaymentVoucherService paymentVoucherService)
        {
            
            _paymentVoucherService = paymentVoucherService;
            
        }
        [HttpGet(FinRoute.PaymentVoucher.FillAccCode)]
        public IActionResult FillAccCode()
        {
            try
            {
                var result = _paymentVoucherService.FillAccountCode();
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
           
        }
        [HttpGet(FinRoute.PaymentVoucher.Getsettings)]
        public IActionResult GetPayVocherSettings()
        {
            try
            {
                var result = _paymentVoucherService.GetPayVocherSettings();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost(FinRoute.PaymentVoucher.Save)]
        public IActionResult SavePayVou(PaymentVoucherDto paymentVoucherDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _paymentVoucherService.SavePayVou(paymentVoucherDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch(FinRoute.PaymentVoucher.Update)]
        public IActionResult UpdatePayVou(PaymentVoucherDto paymentVoucherDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _paymentVoucherService.UpdatePayVoucher(paymentVoucherDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(FinRoute.PaymentVoucher.Delete)]
        public IActionResult DeletePayVoucher(int TransId, int pageId)
        {
            try
            {
                var result = _paymentVoucherService.DeletePayVoucher(TransId, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
