using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Reports.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Reports
{
    [ApiController]

    public class PurchaseReportController : BaseController
    {
        private readonly IPurchaseReportService _purchaseReportService;

        public PurchaseReportController(IPurchaseReportService purchaseReportService)
        {
            _purchaseReportService = purchaseReportService;
        }

        [HttpPost(InvRoute.PurchaseReport.getPurchaseReport)]
        [AllowAnonymous]
        public IActionResult GetPurchaseReport(PurchaseReportDto purchaseReportDto)
        {
            var result = _purchaseReportService.FillPurchaseReport(purchaseReportDto);
            return Ok(result);
        }
    }
}
