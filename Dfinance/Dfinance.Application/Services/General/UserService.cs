using Dfinance.Application.Services.General.Interface;
using Dfinance.Application.Services.Interface;
using Dfinance.AuthApplication.Services.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Domain.Roles;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views;
using Dfinance.Core.Views.General;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace Dfinance.Application.Services.General
{
    public class UserService : IUserService

    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly IEncryptService _encryptService;
        private readonly IDecryptService _decryptService;
        public UserService(DFCoreContext context, IAuthService authService, IEncryptService encryptService)
        {
            _context = context;
            _authService = authService;
            _encryptService = encryptService;

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
        //888888888888888888888888888*UserGetById*8888888888888888888888888888888888888888888888888888888888888888
        public CommonResponse FillUserById(int Id)
        {
            try
            {
                var ur = _context.MaEmployees.Where(x => x.Id == Id).Select(x => x.Id).SingleOrDefault();
                if (ur == null)
                {                   
                    return CommonResponse.NotFound("User Not Found");
                }
                string criteria = "FillUsers";
                var data = _context.SpUserGById.FromSqlRaw($"Exec spNewEmployees @Criteria='{criteria}',@ID='{Id}'").ToList();

                if (data.Count > 0)
                {
                    SpUserGById userDetails = data[0];
                    return CommonResponse.Ok(userDetails);
                }

                return CommonResponse.NotFound("User not found");
            }
            catch (Exception ex)
            {
               return CommonResponse.Error(ex);
            }
        }



        //888888888888888888888888888*Add*8888888888888888888888888888888888888888888888888888888888888888
        public CommonResponse SaveUser(UserDto userDto)
        {
            try
            {
                int CreatedBy = _authService.GetId().Value;
                int CreatedBranchId = _authService.GetBranchId().Value;
                DateTime CreatedOn = DateTime.Now;

                var Passwordtemp = _encryptService.Encrypt(userDto.Password);

                string criteria = "InsertMaEmployees";
                SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("EXEC spNewEmployees @Criteria={0}, @FirstName={1},@MiddleName={2},@LastName={3},@Address={4},@EmailID={5},@OfficeNumber={6},@MobileNumber={7},@DesignationID={8},@Active={9},@EmployeeType={10},@UserName={11},@Password={12},@GmailID={13},@IsLocationRestrictedUser={14},@PhotoID={15},@CreatedBranchId={16},@AccountID={17},@ImagePath={18},@NewID={19} OUTPUT", criteria,
                userDto.FirstName, userDto.MiddleName, userDto.LastName, userDto.Address, userDto.EmailId, userDto.OfficeNumber, userDto.MobileNumber, userDto.DesignationId, userDto.Active, userDto.EmployeeType, userDto.Username, Passwordtemp, userDto.GmailId, userDto.IsLocationRestrictedUser, userDto.PhotoId, CreatedBranchId, userDto.AccountId, userDto.ImagePath, newIdparam);

                int NewIdUser = (int)newIdparam.Value;
                //********************************************************************
                bool isMainBranchSet = false;

                foreach (var branchDetail in userDto.UserBranchDetails)
                {
                    if (branchDetail.IsMainBranch)
                    {
                        if (isMainBranchSet)
                        {
                           return CommonResponse.Error("Only one IsMainBranch is allowed per user.");
                        }
                        isMainBranchSet = true;
                    }
                    if (userDto.UserBranchDetails.Count(b => b.BranchName == branchDetail.BranchName) > 1)
                    {
                        throw new Exception("Duplicate BranchId is not allowed.");
                    }
                    string criteria1 = "InsertMaEmployeeDetails";

                    SqlParameter newIdUserdetails = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    _context.Database.ExecuteSqlRaw("EXEC spNewEmployees @Criteria={0},@EmployeeID={1},@BranchID={2},@DepartmentID={3},@ActiveFlag={4},@IsMainBranch={5},@SupervisorID={6},@MaRoleID={7},@NewID={8} OUTPUT", criteria1,
                        NewIdUser, branchDetail.BranchName, branchDetail.DepartmentId, branchDetail.ActiveFlag, branchDetail.IsMainBranch, branchDetail.SupervisorId, branchDetail.MaRoleId, newIdUserdetails);

                    int NewIdUserDetails = (int)newIdUserdetails.Value;

                    //********************************************************************
                    foreach (var maUserRight in branchDetail.MapagemenuDto)
                    {

                        string criteria2 = "InsertMaUserRights";

                        SqlParameter newIdUserRight = new SqlParameter("@NewID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        _context.Database.ExecuteSqlRaw("EXEC spNewEmployees @Criteria={0},@UserDetailsID={1},@PageMenuID={2},@IsView={3},@IsCreate={4},@IsEdit={5},@IsCancel={6},@IsDelete={7},@IsApprove={8},@IsEditApproved={9},@IsHigherApprove={10},@IsPrint={11},@IsEmail={12},@FrequentlyUsed={13},@NewID={14} OUTPUT",
                            criteria2, NewIdUserDetails, maUserRight.PageMenuId, maUserRight.IsView, maUserRight.IsCreate, maUserRight.IsEdit, maUserRight.IsCancel, maUserRight.IsDelete, maUserRight.IsApprove, maUserRight.IsEditApproved, maUserRight.IsHigherApprove, maUserRight.IsPrint, maUserRight.IsEmail, maUserRight.FrequentlyUsed, newIdUserRight);

                        int NewIdUserRighnt = (int)newIdUserRight.Value;
                    }
                }

                return CommonResponse.Created("Successfully Inserted");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex.Message);
            }
        }

        //8888888888888888888888888*UPDATION*8888888888888888888888888888888888888888888888888888888888888888888


        public CommonResponse UpdateUser(UserDto userDto, int Id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {


                    int CreatedBy = _authService.GetId().Value;
                    int CreatedBranchId = _authService.GetBranchId().Value;
                    DateTime CreatedOn = DateTime.Now;
                    var Passwordtemp = _encryptService.Encrypt(userDto.Password);
                    if (Id == 0)
                        return CommonResponse.Error("User Not Found");
                    else
                    {

                        string criteria = "UpdateMaEmployees";
                        _context.Database.ExecuteSqlRaw($"EXEC spNewEmployees @Criteria='{criteria}',@ID='{Id}',@FirstName='{userDto.FirstName}',@MiddleName='{userDto.MiddleName}',@LastName='{userDto.LastName}',@Address='{userDto.Address}',@EmailID='{userDto.EmailId}',@ResidenceNumber='{userDto.ResidenceNumber}',@OfficeNumber='{userDto.OfficeNumber}',@MobileNumber='{userDto.MobileNumber}',@DesignationID='{userDto.DesignationId}',@Active='{userDto.Active}',@EmployeeType='{userDto.EmployeeType}',@UserName='{userDto.Username}',@Password='{userDto.Password}',@GmailID='{userDto.GmailId}',@IsLocationRestrictedUser='{userDto.IsLocationRestrictedUser}',@PhotoID='{userDto.PhotoId}',@CreatedBranchId='{CreatedBranchId}',@AccountID='{userDto.AccountId}',@ImagePath='{userDto.ImagePath}'");

                        //************************************************************************

                        List<MaEmployeeDetail> maEmployeeDetailss = _context.MaEmployeeBranchDet
                          .Where(d => d.EmployeeId == Id)
                          .OrderBy(d => d.Id)
                          .ToList();

                        MaEmployeeDetail[] userDetailArray = maEmployeeDetailss.ToArray();

                        foreach (var branchDetail in userDto.UserBranchDetails)
                        {
                            bool hasMainBranch = userDetailArray.Any(detail => detail.IsMainBranch);


                            if (hasMainBranch && branchDetail.IsMainBranch)
                            {

                                return CommonResponse.Error("Only one main branch is allowed.");
                            }

                            foreach (var mauserDetail in userDetailArray)
                            {
                                int detailsid = mauserDetail.Id;
                                if (detailsid < 0)
                                {
                                    string criteriaadd = "InsertMaEmployeeDetails";

                                    SqlParameter newIdUserdetails = new SqlParameter("@NewID", SqlDbType.Int)
                                    {
                                        Direction = ParameterDirection.Output
                                    };
                                    _context.Database.ExecuteSqlRaw("EXEC spNewEmployees @Criteria={0},@EmployeeID={1},@BranchID={2},@DepartmentID={3},@ActiveFlag={4},@IsMainBranch={5},@SupervisorID={6},@MaRoleID={7},@NewID={8} OUTPUT", criteriaadd,
                                       Id, CreatedBranchId, branchDetail.DepartmentId, branchDetail.ActiveFlag, branchDetail.IsMainBranch, branchDetail.SupervisorId, branchDetail.MaRoleId, newIdUserdetails);

                                    int NewIdUserDetails = (int)newIdUserdetails.Value;

                                }
                                else
                                {

                                    string criteria1 = "UpdateMaEmployeeDetails";

                                    _context.Database.ExecuteSqlRaw("EXEC spNewEmployees @Criteria={0},@ID={1},@EmployeeID={2},@BranchID={3},@DepartmentID={4},@ActiveFlag={5},@IsMainBranch={6},@SupervisorID={7},@MaRoleID={8}", criteria1,
                                    detailsid, Id, CreatedBranchId, branchDetail.DepartmentId, branchDetail.ActiveFlag, branchDetail.IsMainBranch, branchDetail.SupervisorId, branchDetail.MaRoleId);


                                    //********************************************************************
                                    List<MaUserRight> maUsers = _context.UserRolePermission
                                     .Where(d => d.UserDetailsId == detailsid)
                                     .OrderBy(d => d.Id)
                                     .ToList();


                                    MaUserRight[] maUserRightArray = maUsers.ToArray();

                                    foreach (var userRight in branchDetail.MapagemenuDto)
                                    {
                                        int rightid = maUserRightArray.Count(r => r.Id == userRight.UserDetailsId);

                                        if (rightid < 0)

                                        {

                                            string criteria2 = "InsertMaUserRights";

                                            SqlParameter newIdUserRight = new SqlParameter("@NewID", SqlDbType.Int)
                                            {
                                                Direction = ParameterDirection.Output
                                            };
                                            _context.Database.ExecuteSqlRaw("EXEC spNewEmployees @Criteria={0},@UserDetailsID={1},@PageMenuID={2},@IsView={3},@IsCreate={4},@IsEdit={5},@IsCancel={6},@IsDelete={7},@IsApprove={8},@IsEditApproved={9},@IsHigherApprove={10},@IsPrint={11},@IsEmail={12},@FrequentlyUsed={13},@NewID={14} OUTPUT",
                                                criteria2, userRight.UserDetailsId, userRight.PageMenuId, userRight.IsView, userRight.IsCreate, userRight.IsEdit, userRight.IsCancel, userRight.IsDelete, userRight.IsApprove, userRight.IsEditApproved, userRight.IsHigherApprove, userRight.IsPrint, userRight.IsEmail, userRight.FrequentlyUsed, newIdUserRight);

                                            int NewIdUserRighnt = (int)newIdUserRight.Value;


                                        }
                                        else
                                        {
                                            // Update existing MaUserRight
                                            string criteria3 = "UpdateMaUserRights";

                                            _context.Database.ExecuteSqlRaw("EXEC spNewEmployees @Criteria={0},@ID={1},@UserDetailsID={2},@PageMenuID={3},@IsView={4},@IsCreate={5},@IsEdit={6},@IsCancel={7},@IsDelete={8},@IsApprove={9},@IsEditApproved={10},@IsHigherApprove={11},@IsPrint={12},@IsEmail={13},@FrequentlyUsed={14}",
                                                criteria3, rightid, detailsid, userRight.PageMenuId, userRight.IsView, userRight.IsCreate, userRight.IsEdit, userRight.IsCancel, userRight.IsDelete, userRight.IsApprove, userRight.IsEditApproved, userRight.IsHigherApprove, userRight.IsPrint, userRight.IsEmail, userRight.FrequentlyUsed);
                                        }
                                        transaction.Commit();

                                    }
                                }
                            }
                        }
                        transaction.Commit();
                        return CommonResponse.Created("Successfully Updated");

                    }
                }


                catch (Exception ex)
                {
                    transaction.Rollback();
                   return CommonResponse.Error(ex.Message);
                }

            }
        }
        public CommonResponse DeleteUserRight(int Id)
        {
            try
            {
                string msg = null;
                var userrig=_context.UserRolePermission.Where(x=>x.Id == Id).Select(x=>x.Id).SingleOrDefault();
                if (userrig == null)
                {
                    msg = "UserRight Not Found";
                    return CommonResponse.NotFound(msg);
                }

                string criteria = "DeleteMaUserRights";
                  msg = "userRight is Delete";
                var result = _context.Database.ExecuteSqlRawAsync($"EXEC spNewEmployees @Criteria='{criteria}',@ID='{Id}'");
                return CommonResponse.Ok(msg);
            }
            catch
            {
                return CommonResponse.Error();
            }
        }

        public CommonResponse DeleteBranchdetails(int Id)
        {
            try
            {
                string msg = null;
                var branchdelt = _context.MaEmployeeBranchDet.Where(x => x.Id == Id).Select(x => x.Id).SingleOrDefault();
                if (branchdelt == null)
                {
                    msg = "Branch Details Not Found";
                    return CommonResponse.NotFound(msg);
                }
                string criteria = "DeleteMaEmployeeDetails";
                msg = "branchdetails is Deleted";
                var result = _context.Database.ExecuteSqlRawAsync($"EXEC spNewEmployees @Criteria='{criteria}',@ID='{Id}'");
                return CommonResponse.Ok(msg);
            }
            catch
            {
                return CommonResponse.Error();
            }
        }

        public CommonResponse Deleteuser(int Id)
        {
            try
            {
                string msg = null;
                var ur = _context.MaEmployeeBranchDet.Where(x => x.Id == Id).Select(x => x.Id).SingleOrDefault();
                if (ur == null)
                {
                    msg = "User Not Found";
                    return CommonResponse.NotFound(msg);
                }
                string criteria = "DeleteMaEmployees";
                msg = "user is Deleted";
                var result = _context.Database.ExecuteSqlRawAsync($"EXEC spNewEmployees @Criteria='{criteria}',@ID='{Id}'");
                return CommonResponse.Ok(msg);
            }
            catch
            {
                return CommonResponse.Error();
            }
        }
    }
}




