using Dfinance.api.Framework;
using Dfinance.Application.Services.Finance.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dfinance.api.Authorization;
using Dfinance.Finance.Vouchers.Interface;
using Dfinance.Application.Services.Finance;
using Dfinance.Shared.Routes.v1;
using Dfinance.DataModels.Dto.Finance;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [ApiController]
    [Authorize]

    public class CloseVoucherController : BaseController
    {
        private readonly ICloseVoucherService _closeVoucherService;

        public CloseVoucherController(ICloseVoucherService closeVoucherService)
        {
            _closeVoucherService = closeVoucherService;
        }
        [HttpGet(FinRoute.CloseVoucher.LoadData)]
        public IActionResult GetLoadData()
        {
            try
            {
                var view = _closeVoucherService.GetLoadData();
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(FinRoute.CloseVoucher.Fill)]
        public IActionResult Fill(CloseVoucherDto closeVoucher)
        {
            try
            {
                var view = _closeVoucherService.Fill(closeVoucher);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(FinRoute.CloseVoucher.Apply)]
        public IActionResult CloseVoucherUpdate(int PageId,List<int> Ids)
        {
            try
            {
                var view = _closeVoucherService.CloseVoucherUpdate(PageId,Ids);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
