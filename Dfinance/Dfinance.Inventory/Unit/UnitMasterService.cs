using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.Inventory.Interface;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dfinance.Inventory
{
    public class UnitMasterService : IUnitMasterService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IUserTrackService _userTrackService;
        private ILogger<UnitMasterService> _logger;
        public UnitMasterService(DFCoreContext context, IAuthService authService, IUserTrackService userTrackService,ILogger<UnitMasterService> logger)
        {
            _context = context;
            _authService = authService;
            _userTrackService = userTrackService;
            _logger = logger;
        }

        //called by UnitMasterController/FillMaster
        //**************************** FillMaster ****************************************
        public CommonResponse FillMaster()
        {
            try
            {
                var result = _context.SpFillUnitMaster.FromSqlRaw($"EXEC UnitMasterSP @Criteria ='FillMaster'").ToList();
                _logger.LogInformation("Successfully filled");
                return CommonResponse.Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        //called by UnitMasterController/FillByUnit
        //**************************** FillByUnit ****************************************
        public CommonResponse FillByUnit(string unit)
        {
            try
            {
                var result = _context.UnitMaster.Where(i => i.Unit == unit).Select(i => i.Unit).SingleOrDefault();
                if (result == null)
                {
                    return CommonResponse.NotFound("Unit doesnot exist!");
                }
                var units = _context.SpFillByUnit.FromSqlRaw($"EXEC UnitMasterSP @Criteria ='FillByUnit',@Unit = '{unit}'");
                _logger.LogInformation("Successfully filled by unit");
                return CommonResponse.Ok(units);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        //called by UnitMasterController/UnitDropDown
        //**************************** UnitDropDown ****************************************
        public CommonResponse UnitDropDown()
        {
            try
            {
                var data = _context.DropDownView.FromSqlRaw("Exec DropDownListSP @Criteria='FillUnitMaster'").ToList();
                _logger.LogInformation("Successfully getting dropdown details");
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        //called by UnitMasterController/SaveUnitMaster
        //**************************** SaveUnitMaster ****************************************
        public CommonResponse SaveUnitMaster(UnitMasterDto unitmasterDto)
        {
            try
            {
                //Unit exist or not
                var units = _context.UnitMaster.Any(i => i.Unit == unitmasterDto.Unit);
                if (units)
                {
                    return CommonResponse.Error("Unit already exists");
                }

                var result = _context.Database.ExecuteSqlRaw("Exec UnitMasterSP @Criteria='Insert',@Unit={0},@Description={1},@Factor={2},@IsComplex={3},@BasicUnit={4},@AllowDelete={5},@Precision={6},@Active={7}," +
                    "@ArabicName={8}", unitmasterDto.Unit, unitmasterDto.Description, unitmasterDto.Factor, unitmasterDto.IsComplex, unitmasterDto.BasicUnit.Unit, unitmasterDto.AllowDelete, unitmasterDto.Precision, unitmasterDto.Active, unitmasterDto.ArabicName);

               
             // _userTrackService.AddUserActivity(unitmasterDto.Description, 0, 0, "Added", "UnitMaster", "Unit Master", 0);

                _logger.LogInformation("units saved");
                return CommonResponse.Ok("Inserted Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }


        //called by UnitMasterController/UpdateUnitMaster
        //**************************** UpdateUnitMaster ****************************************
        public CommonResponse UpdateUnitMaster(UnitMasterDto unitmasterDto)
        {
            try
            {
                string msg = null;
                var units = _context.UnitMaster.Where(i => i.Unit == unitmasterDto.Unit).Select(i => i.Unit).SingleOrDefault();
                if (units == null)
                {
                    msg = "Unit Not Found";
                    return CommonResponse.NotFound(msg);
                }
                
                var update = _context.Database.ExecuteSqlRaw($"Exec UnitMasterSP @Criteria='Update',@Unit='{unitmasterDto.Unit}',@Description='{unitmasterDto.Description}',@Factor='{unitmasterDto.Factor}',@IsComplex='{unitmasterDto.IsComplex}',@BasicUnit='{unitmasterDto.BasicUnit.Unit}',@AllowDelete='{unitmasterDto.AllowDelete}'," +
                    $"@Precision='{unitmasterDto.Precision}',@Active='{unitmasterDto.Active}',@ArabicName='{unitmasterDto.ArabicName}'");

                //   _userTrackService.AddUserActivity(unitmasterDto.Description, 0, 1, "Modified", "UnitMaster", "Unit Master", 0);
                _logger.LogInformation("units updated");
                return CommonResponse.Ok("Updated Sucessfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }

        }

        //called by UnitMasterController/DeleteUnitMaster
        //**************************** DeleteUnitMaster ****************************************
        public CommonResponse DeleteUnitMaster(string unit)
        {
            try
            {
                var unitMaster = _context.UnitMaster.SingleOrDefault(i => i.Unit == unit);

                if (unitMaster == null)
                {
                    return CommonResponse.NotFound("Unit Not Found");
                }
                else if (unitMaster.AllowDelete == false)
                {
                    return CommonResponse.Error("This unit cannot be deleted");
                }
                string reference = unitMaster.Description;
                var result = _context.Database.ExecuteSqlRaw($"EXEC UnitMasterSP @Criteria='Delete',@Unit='{unit}'");

                //   _userTrackService.AddUserActivity(reference, 0, 0, "Deleted", "UnitMaster", "Unit Master", 0);
                _logger.LogInformation("Deleted successfully");
                return CommonResponse.Ok("Unit deleted Successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }


        }
    }
}
