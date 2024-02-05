using Dfinance.Application.Dto.General;
using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthApplication.Services.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dfinance.Application.Services.General
{
    public class SettingsService : ISettingsService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IPasswordService _passwordService;


        public SettingsService(DFCoreContext context, IAuthService authService, IPasswordService passwordService)
        {
           _passwordService = passwordService;
            _context = context;
            _authService = authService;
        }



        //called by MaSettingsController/FillMaster
        //**************************** FillMaster ****************************************

        public CommonResponse FillMaster()
        {
            try
            {
                var result = _context.FillSetting.FromSqlRaw($"EXEC SettingsSP @Criteria ='FillMaster'").ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }


        //called by MaSettingsController/FillByID
        //**************************** FillMaSettingsbyID ****************************************

        public CommonResponse FillByID(int Id)
        {
            try
            {
                var data = _context.MaSettings.Where(i => i.Id == Id).Select(i => i.Id).SingleOrDefault();
                if (data == null)
                {
                    return CommonResponse.NotFound();
                }
                var result = _context.FillSettingById.FromSqlRaw($"EXEC SettingsSP @Criteria ='FillByID', @Id = '{Id}'").ToList();
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        //called by MaSettingsController/SaveSettings
        //**************************** SaveSettings ****************************************
    
        public CommonResponse SaveSettings(SettingsDto settingsDto, string password)
        {
            try
            {
                var keys = _context.MaSettings.Any(i => i.Key == settingsDto.Key);

                if (keys)
                {
                    return CommonResponse.Error("Key already exists");
                }

                else
                {
                string criteria = "Insert";

                var passwordCheckResponse = _passwordService.IsPasswordOk(password);

                if ((bool)passwordCheckResponse.Data == true)

                {
                    SqlParameter newIdParam = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    var result = _context.Database.ExecuteSqlRaw(
                        "EXEC SettingsSP @Criteria = {0}, @Key = {1}, @Value = {2}, @Description = {3}, @SystemSetting = {4}, @NewID = @NewID OUTPUT",
                        criteria, settingsDto.Key, settingsDto.Value, settingsDto.Description, settingsDto.SystemSetting, newIdParam);

                    int newId = (int)newIdParam.Value;

                    return CommonResponse.Ok(newId);
                }
                else
                {
                    return CommonResponse.Error("Invalid password.");
                    }
                }
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }

        //called by MaSettingsController/UpdateSettings
        //**************************** UpdateSettings ****************************************
        public CommonResponse UpdateSettings(SettingsDto settingsDto, int Id, string password)
        {
            try
            {
                var criteria = "Update";

                
                string msg = null;
                var setttingId = _context.MaSettings.Where(i => i.Id == Id).Select(i => i.Id).SingleOrDefault();
                if (setttingId == null)
                {
                    msg = "Settings Not Found";
                    return CommonResponse.NotFound(msg);
                }
                else
                {
                    var passwordCheckResponse = _passwordService.IsPasswordOk(password);

                    if ((bool)passwordCheckResponse.Data == true)

                    {
                        var settings = _context.Database.ExecuteSqlRaw($"Exec SettingsSP @Criteria='{criteria}',@ID='{Id}',@Key='{settingsDto.Key}',@Value='{settingsDto.Value}',@Description='{settingsDto.Description}'," +
                        $"@SystemSetting='{settingsDto.SystemSetting}'");
                        return CommonResponse.Ok("Settings Updated Successfully");
                    }
                    else
                    {
                        return CommonResponse.Error("Invalid Password");
                    }
                }

            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        //called by MaSettingsController/DeleteSettings
        //**************************** DeleteSettings ****************************************

        public CommonResponse DeleteSettings(int Id, string password)
        {
            try
            {
                var criteria = "Delete";
                //var isPasswordOk = _passwordService.IsPasswordOk(password);
                string msg = null;
                var deleteId = _context.MaSettings.Where(i => i.Id == Id).Select(i => i.Id).SingleOrDefault();
                if (deleteId == null)
                {
                    msg = "Settings to be deleted  is not found";
                    return CommonResponse.NotFound(msg);
                }
                else
                {
                   var passwordCheckResponse = _passwordService.IsPasswordOk(password);

                    if ((bool)passwordCheckResponse.Data == true)

                    {
                        msg = "Deleted Sucessfully";
                        var result = _context.Database.ExecuteSqlRaw($"EXEC SettingsSP @Criteria='{criteria}',@ID='{Id}'");
                        return CommonResponse.Ok(msg);

                    }
                    else
                    {
                        return CommonResponse.Error("Settings is not deleted");
                    }
                }


            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }


    
        //called by MaSettingsController/KeyValue
        //**************************** KeyValue ****************************************
        public CommonResponse KeyValue(string key)
        {
            try
            {
                string msg = null;
                var keys = _context.MaSettings.Any(i => i.Key == key);
                if(keys)
                {
                    var value = _context.MaSettings.Where(i => i.Key == key)
                              .Select(i => i.Value)
                              .SingleOrDefault();
                    return CommonResponse.Ok(value);
                }               
                    msg = "key Not Found";
                    return CommonResponse.NotFound(msg);
                
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }

        }
    }
}
