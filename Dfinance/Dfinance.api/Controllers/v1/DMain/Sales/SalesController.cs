﻿using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Sales;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : BaseController
    {
        private readonly ISalesInvoiceService _salesService;
        public SalesController(ISalesInvoiceService sales)
        {
            _salesService = sales;
        }
        [HttpPost(InvRoute.Sales.SaveSales)]

        public IActionResult Save([FromBody] InventoryTransactionDto salesDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _salesService.SaveSales(salesDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.Sales.UpdateSales)]
        public IActionResult Update([FromBody] InventoryTransactionDto purchaseDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _salesService.UpdateSales(purchaseDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.Sales.DelSales)]
        public IActionResult Delete(int transId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _salesService.DeleteSales(transId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.Sales.FillSales)]
        public IActionResult FillSales(int pageid, bool post)
        {
            try
            {
                var data = _salesService.FillSales(pageid, post);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.Sales.FillSalesbyid)]

        public IActionResult FillSalesById(int TransId)
        {
            try
            {
                var data = _salesService.FillSalesById(TransId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.Sales.GetData)]
        public IActionResult GetData(int pageId, int voucherId)
        {
            try
            {
                var data = _salesService.GetData(pageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet(InvRoute.Sales.fillitems)]
        public IActionResult FillTransItems(int partyId, int PageID, int locId, int voucherId)
        {
            try
            {
                var data = _salesService.FillTransItems(partyId, PageID, locId, voucherId).Data;
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
