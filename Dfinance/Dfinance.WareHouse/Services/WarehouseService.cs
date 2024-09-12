using Dfinance.Application.Services.General.Interface;
using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.General;
using Dfinance.Core.Views.Inventory;
using Dfinance.DataModels.Dto;
using Dfinance.DataModels.Dto.Inventory;
using Dfinance.DataModels.Dto.Warehouse;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Dfinance.Warehouse.Services.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;
namespace Dfinance.Warehouse.Services

{
    public class WarehouseService : IWarehouseService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private  readonly DataRederToObj _rederToObj;
        private readonly ILogger<IWarehouseService> _logger;
        private readonly IUserTrackService _userTrackService;
        public WarehouseService(DFCoreContext context, IAuthService authService,DataRederToObj dataRederToObj, ILogger<IWarehouseService> logger, IUserTrackService userTrackService)
        {
            _context = context;
            _authService = authService;
            _rederToObj = dataRederToObj;
            _logger = logger;
            _userTrackService = userTrackService;
        }
        /// <summary> 
        /// DropDown Location type 
        /// </summary>
        /// <returns>Id,Name</returns>
        public CommonResponse WarehouseDropdown()
        {
            var data = _context.DropDownViewName.FromSqlRaw("Exec DropDownListSP @Criteria = 'FillLocationTypes'").ToList();
            return CommonResponse.Ok(data);
        }  /// <summary> 
           /// DropDown Location type 
           /// </summary>
           /// <returns>Id,Name</returns>
        public CommonResponse WarehouseDropdownUsingBranch(int createdBranchId)
        {
            //int createdBranchId = _authService.GetBranchId().Value;
            var data = _context.DropDownViewIsdeft.FromSqlRaw($"Exec spLocations @Criteria = 'FillLocationusingBranch',@BranchID='{createdBranchId}'").ToList();
            return CommonResponse.Ok(data);
        }
        /// <summary>
        /// Fill Warehouse details in Side bar
        /// </summary>
        /// <returns>Id,Name,Code</returns>
        public CommonResponse WarehouseFillMaster()
        {
            try
            {
                var data = _context.ReadView.FromSqlRaw($"Exec SpLocations @Criteria='FillMaster'").ToList();
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        /// <summary>
        /// Warehouse fill by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CommonResponse WarehouseFillById(int Id)
        {
            try
            {
                string criteria = "FillByID";
                WareHouseView wareHouseView = new WareHouseView();

                _context.Database.GetDbConnection().Open();

                using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
                {
                    int BranchId = _authService.GetBranchId().Value;


                    dbCommand.CommandText = $"EXEC SpLocations @Criteria='{criteria}', @BranchID='{BranchId}', @ID='{Id}'";
                       

                    using (var reader = dbCommand.ExecuteReader())
                    {
                        // User Details
                        wareHouseView.warehousebyid = _rederToObj.Deserialize<Warehousebyid>(reader).FirstOrDefault();

                        // Branch Details
                        reader.NextResult();
                        wareHouseView.warehousebranch = _rederToObj.Deserialize<WarehouseBranchView>(reader);
                    }

                    return CommonResponse.Ok(new {  WarehouseView = wareHouseView });
                }
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
            finally
            {
                _context.Database.CloseConnection(); 
            }
        }




        /// <summary>
        /// Save Warehouse deatils
        /// </summary>
        /// <param name="warehouseDto"></param>
        /// <returns></returns>
        public CommonResponse Save(WarehouseDto warehouseDto)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(warehouseDto);
                if (warehouseDto.Id == null || warehouseDto.Id == 0)
                {
                    int createdBy = _authService.GetId().Value;
                    int createdBranchId = _authService.GetBranchId().Value;
                    DateTime createdOn = DateTime.Now;
                    string criteria = "InsertLocations";
                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    var data = _context.Database.ExecuteSqlRaw("EXEC SpLocations @Criteria={0},@LocationTypeID={1},@Code={2},@Name={3},@Address={4}," +
                         "@Remarks={5},@ClearingPerCFT={6},@GroundRentPerCFT={7},@LottingPerPiece={8},@LorryHirePerCFT={9},@Active={10}," +
                         "@BranchID={11},@CreatedBy={12},@CreatedOn={13},@NewID={14} OUTPUT",
                         criteria, warehouseDto.Type.Id, warehouseDto.Code, warehouseDto.Name,
                         warehouseDto.Address, warehouseDto.Remarks, warehouseDto.ClearingChargePerCFT,
                         warehouseDto.GroundRentPerCFT, warehouseDto.LottingPerPiece, warehouseDto.LorryHirePerCFT, warehouseDto.Active, createdBranchId, createdBy, createdOn, newId);
                    //int newlocId = (int)newId.Value;
                    warehouseDto.Id= (int)newId.Value;  
                    var branchdetails = SaveLocationBranchList(warehouseDto);
                    _userTrackService.AddUserActivity(warehouseDto.Name, warehouseDto?.Id??0, 0, "Add", "Location", "WareHouse", 0, jsonData);

                    return CommonResponse.Created(new { msg = "WareHouse " + warehouseDto.Name + " Created Successfully", data = 0 });
                }
                else
                {
                    if (warehouseDto.Type.Value == "InTransit")
                    {
                        return CommonResponse.Error("Updation not possiable in Type IN TRANSIT ");
                    }
                    var Location = _context.Locations
                         .Where(x => x.Id == warehouseDto.Id )
                         .FirstOrDefault();
                    if (Location == null)
                    {
                        return CommonResponse.NotFound("Warehouse Not Found");
                    }
                    int createdBy = _authService.GetId().Value;
                        int createdBranchId = _authService.GetBranchId().Value;
                        {
                            string mode = "2";
                            var result = _context.Database.ExecuteSqlRaw("EXEC SpLocations @Mode={0},@LocationTypeID={1},@Code={2},@Name={3},@Address={4}," +
                                 "@Remarks={5},@ClearingPerCFT={6},@GroundRentPerCFT={7},@LottingPerPiece={8},@LorryHirePerCFT={9},@Active={10}," +
                                 "@BranchID={11},@CreatedBy={12},@CreatedOn={13},@ID={14} ",
                                 mode, warehouseDto.Type.Id, warehouseDto.Code, warehouseDto.Name,
                                 warehouseDto.Address, warehouseDto.Remarks, warehouseDto.ClearingChargePerCFT,
                                 warehouseDto.GroundRentPerCFT, warehouseDto.LottingPerPiece, warehouseDto.LorryHirePerCFT, warehouseDto.Active, createdBranchId, createdBy, DateTime.Now, warehouseDto.Id);

                        var branchdetails = SaveLocationBranchList(warehouseDto);
                        _userTrackService.AddUserActivity(warehouseDto.Name, warehouseDto.Id??0, 1, "Update", "Location", "WareHouse", 0, jsonData);

                        return CommonResponse.Created(new { msg = "WareHouse " + warehouseDto.Name + " Updated Successfully", data = 0 });
                        }
                    }
                
            }
            catch (Exception ex)           
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        /// <summary>
        /// save warehouse corresponding Branchlist
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="warehouseDto"></param>
        /// <returns></returns>
        private int SaveLocationBranchList(WarehouseDto warehouseDto)
        {
            try
            {
                var locBranchId = _context.LocationBranchList.Any(i => i.LocationId == warehouseDto.Id);
                var jsonData = JsonSerializer.Serialize(warehouseDto);
                //if (warehouseDto.Id == null || warehouseDto.Id == 0)
                if (!locBranchId)
                {
                    int createdBy = _authService.GetId().Value;
                    int createdBranchId = _authService.GetBranchId().Value;
                    string criteria = "InsertLocationBranchList";
                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    _context.Database.ExecuteSqlRaw("EXEC SpLocations @Criteria={0},@LocationID={1},@BranchID={2},@IsDefault={3},@Active={4},@NewID={5} OUTPUT",
                                                    criteria, warehouseDto.Id, createdBranchId, warehouseDto.IsDefault, warehouseDto.Active, newId);
                    int newlocationId = (int)newId.Value;
                    _userTrackService.AddUserActivity(criteria, newlocationId, 0, "Add", "LocationBranchList", "WareHouse", 0, jsonData);

                    return 1;
                }
                else
                {
                    {
                        int createdBy = _authService.GetId().Value;
                        int createdBranchId = _authService.GetBranchId().Value;
                        int wareid = _context.LocationBranchList
                                      .Where(i => i.LocationId == warehouseDto.Id)
                                      .Select(i => i.Id)
                                      .FirstOrDefault();
                        if (wareid == 0)
                        {
                            return 0;
                        }
                        string criteria = "UpdateLocationBranchList";
                        var result = _context.Database.ExecuteSqlRaw("EXEC SpLocations @Criteria={0},@LocationID={1},@BranchID={2},@IsDefault={3},@Active={4},@ID={5} ",
                                                          criteria, warehouseDto.Id, createdBranchId, warehouseDto.IsDefault, warehouseDto.Active, wareid);
                        _userTrackService.AddUserActivity(criteria, warehouseDto?.Id??0, 1, "Update", "LocationBranchList", "WareHouse", 0, jsonData);
                        return 1;
                    }
                }
                }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        /// Delete WareHouse From Location &BranchList
        /// Active =0
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CommonResponse Delete(int Id)
        {
            try
            {
                var loc =_context.Locations.Find(Id);
                if (loc != null)
                {
                    int mode = 3;
                    var Criteria = "DeleteLocationBranchList";
                    _context.Database.ExecuteSqlRaw("Exec SpLocations @Mode={0},@ID={1}", mode, loc.Id);
                    var branchid= _context.LocationBranchList
                                      .Where(i => i.LocationId == Id)
                                      .Select(i => i.Id)
                                      .FirstOrDefault();
                    _context.Database.ExecuteSqlRaw("Exec SpLocations @Criteria={0},@ID={1}", Criteria, branchid);
                    return CommonResponse.Ok(new { msg = "WareHouse " + loc.Name + " Delete Successfully", data = 0 });
                }
                else
                {
                    return CommonResponse.NotFound(loc.Id);
                }
            }catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        /// <summary>
        /// BranchWiseWarehouseFill in Sales&purchasefrm
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CommonResponse BranchWiseWarehouseFill()
        {
            try
            {
                var Cri = "FillLocationusingBranch";
                int createdBranchId = _authService.GetBranchId().Value;
               var result= _context.Warehousebranchfill.FromSqlRaw($"exec spLocations @Criteria = '{Cri}',@BranchId = '{createdBranchId}'");
                return CommonResponse.Ok(new { result });
            }
            catch (Exception ex)
            { 
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse FillLocationMaster()
        {
            var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"Exec spLocationTypes @Criteria='FillMaster'";
            if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                _context.Database.GetDbConnection().Open();
            using (var reader = cmd.ExecuteReader())
            {
                var tb = new DataTable();
                tb.Load(reader);
                if (tb.Rows.Count > 0)
                {
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in tb.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in tb.Columns)
                        {
                            row.Add(col.ColumnName.Replace(" ", ""), dr[col].ToString().Trim());
                        }
                        rows.Add(row);
                    }
                    return CommonResponse.Ok(rows);
                }
                return CommonResponse.NoContent();
            }
        }
        public CommonResponse FillLocationById(int Id)
        {
            var exist = _context.LocationTypes.Any(x => x.Id == Id);
            if (!exist)
                return CommonResponse.NoContent("No data");
            string criteria = "FillByID";
            var data = _context.LocationTypeViews.FromSqlRaw($"Exec spLocationTypes @Criteria='{criteria}',@ID={Id}").ToList();
            return CommonResponse.Ok(data);

        }
        private CommonResponse SaveLocationTypes(LocationTypeDto locationTypeDto)
        {
            string criteria = "InsertLocationTypes";
            var branchId=_authService.GetBranchId().Value;
            var userId=_authService.GetId().Value;
            var date=DateTime.Now;
            //SqlParameter newIdParameter = new SqlParameter("@NewID", SqlDbType.Int)
            //{
            //    Direction = ParameterDirection.Output
            //};
            var newIdParameter = _context.LocationTypes.Max(x => x.Id)+1;
            _context.Database.ExecuteSqlRaw(
                "EXEC spLocationTypes @Criteria={0}, @LocationType={1}, @CreatedBy={2}, @CreatedOn={3}, " +
                "@ActiveFlag={4},@CompanyID={5}, @ID={6}",
                criteria,
                locationTypeDto.LocationType,
                userId,
                date,
                1,
                branchId,
            newIdParameter);

            return CommonResponse.Created($"LocationType {locationTypeDto.LocationType} created successfully with ID {newIdParameter}");
        }
        private CommonResponse UpdateLocationTypes(LocationTypeDto locationTypeDto)
        {
            string criteria = "UpdateLocationTypes";
            var branchId = _authService.GetBranchId().Value;
            var userId = _authService.GetId().Value;
            var date = DateTime.Now;
           
            _context.Database.ExecuteSqlRaw(
                "EXEC spLocationTypes @Criteria={0}, @LocationType={1}, @CreatedBy={2}, @CreatedOn={3}, " +
                "@ActiveFlag={4},@CompanyID={5}, @ID={6}",
                criteria,
                locationTypeDto.LocationType,
                userId,
                date,
                1,
                branchId,
            locationTypeDto.ID);

            return CommonResponse.Created($"LocationType {locationTypeDto.LocationType} Updated successfully with ID {locationTypeDto.ID}");
        }
        private CommonResponse PermissionDenied(string msg)
        {
            _logger.LogInformation("No Permission for " + msg);
            return CommonResponse.Error("No Permission ");
        }
        private CommonResponse PageNotValid(int pageId)
        {
            _logger.LogInformation("Page not Exists :" + pageId);
            return CommonResponse.Error("Page not Exists");
        }
        public CommonResponse SaveLocation(LocationTypeDto LocationTypeDto, int PageId)
        {
            try
            {
                var moduleName = _context.MaPageMenus.Where(p => p.Id == PageId).Select(p => p.MenuText).FirstOrDefault();
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 2))
                {
                    return PermissionDenied("Save LocationType");
                }
                
                var jsonData = JsonSerializer.Serialize(LocationTypeDto);
                if (LocationTypeDto.ID == null || LocationTypeDto.ID == 0)
                {
                    var msg = SaveLocationTypes(LocationTypeDto).Data;
                    _userTrackService.AddUserActivity(LocationTypeDto.LocationType, LocationTypeDto.ID, 0, "Add", "LocationType", moduleName, 0, jsonData);
                    return CommonResponse.Ok(msg);

                }
                else
                {
                    var msg = UpdateLocationTypes(LocationTypeDto).Data;
                    _userTrackService.AddUserActivity(LocationTypeDto.LocationType, LocationTypeDto.ID, 1, "Update", "LocationType", moduleName, 0, jsonData);
                    return CommonResponse.Ok(msg);
                }
               
               
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
        public CommonResponse DeleteLocationType(int Id,int PageId)
        {
            try
            {
                var moduleName = _context.MaPageMenus.Where(p => p.Id == PageId).Select(p => p.MenuText).FirstOrDefault();
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 2))
                {
                    return PermissionDenied("Save LocationType");
                }
                string msg = null;
                var name = _context.LocationTypes.Where(i => i.Id == Id).
                   Select(i => i.LocationType).
                   SingleOrDefault();
                if (name == null)
                {
                    msg = "This LocationType is Not Found";
                }
                else
                {                    
                    var result = _context.Database.ExecuteSqlRaw($"EXEC spLocationTypes @Mode=3,@ID='{Id}'");
                    msg = name + " Deleted Successfully";
                    _userTrackService.AddUserActivity(Id.ToString(),Id, 2, "Deleted", "LocationType", moduleName, 0, null);

                }
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }
    } 
}
