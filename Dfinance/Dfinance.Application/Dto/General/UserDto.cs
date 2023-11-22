public class UserDto
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string EmailId { get; set; }
    public string ResidenceNumber { get; set; }
    public string OfficeNumber { get; set; }
    public string MobileNumber { get; set; }
    public int DesignationId { get; set; }
    public bool Active { get; set; }
    public int EmployeeType { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string GmailId { get; set; }
    public bool IsLocationRestrictedUser { get; set; }
    public int PhotoId { get; set; }
    
    public int AccountId { get; set; }
    public string ImagePath { get; set; }
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
