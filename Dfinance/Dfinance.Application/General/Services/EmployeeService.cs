using Dfinance.Application.Dto;
using Dfinance.Application.Services.Interface.IGeneral;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views;
using Dfinance.Core.Views.General;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dfinance.Application.Services.General
{
    public class EmployeeService : IEmployeeService

    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public EmployeeService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public CommonResponse FillEmployees()
        {
            try
            {
                var data = _context.SpFillEmployees.FromSqlRaw("Exec DropDownListSP @Criteria='FillEmployees'").ToList();

                var employees = data.Select(i => new SpFillEmployees
                {
                    ID = i.ID,
                    Name = i.Name
                }).ToList();
                return CommonResponse.Ok(employees);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
        //public CommonResponse AddEmployee(MaEmployeeDetailsDto maEmployeeDetailsDto)
        //{
        //    try
        //    {
        //        maEmployeeDetailsDto.CreatedBy = _authService.GetId().Value;
        //        MaEmployee employee = new MaEmployee
        //        {
        //            FirstName = maEmployeeDetailsDto.FirstName,
        //            MiddleName = maEmployeeDetailsDto.MiddleName,
        //            LastName = maEmployeeDetailsDto.LastName,
        //            Address = maEmployeeDetailsDto.Address,
        //            EmailId = maEmployeeDetailsDto.EmailID,
        //            ResidenceNumber = maEmployeeDetailsDto.ResidenceNumber,
        //            OfficeNumber = maEmployeeDetailsDto.OfficeNumber,
        //            MobileNumber = maEmployeeDetailsDto.MobileNumber,
        //            DesignationId = maEmployeeDetailsDto.DesignationID,
        //            EmployeeType = maEmployeeDetailsDto.EmployeeType,
        //            UserName = maEmployeeDetailsDto.UserName,
        //            Password = maEmployeeDetailsDto.Password,
        //            GmailId = maEmployeeDetailsDto.GmailID,
        //            IsLocationRestrictedUser = maEmployeeDetailsDto.IsLocationRestrictedUser,
        //            CreatedBranchId = maEmployeeDetailsDto.CreatedBranchID,
        //            CreatedBy = maEmployeeDetailsDto.CreatedBy,
        //            CreatedOn = maEmployeeDetailsDto.CreatedOn,
        //        };
        //        var result = _context.SpMaEmployeesC.FromSqlRaw($"EXEC spEmployees @Mode ='{1}',@FirstName='{employee.FirstName}',@MiddleName='{employee.MiddleName}',@LastName='{employee.LastName}',@Address='{employee.Address}',@EmailID='{employee.EmailId}',@OfficeNumber='{employee.OfficeNumber}',@MobileNumber='{employee.MobileNumber}',@DesignationID='{1}',@EmployeeType='{employee.EmployeeType}',@UserName='{employee.UserName}',@Password='{employee.Password}',@GmailID='{employee.GmailId}',@IsLocationRestrictedUser='{employee.IsLocationRestrictedUser}',@CreatedBranchId='{1}',@CreatedBy='{employee.CreatedBy}',@CreatedOn='{employee.CreatedOn}'").ToList();
        //        var Id = result.Select(i => new SpMaEmployeesC
        //        {
        //            ID = i.ID
        //        }).ToList();
        //        return CommonResponse.Created(Id);
        //    }

        //    catch (Exception ex)
        //    {
        //        throw new Exception();
        //    }
        //}
    }
}
