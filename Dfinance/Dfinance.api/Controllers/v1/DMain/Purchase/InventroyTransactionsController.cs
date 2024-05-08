using Dfinance.api.Framework;
using Dfinance.Inventory.Service;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{

    [ApiController]
    public class InventroyTransactionsController : BaseController
    {
       
        private readonly IInventoryTransactionService _transactionService;
        public InventroyTransactionsController(IInventoryTransactionService transactionService)
        {
         
            _transactionService = transactionService;
        }
        [HttpGet(InvRoute.InventroyTransactions.getvoucherno)]
        public IActionResult GetAutoVoucherNo(int voucherid)
        {
            try
            {
                var data = _transactionService.GetAutoVoucherNo( voucherid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InventroyTransactions.getsalesman)]
        public IActionResult GetSalesMan()
        {
            try
            {
                var data = _transactionService.GetSalesman();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InventroyTransactions.getreference)]
        public IActionResult GetReference([FromQuery]int voucherno, DateTime? date = null)
        {
            try
            {
                var data = _transactionService.GetReference(voucherno,date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.InventroyTransactions.DeletePurchase)]

        public IActionResult DeletePurchase(int Transid)
        {
            try
            {
                var data = _transactionService.DeletePurchase(Transid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
