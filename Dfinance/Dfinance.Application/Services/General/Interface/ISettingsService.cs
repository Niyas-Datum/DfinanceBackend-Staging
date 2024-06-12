using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;

namespace Dfinance.Application.Services.General.Interface
{
    public interface ISettingsService
    {
       
        CommonResponse FillMaster();
        CommonResponse FillByID(int Id);
        CommonResponse SaveSettings(SettingsDto settingsDto, string password);
        CommonResponse UpdateSettings(SettingsDto settingsDto, int Id, string password);
        CommonResponse DeleteSettings(int Id, string password);
        CommonResponse GetSettings(string key);
        CommonResponse GetAllSettings(string[] keys);
    }
}
