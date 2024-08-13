using Dfinance.api.Framework;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;
using Dfinance.api.Authorization;
using Dfinance.DataModels.Dto.Inventory;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [Authorize]
    [ApiController]
    public class BatchEditController : BaseController
    {
        private readonly IBatchEditService _batchEdit;
        public BatchEditController(IBatchEditService batchEdit)
        {
            _batchEdit = batchEdit;
        }
        [HttpPatch(InvRoute.BatchEdit.Update)]
        public IActionResult UpdateDeliveryIn(int pageId, string? vNo, string? batchNo = null, string? newBatchNo = null, DateTime? expiryDate = null)
        {
            try
            {
                var data = _batchEdit.UpdateBatchNo(pageId,vNo,batchNo,newBatchNo,expiryDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.BatchEdit.LoadDate)]
        public IActionResult GetLoadData()
        {
            try
            {
                var data = _batchEdit.GetLoadData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.BatchEdit.FillBatchDetails)]
        public IActionResult FillBatchDetails(BatchEditDto batchEdit)
        {
            try
            {
                var data = _batchEdit.BatchDetailsForUpdate(batchEdit);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
