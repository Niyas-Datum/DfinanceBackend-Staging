using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Inventory.Reports;
using Dfinance.Inventory.Reports.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Inventory
{
    [ApiController]
    [Authorize]
    public class InventoryApprovalController : BaseController
    {
        private readonly IInventoryApproval _inventoryapprovalService;

        public InventoryApprovalController(IInventoryApproval InventoryApprovalService)
        {
            _inventoryapprovalService = InventoryApprovalService;

        }

        [HttpGet(InvRoute.InventoryApproval.FillInventoryApproval)]
        public ActionResult FillInventoryApproval(DateTime FromDate, DateTime ToDate, int BranchID,int ApprovalStatus,int pageId, int? ModeID = null, string MachineName = "", int? VTypeID = null, int? Detailed = null, int? UserID = null, int? VoucherID = null, bool? AutoEntry = null, int? TransactionID = null)
        
            {
            try
            {

                var result = _inventoryapprovalService.FillInventoryApproval(FromDate, ToDate, BranchID,ApprovalStatus,pageId, ModeID, MachineName = "", VTypeID, Detailed , UserID , VoucherID , AutoEntry , TransactionID ); 
                return Ok(result);
            }
            catch (Exception ex)
            {


                return BadRequest(ex.Message);
            }
        }
    }
}




