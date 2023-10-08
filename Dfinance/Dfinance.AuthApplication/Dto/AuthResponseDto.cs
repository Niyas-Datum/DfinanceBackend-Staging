using Dfinance.Core.Domain;

namespace Dfinance.AuthAppllication.Dto;

public class AuthResponseDto
{
    public AuthResponseDto()
    {
        
    }
    public int Id { get; set; }
    public string? Username { get; set; } 
    public string? Password { get; set; }


    public string Token { get; set; } = null!;
    public BranchDto? BranchDet { get; set; } = null!;
    public CompanyDto? CompanyDet { get; set; } = null!;
    public DateTime? FinYearStartDate { get; set; }
    public DateTime? FinYearEndDate { get; set; }
    public decimal? NumericFormat { get; set; }
    public UserRoleDto? UserRole { get; set; }
    public UserDepartmentDto? UserDepartment { get; set; }
    public string?  HOCompanyName { get; set; }
    public string? CentralTaxNo { get; set; }
    public string? SalesTaxNo { get; set; }

    public string? DepartmentName { get; set; }
    public string? RoleName { get; set; }
    public EmployeeBranchDetDto EmployeeBranchDetDto { get; set; }

    public MaUserRightsDto UserRightsDto { get; set; }

    public AuthResponseDto(AuthResponseDto user, string token)
    {
        this.Username = user.Username;
        this.Id = user.Id;
        this.Token = token;


    }
}

public class MaUserRightsDto
{
    public int Id { get; set; }

    //this id is connecto employee-details table
    public int UserDetailsId { get; set; }

    //we are setting page permission 
    public int PageMenuId { get; set; }

    public bool IsView { get; set; }

    public bool IsCreate { get; set; }

    public bool IsEdit { get; set; }

    public bool IsCancel { get; set; }

    public bool IsDelete { get; set; }

    public bool IsApprove { get; set; }

    public bool IsEditApproved { get; set; }

    public bool IsHigherApprove { get; set; }

    public bool IsPrint { get; set; }

    public bool? IsEmail { get; set; }

    public bool? FrequentlyUsed { get; set; }

}
public partial class EmployeeBranchDetDto
{
    public string? DepartmentName { get; set; }
    public int? MaRoleId { get; set; }
    public string? RoleName { get; set; }
    public int? DepartmentId { get; set; }
    public DepartmentDto? DepartmentDto { get; set; }
    public IList<MaUserRightsDto> MaUserRightsViewDto { get; set; }    

}
public partial class DepartmentDto
{
    public int Id { get; set; }

    public int DepartmentTypeId { get; set; }
    public DepartmentTypeDto DepartmentType { get; set; }
}
public partial class DepartmentTypeDto
{
    public int Id { get; set; }
    public string Department { get; set; }
}
public partial class BranchDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? ArabicName { get; set; }
    public string? MobileNumber { get; set; }
    public string? EmailAddress { get; set; }
    public string? HOCompanyName { get; set; }
    public string? HOCompanyArName { get; set; }
    public int MyProperty { get; set; }

}
public partial class CompanyDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
   

}
public partial class UserRoleDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
}

public partial class UserDepartmentDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
}