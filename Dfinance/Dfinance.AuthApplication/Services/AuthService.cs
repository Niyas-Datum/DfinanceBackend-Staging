using Dfinance.Application.Services.Interface;
using Dfinance.AuthAppllication.Authorization;
using Dfinance.AuthAppllication.Dto;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.AuthCore.Infrastructure;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Dfinance.AuthAppllication.Services;

public class AuthService : IAuthService
{
    private readonly DFCoreContext _dfCoreContext;
    private static AuthResponseDto? _User = null!;
    private readonly IEncryptService _encryptService;
    private readonly IJwtSecret _jwtsecret;
    public AuthService(IJwtSecret jwtSecret, DFCoreContext dFCoreContext,IEncryptService encryptService)
    {
        _jwtsecret = jwtSecret;
        _dfCoreContext = dFCoreContext;
        _encryptService = encryptService;
    }
    public CommonResponse Authenticate(AuthenticateRequestDto model)
    {
        try
        {
            _User = _dfCoreContext.MaEmployees
                .Include(x=> x.CreatedBranchCompany)
                .Include("EmployeeBranchDetails.MaRole")
                .Include("EmployeeBranchDetails.MaUserPagePermisions")

                .Include(x=> x.EmployeeBranchDetails)

                .ThenInclude(x =>  x.Department).ThenInclude(x => x.DepartmentType )
                .Select(x=> new AuthResponseDto()
                {
                    Id = x.Id,
                    Username = x.UserName,
                    Password = x.Password,
                    
                    EmployeeBranchDetDto = x.EmployeeBranchDetails.Where(x=>x.BranchId== model.BranchId)
                                            .Select(x=> new EmployeeBranchDetDto
                                            {
                                                DepartmentName  = x.Department.DepartmentType.Department.Trim(),
                                                DepartmentId = x.DepartmentId,
                                                RoleName = x.MaRole.Role,
                                                MaUserRightsViewDto = x.MaUserPagePermisions.Select(x=>
                                                new MaUserRightsDto()
                                                {
                                                    UserDetailsId=x.UserDetailsId,
                                                    PageMenuId = x.PageMenuId,
                                                    IsApprove = x.IsApprove,
                                                    IsView=x.IsView,
                                                    IsCreate=x.IsCreate,
                                                    IsDelete=x.IsDelete,
                                                    IsEdit=x.IsEdit,
                                                    IsCancel=x.IsCancel,
                                                    IsEditApproved=x.IsEditApproved,
                                                    IsEmail=x.IsEmail,
                                                    IsPrint=x.IsPrint,
                                                    IsHigherApprove=x.IsHigherApprove,
                                                    FrequentlyUsed=x.FrequentlyUsed,

                                                } ).ToList()

                                            }).FirstOrDefault(),
                    //UserRightsDto= x.EmployeeBranchDetails.Where(x => x.BranchId == model.BranchId)
                    //                .Select(x=> new MaUserRightsDto()
                    //                {
                    //                    PageMenuId = x.MaUserPagePermisions.Select(x=> new )
                    //                }).FirstOrDefault(),

                  //  DepartmentName = x.EmployeeBranchDetails.,
                    CompanyDet = new CompanyDto { Id = model.CompanyId, Name= model.CompanyName },
                    BranchDet = new BranchDto { Id = x.CreatedBranchId, Name = x.CreatedBranchCompany.Company,
                                               ArabicName = x.CreatedBranchCompany.ArabicName,
                                                EmailAddress = x.CreatedBranchCompany.EmailAddress,
                                                HOCompanyArName = x.CreatedBranchCompany.HocompanyName,
                                                HOCompanyName = x.CreatedBranchCompany.HocompanyNameArabic,
                                                 MobileNumber = x.MobileNumber},
                   // FinYearEndDate = x.CreatedBranchCompany.
                   SalesTaxNo = x.CreatedBranchCompany.SalesTaxNo,
                   CentralTaxNo = x.CreatedBranchCompany.CentralSalesTaxNo,
                }).FirstOrDefault(u => u.Username == model.Username);

        if(_User == null)
        return CommonResponse.Error("User Account not found");

            string encryptedInputPassword = _encryptService.Encrypt(model.Password);
            if (_User.Password != encryptedInputPassword)
                return CommonResponse.Error("Invalid password");

        //var user = _users.SingleOrDefault(x => x.UserName == model.Username && x.Password == model.Password);
      
            _User.Token = _jwtsecret.GenerateJwtToken(_User);

            return CommonResponse.Ok(_User);
        }catch(Exception ex) {
            return CommonResponse.Error();
        }
    }

    public AuthResponseDto GetUserById(int? id)
    {
        if (_User == null) return null!;
        if (_User.Id == id) return _User;

        return null!;

    }

    public int? GetId()
    {
        return _User.Id;
    }

    public string GetUserName()
    {
        return _User.Username;
    }
    public int? GetBranchId()
    {
        return _User?.BranchDet?.Id;
    }
  

}
