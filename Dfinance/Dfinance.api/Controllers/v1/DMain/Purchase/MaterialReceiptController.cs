using Dfinance.api.Authorization;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [Authorize]
    [ApiController]
    public class MaterialReceiptController : ControllerBase
    {
        private readonly IMaterialReceiptService _matReceipt;
        public MaterialReceiptController(IMaterialReceiptService matReceipt)
        {
            _matReceipt = matReceipt;
        }

        [HttpGet(InvRoute.MaterialReceipt.GetData)]
        public IActionResult GetData(int pageId, int voucherId)
        {
            try
            {
                var data = _matReceipt.GetData(pageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialReceipt.GetFromWH)]
        public IActionResult FillFromWarehouse(int branchId)
        {
            try
            {
                var data = _matReceipt.FillFromWarehouse(branchId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialReceipt.fillitems)]
        public IActionResult FillItems(int partyId, int PageID, int locId, int voucherId)
        {
            try
            {
                var data = _matReceipt.FillTransItems(partyId, PageID, locId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialReceipt.FillAccount)]
        public IActionResult FillBranchAccount()
        {
            try
            {
                var data = _matReceipt.FillBranchAccount();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialReceipt.FillMaster)]
        public IActionResult FillMaster(int? PageId = 0, int? transId = 0, int? voucherId = 0)
        {
            try
            {
                var data = _matReceipt.FillMaster(PageId, transId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialReceipt.FillById)]
        public IActionResult FillById(int transId, int pageId)
        {
            try
            {
                var data = _matReceipt.FillById(transId, pageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.MaterialReceipt.Save)]
        public IActionResult SaveMatReceipt(MaterialTransferDto materialDto, int voucherId, int pageId)
        {
            try
            {
                var data = _matReceipt.SaveMatReceipt(materialDto, voucherId, pageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.MaterialReceipt.Update)]
        public IActionResult UpdateMatReceipt(MaterialTransferDto materialDto, int voucherId, int pageId)
        {
            try
            {
                var data = _matReceipt.UpdateMatReceipt(materialDto, voucherId, pageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.MaterialReceipt.Delete)]
        public IActionResult DeleteMatReceipt(int TransId, int PageId)
        {
            try
            {
                var data = _matReceipt.DeleteMatReceipt(TransId, PageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.MaterialReceipt.CancelMatr)]
        public IActionResult CancelMatReceipt(int TransId, int PageId, string reason)
        {
            try
            {
                var data = _matReceipt.CancelMatReceipt(TransId, PageId,reason);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet(InvRoute.MaterialReceipt.getSettings)]
        //public IActionResult GetMaterialSettings()
        //{
        //    try
        //    {
        //        var data = _matReceipt.GetMaterialSettings();
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet(InvRoute.MaterialReceipt.sizeMasterPopup)]
        public IActionResult SizeMasterPopup()
        {
            try
            {
                var data = _matReceipt.SizeMasterPopup();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialReceipt.findQty)]
        public IActionResult FindQuantity(int itemId, int locId, int qty, int? transId = 0)
        {
            try
            {
                var data = _matReceipt.FindQuantity(itemId, locId, qty, transId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialReceipt.latVoucherdate)]
        public IActionResult GetLatestVoucherDate()
        {
            try
            {
                var data = _matReceipt.GetLatestVoucherDate();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialReceipt.marPrice)]
        public IActionResult GetMarginPrice(int? itemId,int? accountId,int? voucherId, string? unit)
        {
            try
            {
                var data = _matReceipt.GetMarginPrice(itemId,accountId,voucherId,unit);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
