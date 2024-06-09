using Dfinance.api.Authorization;
using Dfinance.api.Framework;
using Dfinance.Application.Services.Finance.Interface;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Routes.v1;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Controllers.v1.DMain.Finance
{
    [ApiController]
    [Authorize]
    public class CardMasterController : BaseController
    {
        private readonly ICardMaster _CardMasterService;

        public CardMasterController(ICardMaster cardMasterService)
        {
            _CardMasterService = cardMasterService;
        }
        [HttpPost(FinRoute.CardMaster.SaveCardMaster)]       
        public IActionResult SaveCardMaster(CardMasterDto cardMaster)
        {
            try
            {
                var result = _CardMasterService.SaveCardMaster(cardMaster);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch(FinRoute.CardMaster.UpdateCardMaster)]       
        public IActionResult UpdateCardMaster(CardMasterDto cardMaster, int Id)
        {
            try
            {
                var result = _CardMasterService.UpdateCardMaster(cardMaster,Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(FinRoute.CardMaster.DeleteCardMaster)]        
        public IActionResult DeleteCardMaster( int Id)
        {
            try
            {
                var result = _CardMasterService.DeleteCardMaster( Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(FinRoute.CardMaster.FillCardMaster)]        
        public IActionResult FillCardMaster(int Id)
        {
            try
            {
                var result = _CardMasterService.FillCardMaster(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet(FinRoute.CardMaster.FillMaster)]      
        public IActionResult FillMaster()
        {
            try
            {
                var result = _CardMasterService.FillMaster();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
