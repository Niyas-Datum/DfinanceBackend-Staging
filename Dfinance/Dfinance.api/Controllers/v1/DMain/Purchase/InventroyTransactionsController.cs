using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Common;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [HttpGet(InvRoute.InventroyTransactions.partyBal)]
        [SwaggerOperation(Summary = "shows party balance")]
        public IActionResult FillPartyBal(int partyId)
        {
            try
            {
                var data = _transactionService.FillPartyBal(partyId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InventroyTransactions.payType)]
        [SwaggerOperation(Summary = "Payment type:Cash/Card")]
        public IActionResult FillPayType()
        {
            try
            {
                var data = _transactionService.FillPayType();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InventroyTransactions.getvoucherno)]
        public IActionResult GetAutoVoucherNo(int voucherid)
        {
            try
            {
                var data = _transactionService.GetAutoVoucherNo(voucherid);
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
        [SwaggerOperation(Summary = "Fill the references")]
        public IActionResult GetReference([FromQuery] int voucherno, DateTime? date = null)
        {
            try
            {
                var data = _transactionService.GetReference(voucherno, date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.InventroyTransactions.DeleteTrans)]

        public IActionResult DeleteTransactions(int Transid)
        {
            try
            {
                var data = _transactionService.DeleteTransactions(Transid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.InventroyTransactions.Cancel)]

        public IActionResult CancelTransaction(int Transid, string reason)
        {
            try
            {
                var data = _transactionService.CancelTransaction(Transid,reason);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InventroyTransactions.refItemList)]
        [SwaggerOperation(Summary = "Shows the Item list of selected reference")]
        public IActionResult FillImportItemList(int? transId, int? voucherId)
        {
            try
            {
                var data = _transactionService.FillImportItemList(transId,voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InventroyTransactions.FillVoucherType)]
        [SwaggerOperation(Summary = "Dropdown VoucherType in Reference")]
        public IActionResult FillVoucherType(int voucherId)
        {
            try
            {
                var data = _transactionService.FillVoucherType(voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InventroyTransactions.FillRefItems)]
        
        public IActionResult FillRefItems(List<ReferenceDto> referenceDto)
        {
            try
            {
                var data = _transactionService.FillReference(referenceDto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// InventroyTransactions reports
        /// </summary>
        /// <param name="inventoryTransactionDto"></param>
        /// <param name="moduleid"></param>
        /// <returns></returns>
        [HttpPost(InvRoute.InventroyTransactions.GetInventoryTransactions)]
        [AllowAnonymous]
        public IActionResult InventoryTransactions(InventoryTransactionsDto inventoryTransactionDto, int? moduleid)
        {
            var result = _transactionService.InventoryTransactions(inventoryTransactionDto, moduleid);
            return Ok(result);
        }

        [HttpGet(InvRoute.InventroyTransactions.FillTranById)]
        public IActionResult FillTransactionbyId(int Id)
        {
            try
            {
                var data = _transactionService.FillTransactionbyId(Id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

        [HttpPost(InvRoute.InventroyTransactions.impItems)]
        [SwaggerOperation(Summary = "Fill the Imported Items")]
        public IActionResult FillImportItems(int transId, int? voucherId = null)
        {
            var result = _transactionService.FillImportItems(transId, voucherId);
            return Ok(result);
        }

    }
}

