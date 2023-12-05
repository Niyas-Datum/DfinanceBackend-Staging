using System.ComponentModel.DataAnnotations;

public class UserDto
{
    [Required(ErrorMessage = "First Name is required")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name should not contain special characters")]
    public string FirstName { get; set; }

    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Middle Name should not contain special characters")]
    public string MiddleName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name should not contain special characters")]
    public string LastName { get; set; }

    public string Address { get; set; }
    [Required(ErrorMessage = "Email ID is required")]
    [EmailAddress(ErrorMessage = "Invalid Email ID")]
    public string EmailId { get; set; }
    public string? ResidenceNumber { get; set; }
    public string OfficeNumber { get; set; }
    [Required(ErrorMessage = "Mobile Number is required")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Mobile Number should contain only numbers")]
    public string MobileNumber { get; set; }
    [Required(ErrorMessage = "Designation is required")]
    public int DesignationId { get; set; }
    public bool Active { get; set; }
    [Required(ErrorMessage = "Employee Type is required")]
    public int EmployeeType { get; set; }

    [Required(ErrorMessage = "Username is required")]
    //[UniqueUsername(ErrorMessage = "Username must be unique")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    [Required(ErrorMessage = "Gmail ID is required")]
    [EmailAddress(ErrorMessage = "Invalid Gmail ID")]
    public string GmailId { get; set; }

   

    public bool IsLocationRestrictedUser { get; set; }
    public int? PhotoId { get; set; }
    
    public int? AccountId { get; set; }
    public string? ImagePath { get; set; }
    public List<UserBranchDetailsDto> UserBranchDetails { get; set; }
}

public class UserBranchDetailsDto
{
    public int EmployeeId { get; set; }
   
    public int DepartmentId { get; set; }
    public DateTime CreatedOn { get; set; }
    public byte ActiveFlag { get; set; }
    public bool IsMainBranch { get; set; }
    public int BranchName { get; set; }
    public int? SupervisorId { get; set; }
    public int? MaRoleId { get; set; }
    public List<MapagemenuDto> MapagemenuDto { get; set; }
}

public class MapagemenuDto
{


    public int UserDetailsId { get; set; }
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
