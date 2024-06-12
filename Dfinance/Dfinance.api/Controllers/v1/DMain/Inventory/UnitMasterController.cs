using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Inventory.Interface;
using Dfinance.Shared.Routes;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers
{
    [Authorize]
    [ApiController]
    public class UnitMasterController : BaseController
    {

        private readonly IUnitMasterService _UnitMasterService;
       
        public UnitMasterController(IUnitMasterService UnitMasterService)
        {
            _UnitMasterService = UnitMasterService;
            
        }
        /// <summary>
        /// @windows: -  General/masters
        /// @Form : UnitMaster
        /// </summary>
        ///  <returns>FillMaster</returns>
        [HttpGet(InvRoute.UnitMaster.FillMaster)]
        public IActionResult FillMaster()
        {
            try
            {
                var view = _UnitMasterService.FillMaster();
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// @windows: -  General/masters
        /// @Form : UnitMaster
        /// </summary>
        ///  <returns>UnitMaster Details by Unit</returns>
        [HttpGet(InvRoute.UnitMaster.FillByUnit)]
        public IActionResult FillByUnit(string unit)
        {
            try
            {
                var view = _UnitMasterService.FillByUnit(unit);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -  General/masters
        /// @Form : UnitMaster
        /// </summary>
        ///  <returns>Dropdown details</returns>
        [HttpGet(InvRoute.UnitMaster.UnitDropDown)]
        public IActionResult UnitDropDown()
        {
            try
            {
                var user = _UnitMasterService.UnitDropDown();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -General/masters  
        /// @Form : UnitMaster
        /// </summary>
        ///  <param name="UnitMasterDto"> </param>
        [HttpPost(InvRoute.UnitMaster.SaveUnitMaster)]
        public IActionResult SaveUnitMaster(UnitMasterDto unitmasterDto)
        {
            try
            {
                var save = _UnitMasterService.SaveUnitMaster(unitmasterDto);
                return Ok(save);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -General/masters  
        /// @Form : UnitMaster
        /// </summary>
        ///  <param name="UnitMasterDto"> </param>
        [HttpPatch(InvRoute.UnitMaster.UpdateUnitMaster)]
        public IActionResult UpdateUnitMaster(UnitMasterDto unitmasterDto)
        {
            try
            {
              
                    var update = _UnitMasterService.UpdateUnitMaster(unitmasterDto);
                    return Ok(update);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -General/masters  
        /// @Form : UnitMaster
        /// </summary>
        /// <param> [unit] </param> 
        [HttpDelete(InvRoute.UnitMaster.DeleteUnitMaster)]
        public IActionResult DeleteUnitMaster(string unit)
        {
            try
            {
               
                    var delete = _UnitMasterService.DeleteUnitMaster(unit);
                    return Ok(delete);
              
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete(InvRoute.UnitMaster.UnitPopup)]
        public IActionResult UnitPopup()
        {
            try
            {
                var unit = _UnitMasterService.UnitPopup();
                return Ok(unit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
