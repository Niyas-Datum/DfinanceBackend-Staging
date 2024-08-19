using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Sales;
using Dfinance.Sales.Service.Interface;
using Dfinance.Shared.Domain;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Swashbuckle.AspNetCore.Annotations;
using static Dfinance.Shared.Routes.v1.ApiRoutes;

namespace Dfinance.api.Controllers.v1.DMain.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : BaseController
    {
        private readonly ISalesInvoiceService _salesService;
        private readonly ISalesOrder _salesOrder;
        public SalesController(ISalesInvoiceService sales,ISalesOrder salesOrder)
        {
            _salesService = sales;
            _salesOrder=salesOrder;
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
        public IActionResult Delete(int transId, int pageId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _salesService.DeleteSales(transId,pageId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.Sales.CanlSales)]
        public IActionResult Cancel(int transId, int pageId,string reason)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _salesService.CancelSales(transId,pageId,reason);
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
        [HttpGet(InvRoute.Sales.getsalessummary)]

        public IActionResult GetMonthlySalesSummary(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var data = _salesService.GetMonthlySalesSummary(startDate, endDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.Sales.DaySummary)]
        [AllowAnonymous]
        public IActionResult GetFillSalesDaySummary(string? criteria, DateTime startDate, DateTime endDate, int? branch, int? user)
        {
            try
            {
                var data = _salesService.GetFillSalesDaySummary(criteria, startDate, endDate, branch, user);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving sales day summary: {ex.Message}");
            }
        }
        [HttpGet(InvRoute.Sales.SalesPurchaseSummary)]
        [AllowAnonymous]
        public IActionResult GetSalesPurchaseSummary(DateTime startDate, DateTime endDate, int? branch, int? user)
        {
            try
            {
                var data = _salesService.GetSalesPurchaseSummary(startDate, endDate, branch, user);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving sales day summary: {ex.Message}");
            }
        }
        [HttpGet(InvRoute.Sales.AreaWiseSales)]
        [AllowAnonymous]
        public IActionResult AreaWiseSales(string? viewby, DateTime startdate, DateTime enddate, int? item, int? Area)
        {
            try
            {
                var data = _salesService.AreaWiseSales(viewby,startdate, enddate, item,Area);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.Sales.SalesReport)]
        [AllowAnonymous]
        public IActionResult SalesReport(string? Criteria, DateTime DateFrom, DateTime DateUpto, int? VoucherID, bool? Detailed, int? AccountID, string? VoucherNo, int? SalesManID)
        {
            try
            {
                var data = _salesService.SalesReport(Criteria, DateFrom, DateUpto, VoucherID, Detailed, AccountID, VoucherNo, SalesManID);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.Sales.SalesCommission)]
        public IActionResult SalesCommission(DateTime startdate, DateTime enddate, int? salesmanId, int? userId)
        {
            try
            {
                var data = _salesService.SalesCommission(startdate, enddate, salesmanId, userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(InvRoute.Sales.TopCustomerSupplier)]
        public IActionResult TopCustomerSupplier(DateTime startdate, DateTime enddate, int? pageId)
        {
            try
            {
                var data = _salesService.TopCustomerSupplier(startdate, enddate, pageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost(InvRoute.Sales.SaveSalesOrder)]

        public IActionResult SaveSalesOrder([FromBody] InventoryTransactionDto salesDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _salesOrder.SaveSalesOrder(salesDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.Sales.UpdateSalesOrder)]
        public IActionResult UpdatSalesOrder([FromBody] InventoryTransactionDto purchaseDto, int PageId, int voucherId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                object result = _salesOrder.UpdateSalesOrder(purchaseDto, PageId, voucherId);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.Sales.userwiseProfit)]
        [SwaggerOperation(Summary = "PageID=529")]
        public IActionResult UserwiseProfit(DateTime startDate, DateTime endDate, int pageId, int? User, bool? detailed)
        {
            try
            {                                                 
                var result = _salesService.UserwiseProfit(startDate, endDate, pageId, User, detailed);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}

