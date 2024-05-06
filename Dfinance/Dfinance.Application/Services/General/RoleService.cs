using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.General;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Application.Services.General
{
    public class RoleService : IRoleService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly DataRederToObj _rederToObj;
        private readonly ILogger<RoleService> _logger;

        public RoleService(DFCoreContext context, IAuthService authService, DataRederToObj rederToObj,ILogger<RoleService> logger)
        {
            _authService = authService;
            _context = context;
            _rederToObj = rederToObj;
            _logger = logger;


        }

        /*************************FillRoleMaster*****************/
        public CommonResponse FillRoleMaster()
        {
            try
            {
                int branchId = _authService.GetBranchId().Value;
                string Criteria = "FillRolesMaster";
                var data = _context.FillRole
                    .FromSqlRaw($"Exec spNewRoleSettings @Criteria='{Criteria}', @BranchID='{branchId}'")
                    .ToList();
                _logger.LogInformation("Successfully filled");
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error("");
            }
        }

        /*************************FillRoleRight*****************/
        public CommonResponse FillRoleRights()
        {
            try
            {
                string Criteria = "FillRoleRights";
                var data = _context.FillRoleRight.FromSqlRaw($"EXEC spNewRoleSettings @Criteria ='{Criteria}'").ToList();
                _logger.LogInformation("Successfully filled");
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error("");
            }
        }

        /*************************FillRoleandRoleRights*****************/

        public CommonResponse FillRoleandRoleRights(int Id)
        {
            try
            {
                string Criteria = "FillRoleandRoleRights";
                SpFillRoles fill = new SpFillRoles();
                _context.Database.GetDbConnection().Open();

                using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandText = $"Exec spNewRoleSettings @Criteria='{Criteria}',@ID='{Id}'";
                    using (var reader = dbCommand.ExecuteReader())
                    {
                        fill.fillRole = _rederToObj.Deserialize<FillRole>(reader).FirstOrDefault();
                        reader.NextResult();
                        fill.fillRoleRights = _rederToObj.Deserialize<FillRoleRight>(reader);
                    }

                    _logger.LogInformation("Successfully filled both");
                    return CommonResponse.Ok(fill);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }

        /***********************Save*****************************/

        public CommonResponse AddRole(RoleDto roleDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int CreatedBy = _authService.GetId().Value;
                    int CreatedBranchId = _authService.GetBranchId().Value;
                    DateTime CreatedOn = DateTime.Now;

                    string criteria = "InsertMaRoles";
                    SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    var data = _context.Database.ExecuteSqlRaw("EXEC spNewRoleSettings @Criteria ={0},@Role={1},@Active={2},@BranchID={3},@CreatedBy={4},@CreatedOn={5},@NewID={6} OUTPUT", criteria,
                        roleDto.Role, roleDto.Active, CreatedBranchId, CreatedBy, CreatedOn, newIdparam);
                    int NewIdUser = (int)newIdparam.Value;
                    //return NewIdUser;
                    Saveroleright(roleDto.RolerightDto, NewIdUser);
                
                    transaction.Commit();
                    _logger.LogInformation("role saved sucessfully");
                    return CommonResponse.Ok("insert sucessfully");
                }
                catch (Exception ex)
                {
                   
                    transaction.Rollback();
                    _logger.LogError(ex.Message);
                    return CommonResponse.Error(ex.Message);
                }
            }
        }


        private void Saveroleright(List<RolerightDto> rolerightDtos, int RoleId)
        {
            foreach (var rolerightDto in rolerightDtos)
            {
               
                string criteria = "InsertMaRoleRights";
                SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("EXEC spNewRoleSettings @Criteria ={0},@RoleID={1},@PageMenuID={2},@IsView={3},@IsCreate={4},@IsEdit={5},@IsCancel={6}," +
                    "@IsDelete={7},@IsApprove={8},@IsEditApproved={9},@IsHigherApprove={10},@IsPrint={11},@IsEmail={12},@NewID={6} OUTPUT", 
                    criteria, RoleId, rolerightDto.PageMenuId, rolerightDto.IsView, rolerightDto.IsCreate, rolerightDto.IsEdit, rolerightDto.IsCancel,
                    rolerightDto.IsDelete, rolerightDto.IsApprove, rolerightDto.IsEditApproved, rolerightDto.IsHigherApprove, rolerightDto.IsPrint, rolerightDto.IsEmail, newIdparam);

               // int NewIdUser = (int)newIdparam.Value;
               
            }
        }


        public CommonResponse UpdateRole(RoleDto roleDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var userRole = _context.UserRoles.Where(i => i.Id == roleDto.Id).Select(i => i.Id).SingleOrDefault();
                    if (userRole == 0)
                    {
                        return CommonResponse.Error("Role Id not found");
                    }
                    string criteria = "UpdateMaRoles";
                    int CreatedBy = _authService.GetId().Value;
                    int CreatedBranchId = _authService.GetBranchId().Value;
                    DateTime CreatedOn = DateTime.Now;
                    _context.Database.ExecuteSqlRaw("EXEC spNewRoleSettings @Criteria ={0},@Role={1},@Active={2},@BranchID={3},@CreatedBy={4},@CreatedOn={5},@ID={6}", criteria,roleDto.Role,
                        roleDto.Active, CreatedBranchId, CreatedBy, CreatedOn,roleDto.Id);
                    var existingUserDetails = _context.MaRoleRights.Where(d => d.RoleId == roleDto.Id).ToList();
                    _context.MaRoleRights.RemoveRange(existingUserDetails);
                    _context.SaveChanges();
                    Saveroleright(roleDto.RolerightDto, roleDto.Id);

                    transaction.Commit();
                    _logger.LogInformation("role updated");
                    return CommonResponse.Created("Successfully Updated");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex.Message);
                    return CommonResponse.Error(ex.Message);
                }
            }
        }


        
        /*******************Delete********************/
            public CommonResponse DeleteRole(int Id)
            {
            try
            {
                
                var userRole = _context.UserRoles.Where(i => i.Id == Id).Select(i => i.Id).SingleOrDefault();
                if (userRole == 0)
                {
                    var msg = "Id Not Found";
                    return CommonResponse.NotFound(msg);
                }
                string criteria = "DeleteMaRoles";

                var result = _context.Database.ExecuteSqlRaw($"EXEC spNewRoleSettings @Criteria='{criteria}', @ID='{Id}'");
                var del = _context.MaRoleRights.Where(d => d.RoleId == Id).ToList();
                _context.MaRoleRights.RemoveRange(del);
                _context.SaveChanges();
                _logger.LogInformation("role deleted");
                return CommonResponse.Ok("Deleted Sucessfully");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex.Message);
            }
        }

    }
}
