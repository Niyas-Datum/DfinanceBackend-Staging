using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [ApiController]
    [Authorize]
    //[Route()]
    public class CurrencyController : BaseController
    {
        private readonly ICurrencyService _currencyService;
        public CurrencyController(ICurrencyService currency)
        {
            _currencyService = currency;
        }

        /// <summary>
        /// @windows: -Finance/masters                 
        /// </summary>
        ///  <returns>currencycode Details</returns>
        [HttpGet(ApiRoutes.Currency.FillAllCurrencycode)]
        public IActionResult FillAllCurrencyCode()
        {
            try
            {

                var data = _currencyService.FillAllCurrencyCode();

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
        /// <param name="Id">[Id]</param>
        /// <returns>Currency Code Details</returns>
        [HttpGet(ApiRoutes.Currency.FillCurrencycodeById)]
        public IActionResult FillCurrencycodeById(int Id)
        {
            try
            {
                var result = _currencyService.FillCurrencyCodeById(Id);
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
        /// <param name="CurrencyCodeDto"> [code : , Name]</param> 
        [HttpPost(ApiRoutes.Currency.SaveCurrencycode)]
        public IActionResult SaveCurrencyCode(CurrencyCodeDto currencyCodeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var data = _currencyService.SaveCurrencyCode(currencyCodeDto);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -Inventory/masters               
        /// </summary>
        /// <param name="CurrencyCodeDto"> [Id:, Code : , Name:]</param>
        [HttpPatch(ApiRoutes.Currency.UpdateCurrencycode)]
        public IActionResult UpdateCurrencyCode([FromBody] CurrencyCodeDto currencyCodeDto, int Id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _currencyService.UpdateCurrencyCode(currencyCodeDto, Id);

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
        [HttpDelete(ApiRoutes.Currency.DeleteCurrencycode)]
        public IActionResult DeleteCurrencyCode(int Id)
        {
            try
            {
                var result = _currencyService.DeleteCurrencyCode(Id);
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
        ///  <returns>currency Details</returns>
        [HttpGet(ApiRoutes.Currency.FillAllCurrency)]
        public IActionResult FillAllCurrency()
        {
            try
            {

                var data = _currencyService.FillAllCurrency();

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
        /// <param name="Id">[Id]</param>
        /// <returns>Currency Details</returns>
        [HttpGet(ApiRoutes.Currency.FillCurrencyById)]
        public IActionResult FillCurrencyById(int Id)
        {
            try
            {
                var result = _currencyService.FillCurrencyById(Id);
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
        /// <param name="CurrencyDto"> [CurrencyName : , CurrencyCode,CurrencyRate,IsDefault,Coin]</param> 
        [HttpPost(ApiRoutes.Currency.SaveCurrency)]
        public IActionResult SaveCurrency(CurrencyDto currencyDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var data = _currencyService.SaveCurrency(currencyDto);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// @windows: -Inventory/masters               
        /// </summary>
        /// <param name="CurrencyDto"> [CurrencyName : , CurrencyCode,CurrencyRate,IsDefault,Coin]</param> 
        [HttpPatch(ApiRoutes.Currency.UpdateCurrency)]
        public IActionResult UpdateCurrency([FromBody] CurrencyDto currencyDto, int Id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _currencyService.UpdateCurrency(currencyDto, Id);

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
        [HttpDelete(ApiRoutes.Currency.DeleteCurrency)]
        public IActionResult DeleteCurrency(int Id)
        {
            try
            {
                var result = _currencyService.DeleteCurrency(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.Currency.curDropdown)]
        public IActionResult CurrencyDropdown()
        {
            try
            {
                var result = _currencyService.CurrencyDropdown();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
