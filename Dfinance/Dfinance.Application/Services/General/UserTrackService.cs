using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dfinance.Application.Services.General
{
    public class UserTrackService : IUserTrackService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private ILogger<UserTrackService> _logger;
        public UserTrackService(DFCoreContext context, IAuthService authService, ILogger<UserTrackService> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }
        public CommonResponse AddUserActivity(string Reference, int RowID, int ActionID, string Reason, string TableName, string ModuleName, decimal Amount,string details)
        {
            try
            {
                var ActionDate = DateTime.Now;
                var UserID = _authService.GetId().Value;
                string criteria = "UpdateUserTrack";
                string MachineName = Environment.MachineName;
                var result = _context.Database.ExecuteSqlRaw(
                "EXEC UserTrackSP @Criteria = {0}, @UserID = {1}, @TableName = {2}, @ActionDate = {3}, @Reason = {4}, @ActionID = {5} , @RowID = {6}, @MachineName = {7}, @ModuleName = {8}, @Reference ={9}, @Amount ={10},@Details={11}",
                criteria, UserID, TableName, ActionDate, Reason, ActionID, RowID, MachineName, ModuleName, Reference, Amount,details);
                _logger.LogInformation("added usertrack details");
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }
        
        public CommonResponse FillUserTrack(UserTrackDto userTrackDto)
        {
            try
            {
                string criteria = "FillUserTrack";
                var query = _context.UserTrackView.FromSqlRaw("EXEC UserTrackSP @Criteria={0}, @DateFrom={1}, @DateUpto={2}, @Username={3}, @ModuleName={4}, @MachineName={5}, @TableName={6}, @RowID={7}, @Reference={8}, @Amount={9}, @Reason={10}, @ActionID={11}",
                            criteria, userTrackDto.DateFrom, userTrackDto.DateUpto, userTrackDto.UserName, userTrackDto.ModuleName, userTrackDto.MachineName, userTrackDto.TableName, userTrackDto.RowID , userTrackDto.Reference, userTrackDto.Amount, userTrackDto.Reason, userTrackDto.Action?.Id).ToList();
                _logger.LogInformation("Successfully filled");
                return CommonResponse.Ok(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }




    }
}




