using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Inventory.Service.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                var result = _payservice.FillCash();
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
                var result = _payservice.FillCard();
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
                var result = _payservice.FillEpay();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet(InvRoute.InventoryPaymentTransaction.FillAdvance)]
        public IActionResult FillAdvance(int AccountId, string Drcr, DateTime? date)
        {
            try
            {
                var result = _payservice.FillAdvance(AccountId, Drcr, date);
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

        public IActionResult SaveCheque([FromBody] ChequesDto chequeDto, int VEId, int PartyId, string Status)
        {
            try
            {
                var view = _payservice.SaveCheque(chequeDto, VEId, PartyId, Status);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
