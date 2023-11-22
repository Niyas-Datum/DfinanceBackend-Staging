using Dfinance.Application.Dto;
using Dfinance.Application.Services.Interface;
using Dfinance.Application.Services.Interface.IGeneral;
using Dfinance.AuthApplication.Services.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Domain.Roles;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views;
using Dfinance.Core.Views.General;
using Dfinance.Shared.Domain;
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
        public UserService(DFCoreContext context, IAuthService authService, IEncryptService encryptService)
        {
            _context = context;
            _authService = authService;
            _encryptService = encryptService;

        }
        public CommonResponse FillUser()
        {
            try
            {
                var data = _context.SpFillEmployees.FromSqlRaw("Exec DropDownListSP @Criteria='FillEmployees'").ToList();

                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        //888888888888888888888888888*UserGetByID*8888888888888888888888888888888888888888888888888888888888888888

        public CommonResponse GetUserById(int Id)
        {
            try
            {
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
                throw new Exception("An error occurred while fetching user by ID", ex);
            }
        }



        //888888888888888888888888888*Add*8888888888888888888888888888888888888888888888888888888888888888
        public CommonResponse AddUser(UserDto userDto)
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
                userDto.FirstName, userDto.MiddleName, userDto.LastName, userDto.Address, userDto.EmailId, userDto.OfficeNumber, userDto.MobileNumber, userDto.DesignationId, userDto.Active, userDto.EmployeeType, userDto.Username, userDto.Password, userDto.GmailId, userDto.IsLocationRestrictedUser, userDto.PhotoId, CreatedBranchId, userDto.AccountId, userDto.ImagePath, newIdparam);

                int NewIdUser = (int)newIdparam.Value;
                //********************************************************************
                bool isMainBranchSet = false;

                foreach (var branchDetail in userDto.UserBranchDetails)
                {
                    if (branchDetail.IsMainBranch)
                    {
                        if (isMainBranchSet)
                        {
                            throw new Exception("Only one IsMainBranch is allowed per user.");
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
                throw new Exception("An error occurred while adding user and details", ex);
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
                    throw new Exception("An error occurred while adding user and details", ex);
                }

            }
        }
        public CommonResponse DeleteUserRight(int Id)
        {
            try
            {
                string criteria = "DeleteMaUserRights";
                string msg = "userRight is Suspended";
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
                string criteria = "DeleteMaEmployeeDetails";
                string msg = "branchdetails is Suspended";
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
                string criteria = "DeleteMaEmployees";
                string msg = "user is Suspended";
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




         //public CommonResponse UpdateUser(UserDto userDto, int Id)
         //{
         //    try
         //    {
         //        // Find the user by their ID, including related entities.
         //        var user = _context.MaEmployees
         //            .Include(e => e.EmployeeBranchDetails)
         //            .ThenInclude(d => d.MaUserPagePermisions)
         //            .FirstOrDefault(e => e.Id == Id);

//        if (user == null)
//        {
//            return CommonResponse.Error("User not found");
//        }





//        user.FirstName = userDto.FirstName;
//        user.MiddleName = userDto.MiddleName;
//        user.LastName = userDto.LastName;
//        user.Address = userDto.Address;
//        user.EmailId = userDto.EmailId;
//        user.ResidenceNumber = userDto.ResidenceNumber;
//        user.OfficeNumber = userDto.OfficeNumber;
//        user.MobileNumber = userDto.MobileNumber;
//        user.DesignationId = userDto.DesignationId;
//        user.Active = userDto.Active;
//        user.EmployeeType = userDto.EmployeeType;
//        user.UserName = userDto.Username;
//        user.CreatedOn = userDto.CreatedOn;

//        var Passwordtemp = _encryptService.Encrypt(userDto.Password);
//        user.Password = Passwordtemp;

//        user.GmailId = userDto.GmailId;
//        user.IsLocationRestrictedUser = userDto.IsLocationRestrictedUser;
//        user.PhotoId = userDto.PhotoId;
//        user.CreatedBranchId = _authService.GetBranchId().Value;
//        user.AccountId = userDto.AccountId;
//        user.ImagePath = userDto.ImagePath;



//        // Update or add user branch details and page permissions from the DTO.
//        foreach (var data in userDto.userbranchdetailsDto)
//        {



//             MaEmployeeDetail   userDetails = new MaEmployeeDetail
//                {
//                    //EmployeeId = Id,
//                    BranchId = data.BranchId,
//                    DepartmentId = data.DepartmentId,
//                    SupervisorId = data.SupervisorId,
//                    CreatedOn = data.CreatedOn,
//                    MaRoleId = data.MaRoleId,
//                    MaUserPagePermisions = new List<MaUserRight>()
//                };




//            //********************************************************************
//            MaUserRight maUserRight = new MaUserRight
//            {
//                UserDetailsId = userDto.UserDetailsId,
//                PageMenuId = userDto.PageMenuId,
//                IsView = userDto.IsView,
//                IsCreate = userDto.IsCreate,
//                IsEdit = userDto.IsEdit,
//                IsCancel = userDto.IsCancel,
//                IsDelete = userDto.IsDelete,
//                IsApprove = userDto.IsApprove,
//                IsEditApproved = userDto.IsEditApproved,
//                IsHigherApprove = userDto.IsHigherApprove,
//                IsPrint = userDto.IsPrint,
//                IsEmail = userDto.IsEMail,
//                FrequentlyUsed = userDto.FrequentlyUsed,
//            };
//            string criteria2 = "UpdateMaUserRights";


//            _context.Database.ExecuteSqlRaw("EXEC spNewEmployees @Criteria={0},@UserDetailsID={1},@PageMenuID={2},@IsView={3},@IsCreate={4},@IsEdit={5},@IsCancel={6},@IsDelete={7},@IsApprove={8},@IsEditApproved={9},@IsHigherApprove={10},@IsPrint={11},@IsEmail={12},@FrequentlyUsed={13}",
//                criteria2, maUserRight.UserDetailsId, maUserRight.PageMenuId, maUserRight.IsView, maUserRight.IsCreate, maUserRight.IsEdit, maUserRight.IsCancel, maUserRight.IsDelete, maUserRight.IsApprove, maUserRight.IsEditApproved, maUserRight.IsHigherApprove, maUserRight.IsPrint, maUserRight.IsEmail, maUserRight.FrequentlyUsed);

//            return CommonResponse.Created("Successfully Updated");
//        }
//    }
//    catch (Exception ex)
//    {
//        throw new Exception("An error occurred while adding user and details", ex);
//    }



