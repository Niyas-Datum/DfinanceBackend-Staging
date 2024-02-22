using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.General.Interface;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.General
{
    [Authorize]
    [ApiController]
    public class SettingsController : BaseController
    {

        private readonly ISettingsService _MaSettingsService;

        public SettingsController(ISettingsService MaSettingsService)
        {
            _MaSettingsService = MaSettingsService;
        }



        /// <summary>
        /// @windows: -General/masters  
        /// @Form : Settings
        /// </summary>
        ///  <returns>FillMaster</returns>
        [HttpGet(ApiRoutes.MaSettings.FillMaster)]
        public IActionResult FillMaster()
        {
            try
            {
                var view = _MaSettingsService.FillMaster();
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// @windows: -General/masters  
        /// @Form : Settings
        /// </summary>
        ///  <returns>MaSettings Details by ID</returns>
        [HttpGet(ApiRoutes.MaSettings.FillByID)]
        public IActionResult FillByID(int Id)
        {
            try
            {
                var view = _MaSettingsService.FillByID(Id);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// @windows: -General/masters  
        /// @Form : Settings
        /// </summary>
        ///  <param name="SettingsDto"> </param>
        [HttpPost(ApiRoutes.MaSettings.SaveSettings)]
     
        public IActionResult SaveSettings(SettingsDto settingsDto, string password)
        {
            try
            {
                var view = _MaSettingsService.SaveSettings(settingsDto, password);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// @windows: -General/masters  
        /// @Form : Settings
        /// </summary>
        ///  <param name="SettingsDto"> </param>
        [HttpPatch(ApiRoutes.MaSettings.UpdateSettings)]
        public IActionResult UpdateSettings(SettingsDto settingsDto, int Id, string password)
        {
            try
            {
                var view = _MaSettingsService.UpdateSettings(settingsDto,Id, password);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// @windows: -General/masters  
        /// @Form : Settings
        /// </summary>
        /// <param> [Id] </param> 
        [HttpDelete(ApiRoutes.MaSettings.DeleteSettings)]
       
        public IActionResult DeleteSettings(int Id, string password)
        {
            try
            {
                var view = _MaSettingsService.DeleteSettings(Id, password);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// @windows: -General/masters  
        /// @Form : Settings
        /// </summary>
        ///  <returns>Value</returns>
        [HttpGet(ApiRoutes.MaSettings.KeyValue)]
        public IActionResult KeyValue(string key)
        {
            try
            {
                var view = _MaSettingsService.KeyValue(key);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
