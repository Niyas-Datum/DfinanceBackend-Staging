using Dfinance.api.Framework;
using Dfinance.Application;
using Dfinance.DataModels.Dto;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionAdditionalController : BaseController
    {
        private readonly ITransactionAdditionalsService _service;
        public TransactionAdditionalController(ITransactionAdditionalsService service)
        {
            _service = service;
        }
        [HttpGet(InvRoute.TransactionAdditionals.GetByTransactionId)]
        public IActionResult GetTransactionAdditionals(int transactionId)
        {
            try
            {
                var res = _service.FillTransactionAdditionals(transactionId);
                return Ok(res);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost(InvRoute.TransactionAdditionals.Save)]
        public IActionResult SaveTransactionAdditionals([FromBody]FiTransactionAdditionalDto fiTransactionAdditionalDto)
        {
            try
            {
                var result = _service.SaveTransactionAdditional(fiTransactionAdditionalDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.TransactionAdditionals.Update)]
        public IActionResult UpdateTransactionAdditionals(FiTransactionAdditionalDto fiTransactionAdditionalDto)
        {
            try
            {
                var result = _service.UpdateTransactionAdditional(fiTransactionAdditionalDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.TransactionAdditionals.Delete)]
        public IActionResult DeleteTransactionAdditionals(int transactionId)
        {
            try
            {
                var result = _service.DeleteTransactionAdditional(transactionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
