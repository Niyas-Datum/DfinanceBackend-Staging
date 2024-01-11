using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Dto.Finance;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [ApiController]
    [Authorize]
    public class FinanceYearController :BaseController
    {
        public readonly IFinanceYearService _financeYearService;
        public FinanceYearController(IFinanceYearService financeYearService)
        {
            _financeYearService = financeYearService;
        }
        /// <summary>
        /// @windows: -Finance/masters                 
        /// </summary>
        ///  <returns>FinanceYear Details</returns>
        [HttpGet(FinRoute.FinanceYear.FillAllFinanceYear)]
        public IActionResult FillAllFinanceYear()
        {
            try
            {

                var data = _financeYearService.FillAllFinanceYear();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Finance/masters                 
        /// </summary>
        /// <param FinYearID="Id">[Id]</param>
        /// <returns>FinanceYear Details</returns>
        [HttpGet(FinRoute.FinanceYear.FillAllFinanceYearById)]
        public IActionResult FillAllFinanceYearById(int Id)
        {
            try
            {
                var result = _financeYearService.FillFinanceYearById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Finance/masters                 
        /// </summary>    
        /// <param name="FinanceYearDto"> [FinancialYear : , StartDate:,EndDate:,LockTillDate:,Status:]</param> 
        [HttpPost(FinRoute.FinanceYear.SaveFinanceYear)]
        public IActionResult SaveFinanceYear([FromBody]FinanceYearDto financeYearDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var data = _financeYearService.SaveFinanceYear(financeYearDto);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Finance/masters               
        /// </summary>
        /// <param name="FinanceYearDto"> [Id:,FinancialYear : , StartDate:,EndDate:,LockTillDate:,Status:]</param>
        [HttpPatch(FinRoute.FinanceYear.UpdateFinanceYear)]
        public IActionResult UpdateFinanceYear([FromBody] FinanceYearDto financeYearDto, int Id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _financeYearService.UpdateFinanceYear(financeYearDto, Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Finance/masters                 
        /// </summary>
        /// <param> [Id] </param> 
        [HttpDelete(FinRoute.FinanceYear.DeleteFinanceYear)]
        public IActionResult DeleteFinanceYear(int Id)
        {
            try
            {
                var result = _financeYearService.DeleteFinanceYear(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
