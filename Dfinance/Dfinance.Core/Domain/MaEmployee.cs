namespace Dfinance.Core.Domain;


/// <summary>
/// Not Completed, need to update
/// </summary>
public partial class MaEmployee
{
    //PK
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public string? Address { get; set; }
    public string EmailId { get; set; } = null!;
    public string? ResidenceNumber { get; set; }
    public string? OfficeNumber { get; set; }
    public string MobileNumber { get; set; } = null!;
    //FK
    public int? DesignationId { get; set; }
    public bool Active { get; set; }
    public int EmployeeType { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string GmailId { get; set; } = null!;
    public bool IsLocationRestrictedUser { get; set; }
    public int? PhotoId { get; set; }
    //FK
    public int? CreateBranchId { get; set; }
    //FK
    public int? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    //FK
    public int? AccoutnId { get; set; }
    public string? ImagePath { get; set; }
    //---relationship---
    public virtual FiMaAccount? Account { get; set; }


}