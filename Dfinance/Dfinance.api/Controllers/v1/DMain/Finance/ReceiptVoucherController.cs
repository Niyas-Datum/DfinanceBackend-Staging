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
    public class ReceiptVoucherController : BaseController
    {
        private readonly IReceiptVoucherService _receiptVoucherService;
        public ReceiptVoucherController(IReceiptVoucherService receiptVoucherService)
        {

            _receiptVoucherService = receiptVoucherService;

        }
        [HttpGet(FinRoute.ReceiptVoucher.FillVoucher)]
        public IActionResult FillMaVoucher(int? VoucherId, int? PageId)
        {
            try
            {
                var result = _receiptVoucherService.FillMaVoucher(VoucherId, PageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet(FinRoute.ReceiptVoucher.FillMaster)]
        public IActionResult FillMaster(int? TransId, int? PageId)
        {
            try
            {
                var result = _receiptVoucherService.FillMaster(TransId, PageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost(FinRoute.ReceiptVoucher.Save)]
        public IActionResult SaveReceiptVou(FinanceTransactionDto receiptVoucherDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _receiptVoucherService.SaveReceiptVou(receiptVoucherDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch(FinRoute.ReceiptVoucher.Update)]
        public IActionResult UpdateReceiptVou(FinanceTransactionDto paymentVoucherDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _receiptVoucherService.UpdateReceiptVoucher(paymentVoucherDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(FinRoute.ReceiptVoucher.Delete)]
        public IActionResult DeleteReceiptVoucher(int TransId, int pageId)
        {
            try
            {
                var result = _receiptVoucherService.DeleteReceiptVoucher(TransId, pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }







    }
}
