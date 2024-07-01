using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.General;
using Dfinance.Core.Views.Inventory;
using Dfinance.DataModels.Dto;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Dfinance.Warehouse.Services.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace Dfinance.Warehouse.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private  readonly DataRederToObj _rederToObj;
        public WarehouseService(DFCoreContext context, IAuthService authService,DataRederToObj dataRederToObj)
        {
            _context = context;
            _authService = authService;
            _rederToObj = dataRederToObj;
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
                         criteria, warehouseDto.Type.ID, warehouseDto.Code, warehouseDto.Name,
                         warehouseDto.Address, warehouseDto.Remarks, warehouseDto.ClearingChargePerCFT,
                         warehouseDto.GroundRentPerCFT, warehouseDto.LottingPerPiece, warehouseDto.LorryHirePerCFT, warehouseDto.Active, createdBranchId, createdBy, createdOn, newId);
                    //int newlocId = (int)newId.Value;
                    warehouseDto.Id= (int)newId.Value;  
                    var branchdetails = SaveLocationBranchList(warehouseDto);
                    return CommonResponse.Created(new { msg = "WareHouse " + warehouseDto.Name + " Created Successfully", data = 0 });
                }
                else
                {
                    if (warehouseDto.Type.ID ==10)
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
                                 mode, warehouseDto.Type.ID, warehouseDto.Code, warehouseDto.Name,
                                 warehouseDto.Address, warehouseDto.Remarks, warehouseDto.ClearingChargePerCFT,
                                 warehouseDto.GroundRentPerCFT, warehouseDto.LottingPerPiece, warehouseDto.LorryHirePerCFT, warehouseDto.Active, createdBranchId, createdBy, DateTime.Now, warehouseDto.Id);

                        var branchdetails = SaveLocationBranchList(warehouseDto);
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
                //if (warehouseDto.Id == null || warehouseDto.Id == 0)
                if(!locBranchId)
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
    } 
}
