using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.DataModels.Dto.Inventory.Purchase;
using Dfinance.Purchase.Services.Interface;
using Dfinance.Shared.Routes;
using Dfinance.WareHouse.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Purchase
{
    [Authorize]
    [ApiController]
    public class MaterialRequestController : BaseController
    {
        private readonly IMaterialRequestService _materialReq;
        public MaterialRequestController(IMaterialRequestService materialReq)
        {
            _materialReq = materialReq;
        }

        [HttpGet(InvRoute.MaterialRequest.GetData)]
        public IActionResult GetData(int pageId, int voucherId)
        {
            try
            {
                var data = _materialReq.GetData(pageId, voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialRequest.GetFromWH)]
        public IActionResult FillFromWarehouse(int branchId)
        {
            try
            {
                var data = _materialReq.FillFromWarehouse(branchId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialRequest.fillitems)]
        public IActionResult FillItems(int partyId, int PageID, int locId, int voucherId)
        {
            try
            {
                var data = _materialReq.FillTransItems(partyId,PageID,locId,voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialRequest.Fill)]
        public IActionResult FillMaster(int? PageId = 0, int? transId = 0, int? voucherId = 0)
        {
            try
            {
                var data = _materialReq.FillMaster(PageId,transId,voucherId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialRequest.FillById)]
        public IActionResult FillById(int TransId, int PageId)
        {
            try
            {
                var data = _materialReq.FillById(TransId, PageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet(InvRoute.MaterialRequest.getSettings)]
        //public IActionResult GetMaterialSettings()
        //{
        //    try
        //    {
        //        var data = _materialReq.GetMaterialSettings();
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost(InvRoute.MaterialRequest.Save)]
        public IActionResult SaveMaterialReq(MaterialTransferDto materialDto, int voucherId, int pageId)
        {
            try
            {
                var data = _materialReq.SaveMaterialReq(materialDto,voucherId, pageId);
                return Ok(data);
            }
            catch (Exception ex)
            {                
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(InvRoute.MaterialRequest.Update)]
        public IActionResult UpdateMaterialReq(MaterialTransferDto materialDto, int VoucherId, int PageId)
        {
            try
            {
                var data = _materialReq.UpdateMaterialReq(materialDto, VoucherId, PageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpDelete(InvRoute.MaterialRequest.Delete)]
        public IActionResult DeleteMaterialRequest(int TransId, int PageId)
        {
            try
            {
                var data = _materialReq.DeleteMaterialReq(TransId, PageId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialRequest.sizeMasterPopup)]
        public IActionResult SizeMasterPopup()
        {
            try
            {
                var data = _materialReq.SizeMasterPopup();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialRequest.findQty)]
        public IActionResult FindQuantity(int itemId, int locId, int qty, int? transId = 0)
        {
            try
            {
                var data = _materialReq.FindQuantity(itemId,locId,qty,transId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(InvRoute.MaterialRequest.latVoucherdate)]
        public IActionResult GetLatestVoucherDate()
        {
            try
            {
                var data = _materialReq.GetLatestVoucherDate();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
