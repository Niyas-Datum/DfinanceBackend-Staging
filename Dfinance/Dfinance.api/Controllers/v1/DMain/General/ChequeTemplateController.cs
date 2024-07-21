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
    public class ChequeTemplateController : BaseController
    {
        private readonly IChequeTemplateService _chequeTemplateService;
        public ChequeTemplateController(IChequeTemplateService chequeTemplateService)
        {
            _chequeTemplateService = chequeTemplateService;
        }

        [HttpGet(ApiRoutes.ChequeTemplate.FillChq)]
       
        public IActionResult FillMaster()
        {
            try
            {
                var view = _chequeTemplateService.FillMaster();
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.ChequeTemplate.FillChqtemplate)]

        public IActionResult FillChqTemplate(int Id)
        {
            try
            {
                var view = _chequeTemplateService.FillChqTemplate(Id);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.ChequeTemplate.FillChqtemplateField)]
        public IActionResult FillChqTemplateField(int CheqTempId)
        {
            try
            {
                var view = _chequeTemplateService.FillChqTemplateField( CheqTempId);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(ApiRoutes.ChequeTemplate.Save)]
        public IActionResult SaveChequeTemplate(CheqTempDto cheqTempDto)
        {
            try
            {
                var view = _chequeTemplateService.SaveChequeTemplate(cheqTempDto);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(ApiRoutes.ChequeTemplate.Update)]
        public IActionResult UpdateCheqTemp(CheqTempDto cheqTempDto)
        {
            try
            {
                var view = _chequeTemplateService.UpdateCheqTemp(cheqTempDto);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(ApiRoutes.ChequeTemplate.Delete)]
        public IActionResult DeleteCheqTemp(int CheqTempId)
        {
            try
            {
                var view = _chequeTemplateService.DeleteCheqTemp(CheqTempId);
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(ApiRoutes.ChequeTemplate.BankPopup)]
        public IActionResult BankPopup()
        {
            try
            {
                var view = _chequeTemplateService.BankPopup();
                return Ok(view);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
