using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Finance.Statements.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [Authorize]
    [ApiController]
    public class DayBookController : BaseController
    {
        private readonly IDayBookService _dayBook;
        public DayBookController(IDayBookService dayBook)
        {
            _dayBook = dayBook;
        }
        [HttpGet(FinRoute.DayBook.voucherUser)]
        public IActionResult FillVoucherAndUser()
        {
            try
            {
                var result = _dayBook.FillVoucherAndUser();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(FinRoute.DayBook.fill)]
        public IActionResult FillDayBook(DayBookDto dayBookDto, int pageId)
        {
            try
            {
                var result = _dayBook.FillDayBook(dayBookDto,pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
