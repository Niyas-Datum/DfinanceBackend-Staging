using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Shared.Routes;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Inventory
{
    [ApiController]
    [Authorize]
    public class DosageMasterController : BaseController
    {
        private readonly IDosageMasterService _dosageservice;
        public DosageMasterController(IDosageMasterService dosageservice)
        {
            _dosageservice = dosageservice;
        }
        [HttpGet(InvRoute.DosageMaster.FillMasterAndId)]
        public IActionResult FillMasterAndById(int? Id)
        {
            try
            {
                var result = _dosageservice.FillMasterAndById(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(InvRoute.DosageMaster.SaveUpdateDosage)]
        public IActionResult SaveUpdateDosage(DosageDto dosageDto, int PageId)
        {
            try
            {
                var result = _dosageservice.SaveUpdateDosage(dosageDto,PageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(InvRoute.DosageMaster.Delete)]
        public IActionResult DeleteDosage(int Id, int PageId)
        {
            try
            {
                var result = _dosageservice.DeleteDosage(Id, PageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
