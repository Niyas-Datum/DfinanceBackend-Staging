using Dfinance.Application.Services.Finance.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Finance;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Application.Services
{
    public class CardMasterService : ICardMaster
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private ILogger<CardMasterService> _logger;
        public CardMasterService(DFCoreContext context, IAuthService authService, ILogger<CardMasterService> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }
        public CommonResponse SaveCardMaster(CardMasterDto cardMaster)
        {
            try
            {
                string criteria = "InsertCardMaster";

                SqlParameter newIdCam = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                var checkDefault = _context.FiMaCards.Any(x => x.Default == true);
                if (cardMaster.Default == true && checkDefault)
                    return CommonResponse.Error("Only one card is set as Default");
                _context.Database.ExecuteSqlRaw("EXEC SpCardMaster @Criteria={0}, @AccountID={1}, @Description={2}, @Commission={3}, @Default={4}, @NewID={5} OUTPUT",
                    criteria,
                    cardMaster.AccountName.ID,
                    cardMaster.Description,
                    cardMaster.Commission,
                    cardMaster.Default,
                    newIdCam
                );
                var newId = (int)newIdCam.Value;
                _logger.LogInformation("Card Master saved Successfully ");
                return CommonResponse.Created("Saved Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("Fail to save Card Master");
                return CommonResponse.Error(ex.Message);

            }
        }
        public CommonResponse UpdateCardMaster(CardMasterDto cardMaster, int Id)
        {
            try
            {
                var card = _context.FiMaCards.Any(x => x.Id == Id);
                if (!card)
                    return CommonResponse.NotFound("Not Found");
                var checkDefault = _context.FiMaCards.Any(x => x.Default == true);
                if (cardMaster.Default == true && checkDefault)
                    return CommonResponse.Error("Only one card is set as Default");
                string criteria = "UpdateCardMaster";
                _context.Database.ExecuteSqlRaw("EXEC SpCardMaster @Criteria={0},@AccountID={1},@Description={2},@Commission={3},@Default={4},@ID= {5}",
                criteria,
                cardMaster.AccountName.ID,
                cardMaster.Description,
                cardMaster.Commission,
                cardMaster.Default,
                Id
                );
                _logger.LogInformation("Card Master updated Successfully ");
                return CommonResponse.Ok("Updated Successfully");

            }
            catch (Exception ex)
            {
                _logger.LogError("Fail to update Card Master");
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse DeleteCardMaster(int Id)
        {
            try
            {

                string msg = null;
                var card = _context.FiMaCards.Any(x => x.Id == Id);
                if (!card)
                    return CommonResponse.NotFound("Not Found");

                string criteria = "DeleteCardMaster";
                    var result = _context.Database.ExecuteSqlRaw($"Exec SpCardMaster @Criteria='{criteria}',@ID='{Id}'");
                _logger.LogInformation("Card Master deleted Successfully ");
                msg = Id + " Deleted Successfully";
                    return CommonResponse.Ok(msg);

                
            }
            catch (Exception ex)
            {
                _logger.LogError("Fail to delete Card Master");
                return CommonResponse.Error(ex.Message);
            }
    }
        public CommonResponse FillCardMaster(int Id)
        {
            try
            {
                var card=_context.FiMaCards.Any(x => x.Id == Id);
                if (!card)
                    return CommonResponse.NotFound("Not Found");
                string criteria = "FillCardMaster";
                var result = _context.FillCardMaster.FromSqlRaw($"Exec SpCardMaster @Criteria='{criteria}',@ID='{Id}'").ToList();
                return CommonResponse.Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError("Fail to fill Card Master");
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse FillMaster()
        {
            try
            {
                string criteria = "FillMaster";
                var result = _context.FillMaster.FromSqlRaw($"Exec SpCardMaster @Criteria='{criteria}'").ToList();
                return CommonResponse.Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError("Fail to fill Card Master");
                return CommonResponse.Error(ex.Message);
            }
        }
    }
}




