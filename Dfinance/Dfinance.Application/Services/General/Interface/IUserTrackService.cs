using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.General.Interface
{
    public interface IUserTrackService
    {
        CommonResponse AddUserActivity(string Reference, int RowID, int ActionID, string Reason, string TableName, string ModuleName, decimal Amount,string details);
        CommonResponse FillUserTrack(UserTrackDto userTrackDto);
    }

}