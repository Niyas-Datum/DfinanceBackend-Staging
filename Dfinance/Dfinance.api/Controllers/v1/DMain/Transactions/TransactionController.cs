using Dfinance.api.Framework;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{

    [ApiController]
    public class TransactionController : BaseController
    {
        private readonly ITransactionFactory _transfactory;
        public TransactionController(ITransactionFactory transfactory)
        {
            _transfactory = transfactory;
        }
        //[HttpGet(InvRoute.Transactions.getsalesman)]
        //public IActionResult GetSalesMan()
        //{
        //    try
        //    {
        //        var data = _transfactory.GetSalesman();
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
