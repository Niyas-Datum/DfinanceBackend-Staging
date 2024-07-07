using Dfinance.api.Framework;
using Dfinance.Application.Services.General;
using Dfinance.Core.Views.General;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Routes;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.General
{
    public class RecallVoucherController : BaseController
    {
        private readonly IRecallVoucherService _recallVoucherService;
        public RecallVoucherController(IRecallVoucherService recallVoucherService)
        {

            _recallVoucherService = recallVoucherService;
        }
        [HttpGet(ApiRoutes.RecallVoucher.GatData)]
        public IActionResult GetData()
        {
            try
            {
                var data = _recallVoucherService.GetData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.RecallVoucher.GetCancelVch)]
        public IActionResult FillCancelledVouchers(int? accountId, int? vTypeId, DateTime? dateFrom, DateTime? dateUpTo, string? transactionNo)
        {
            try
            {
                var data = _recallVoucherService.FillCancelledVouchers(accountId,vTypeId,dateFrom,dateUpTo, transactionNo);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(ApiRoutes.RecallVoucher.UpdateVch)]
        public IActionResult ApplyUpdateVoucher(string? Reason, int[] voucherID)
        {
            try
            {
                var data = _recallVoucherService.ApplyUpdateVoucher(Reason, voucherID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
