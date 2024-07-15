using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
   
    [ApiController]
    public class InventoryPaymentServiceController : BaseController
    {
        private readonly IInventoryPaymentService _payservice;
       
        public InventoryPaymentServiceController(IInventoryPaymentService payservice)
        {
            _payservice = payservice;
        }
        [HttpGet(InvRoute.InventoryPaymentTransaction.FillTax)]
        public IActionResult FillTax()
        {
            try
            {
                var result = _payservice.FillTax();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InventoryPaymentTransaction.FillAddCharge)]
        public IActionResult FillAddCharge()
        {
            try
            {
                var result = _payservice.FillAddCharge();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(InvRoute.InventoryPaymentTransaction.FillCash)]
        public IActionResult FillCash()
        {
            try
            {
                var result = _payservice.FillPaymentDetails("CASH");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InventoryPaymentTransaction.FillCard)]
        public IActionResult FillCard()
        {
            try
            {
                var result = _payservice.FillPaymentDetails("CARD");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(InvRoute.InventoryPaymentTransaction.FillEpay)]
        public IActionResult FillEpay()
        {
            try
            {
                var result = _payservice.FillPaymentDetails("EPAY");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet(InvRoute.InventoryPaymentTransaction.FillAdvance)]
        public IActionResult FillAdvance(int AccountId, int voucherId, DateTime? date)
        {
            try
            {
                var result = _payservice.FillAdvance(AccountId,voucherId, date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(InvRoute.InventoryPaymentTransaction.FillCheque)]
        public IActionResult FillCheque()
        {
            try
            {
                var result = _payservice.FillCheque();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InventoryPaymentTransaction.FillBankName)]
        public IActionResult FillBankName()
        {
            try
            {
                var result = _payservice.FillBankName();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.InventoryPaymentTransaction.SaveCheque)]

        public IActionResult SaveCheque([FromBody] InvChequesDto chequeDto, int VEId, int PartyId)
        {
            try
            {
                var view = _payservice.SaveCheque(chequeDto, VEId, PartyId);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.InventoryPaymentTransaction.defaultAcc)]
        [SwaggerOperation(Summary = "Fills the Default Account for the time of Payment(Give input as Cash/ Card/ Online)")]
        public IActionResult SetDefaultAccount(string TranType)
        {
            try
            {
                var view = _payservice.SetDefaultAccount(TranType);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
