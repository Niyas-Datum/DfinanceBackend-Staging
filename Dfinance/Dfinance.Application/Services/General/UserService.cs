using Dfinance.Application.Services.General.Interface;
using Dfinance.Application.Services.Interface;
using Dfinance.AuthApplication.Services.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.General;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Deserialize;
using Dfinance.Shared.Domain;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dfinance.Application.Services.General
{
    public class UserService : IUserService

    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IEncryptService _encryptService;
        private readonly IDecryptService _decryptService;
        private readonly DataRederToObj _rederToObj;
        public UserService(DFCoreContext context, IAuthService authService, IEncryptService encryptService, DataRederToObj rederToObj)
        {
            _context = context;
            _authService = authService;
            _encryptService = encryptService;
            _rederToObj = rederToObj;
        }
        public CommonResponse UserDropDown()
        {
            try
            {
                var data = _context.DropDownViewName.FromSqlRaw("Exec DropDownListSP @Criteria='FillEmployees'").ToList();

                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        //888888888888888888888888888*UserGet*8888888888888888888888888888888888888888888888888888888888888888
        public CommonResponse FillUser()
        {
            try
            {
                int BranchId = _authService.GetBranchId().Value;
                string criteria = "FillEmployeeMaster";
                var data = _context.SpUser.FromSqlRaw($"Exec spNewEmployees @Criteria='{criteria}',@BranchID='{BranchId}'").ToList();

                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        /// <summary>
        /// Fill  Role DropDown
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public CommonResponse GetRole()
        {
            try
            {
                int CreatedBranchId = _authService.GetBranchId().Value;
                string criteria = "FillRoles";
                var data = _context.GetRole.FromSqlRaw($"Exec DropDownListSP @Criteria='{criteria}',@IntParam='{CreatedBranchId}'").ToList();


                return CommonResponse.Ok(data);


            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        /// <summary>
        /// Fill User Role
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public CommonResponse FillRole(int RoleId)
        {
            try
            {

                string criteria = "FillUserRights";
                var data = _context.RoleRightsModel.FromSqlRaw($"Exec spNewEmployees @Criteria='{criteria}',@RoleID='{RoleId}'").ToList();


                return CommonResponse.Ok(data);


            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }


        //888888888888888888888888888*UserGetById*8888888888888888888888888888888888888888888888888888888888888888
        public CommonResponse FillUserById(int Id)
        {
            try
            {
                string criteria = "FillUsers";
                SpUserGById fillUserResponse = new SpUserGById();

                _context.Database.GetDbConnection().Open();

                using (var dbCommand = _context.Database.GetDbConnection().CreateCommand())
                {
                    dbCommand.CommandText = $"Exec spNewEmployees @Criteria='{criteria}',@ID='{Id}'";

                    using (var reader = dbCommand.ExecuteReader())
                    {
                        // User Details
                        fillUserResponse.UserDetails = _rederToObj.Deserialize<FillUserSpView>(reader).FirstOrDefault();

                        // Branch Details
                        reader.NextResult();
                        fillUserResponse.BranchDetails = _rederToObj.Deserialize<UserBranchDetails>(reader);

                        // User Rights
                        reader.NextResult();
                        fillUserResponse.UserRights = _rederToObj.Deserialize<UserRights>(reader);

                        // Location Restrictions
                        reader.NextResult();
                        fillUserResponse.LocationRestrictions = _rederToObj.Deserialize<LocationRestriction>(reader);
                    }
                }

                if (fillUserResponse.UserDetails != null)
                {
                    return CommonResponse.Ok(fillUserResponse);
                }

                return CommonResponse.NotFound("User not found");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }

        /// <summary>
        /// Add User
        /// Frm:Gen=>User
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public CommonResponse SaveUser(UserDto userDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int createdBy = _authService.GetId().Value;
                    int createdBranchId = _authService.GetBranchId().Value;

                    string encryptedPassword = _encryptService.Encrypt(userDto.Password);

                    int newUserId = InsertMaEmployees(userDto, createdBranchId, encryptedPassword);
                    if (newUserId == 0)
                        return CommonResponse.Error("Failed to insert user.");

                    InsertMaEmployeeDetails(userDto, newUserId);

                    transaction.Commit();

                    return CommonResponse.Created("Successfully Inserted");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return CommonResponse.Error($"Failed to insert user. Error: {ex.Message}");
                }
            }
        }

        private int InsertMaEmployees(UserDto userDto, int createdBranchId, string encryptedPassword)
        {
            string criteria = "InsertMaEmployeesweb";
            SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            if (userDto.Account.Id == 0)
            {
                userDto.Account.Id = null;
            }
            _context.Database.ExecuteSqlRaw("EXEC spNewEmployees @Criteria={0}, @FirstName={1},@MiddleName={2},@LastName={3},@Address={4},@EmailID={5},@OfficeNumber={6},@MobileNumber={7},@DesignationID={8},@Active={9},@EmployeeType={10},@UserName={11},@Password={12},@GmailID={13},@IsLocationRestrictedUser={14},@PhotoID={15},@CreatedBranchId={16},@AccountID={17},@ImagePath={18},@CashAccountId={19},@WarehouseId={20},@NewID={21} OUTPUT", criteria,
                    userDto.FirstName, userDto.MiddleName, userDto.LastName, userDto.Address, userDto.EmailId, userDto.OfficeNumber, userDto.MobileNumber, userDto.Designation.Id, userDto.Active, userDto.EmployeeType.Id, userDto.Username, encryptedPassword, userDto.GmailId, userDto.IsLocationRestrictedUser, userDto.PhotoId, createdBranchId, userDto.Account.Id, userDto.ImagePath,userDto.CashAccountId.Id,userDto.WarehouseId.Id ,newIdparam);

            return (int)newIdparam.Value;
        }

        private void InsertMaEmployeeDetails(UserDto userDto, int newUserId)
        {
            foreach (var branchDetail in userDto.UserBranchDetails)
            {
                string criteria1 = "InsertMaEmployeeDetails";

                SqlParameter newIdUserdetails = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                _context.Database.ExecuteSqlRaw("EXEC spNewEmployees @Criteria={0},@EmployeeID={1},@BranchID={2},@DepartmentID={3},@ActiveFlag={4},@IsMainBranch={5},@SupervisorID={6},@MaRoleID={7},@NewID={8} OUTPUT", criteria1,
                    newUserId, branchDetail.BranchName.Id, branchDetail.DepartmentName.Id, branchDetail.ActiveFlag, branchDetail.IsMainBranch, branchDetail.Supervisor.Id, branchDetail.MaRoleId, newIdUserdetails);

                int newUserDetailId = (int)newIdUserdetails.Value;

                // Insert user rights based on this branch detail
                InsertMaUserRights(branchDetail, newUserDetailId);
            }
        }

        private void InsertMaUserRights(UserBranchDetailsDto branchDetail, int newUserDetailId)
        {
            foreach (var maUserRight in branchDetail.MapagemenuDto)
            {
                string criteria2 = "InsertMaUserRights";

                SqlParameter newIdUserRight = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                _context.Database.ExecuteSqlRaw("EXEC spNewEmployees @Criteria={0},@UserDetailsID={1},@PageMenuID={2},@IsView={3},@IsCreate={4},@IsEdit={5},@IsCancel={6},@IsDelete={7},@IsApprove={8},@IsEditApproved={9},@IsHigherApprove={10},@IsPrint={11},@IsEmail={12},@FrequentlyUsed={13},@NewID={14} OUTPUT",
                    criteria2, newUserDetailId, maUserRight.PageMenuId, maUserRight.IsView, maUserRight.IsCreate, maUserRight.IsEdit, maUserRight.IsCancel, maUserRight.IsDelete, maUserRight.IsApprove, maUserRight.IsEditApproved, maUserRight.IsHigherApprove, maUserRight.IsPrint, maUserRight.IsEmail, maUserRight.FrequentlyUsed, newIdUserRight);
            }
        }


        /// <summary>
        /// Update User 
        /// Frm:General=>User
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CommonResponse UpdateUser(UserDto userDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                   var userfind =_context.MaEmployees.FirstOrDefault(x=>x.Id==userDto.Id);
                    if (userDto.Id == 0)
                    {
                        return CommonResponse.Error("User Not Found");
                    }

                    int createdBranchId = _authService.GetBranchId().Value;
                    var encryptedPassword = _encryptService.Encrypt(userDto.Password);

                    if (userDto.Account.Id == 0)
                    {
                        userDto.Account.Id = null;
                    }

                    string criteria = "UpdateMaEmployeesweb";
                    _context.Database.ExecuteSqlRaw("EXEC spNewEmployees @Criteria={0}, @FirstName={1},@MiddleName={2},@LastName={3},@Address={4},@EmailID={5},@OfficeNumber={6},@MobileNumber={7},@DesignationID={8},@Active={9},@EmployeeType={10},@UserName={11},@Password={12},@GmailID={13},@IsLocationRestrictedUser={14},@PhotoID={15},@CreatedBranchId={16},@AccountID={17},@ImagePath={18},@CashAccountId={19},@WarehouseId={20},@ID={21}", criteria,
                         userDto.FirstName, userDto.MiddleName, userDto.LastName, userDto.Address, userDto.EmailId, userDto.OfficeNumber, userDto.MobileNumber, userDto.Designation.Id, userDto.Active, userDto.EmployeeType.Id, userDto.Username, encryptedPassword, userDto.GmailId, userDto.IsLocationRestrictedUser, userDto.PhotoId, createdBranchId, userDto.Account.Id, userDto.ImagePath, userDto.CashAccountId.Id, userDto.WarehouseId.Id ,userDto.Id);

                    var existingUserDetails = _context.MaEmployeeBranchDet.Where(d => d.EmployeeId == userDto.Id).ToList();
                    var existingUserDetailsIds = existingUserDetails.Select(d => d.Id).ToList();

                    // Remove existing user rights
                    var existingUserRights = _context.UserRolePermission.Where(r => existingUserDetailsIds.Contains(r.UserDetailsId)).ToList();
                    _context.UserRolePermission.RemoveRange(existingUserRights);
                    _context.SaveChanges();

                    // Remove existing branch details
                    _context.MaEmployeeBranchDet.RemoveRange(existingUserDetails);
                    // Save changes to the database
                    _context.SaveChanges();
                    // Insert new branch details and user rights
                    InsertMaEmployeeDetails(userDto, userfind.Id);


                    transaction.Commit();
                    return CommonResponse.Created("Successfully Updated");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return CommonResponse.Error(ex.Message);
                }
            }
        }


        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        public CommonResponse Deleteuser(int Id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string msg = null;
                    var user = _context.MaEmployees.Include(x => x.EmployeeBranchDetails)
                                                    .ThenInclude(d => d.MaUserPagePermisions)
                                                    .FirstOrDefault(x => x.Id == Id);
                    if (user != null)
                    {
                        // Remove user rights
                        foreach (var branchDetail in user.EmployeeBranchDetails)
                        {
                            _context.UserRolePermission.RemoveRange(branchDetail.MaUserPagePermisions);
                        }
                        _context.MaEmployeeBranchDet.RemoveRange(user.EmployeeBranchDetails);
                        _context.MaEmployees.Remove(user);

                       
                        _context.SaveChanges();
                        transaction.Commit();

                        msg = "User and other details deleted successfully";
                        return CommonResponse.Ok(msg);
                    }
                    else
                    {
                        return CommonResponse.Error("User Not Found");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return CommonResponse.Error("This user cannot be deleted because it is used in other forms");
                }
            }
        }

    }

}





