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
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string GmailId { get; set; } = null!;
    public bool IsLocationRestrictedUser { get; set; }
    public int? PhotoId { get; set; }
    //FK
    public int? CreatedBranchId { get; set; }
    //FK
    public int? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    //FK
    public int? AccountId { get; set; }
    public string? ImagePath { get; set; }


    /// <summary>
    /// newly configure 
    /// currently using
    /// </summary>

    

    //employee branch details
    public virtual ICollection<MaEmployeeDetail> EmployeeBranchDetails { get; set; } = new List<MaEmployeeDetail>();
    //used for who created emplyee branch details
    public virtual ICollection<MaEmployeeDetail> EmployeeBranchDetCreatedUserList { get; set; } = new List<MaEmployeeDetail>();


    /// end use
    //---relationship ma Account one =>---
    public virtual FiMaAccount? Account { get; set; }

    public virtual MaCompany CreatedBranchCompany { get; set; }
    // relationship with ma company  one  to many 
    public virtual ICollection<MaCompany>? MaCompanyContactPeople { get; set; }

    //relationship with ma-company  one to many 
    public virtual ICollection<MaCompany> MaCompanyCreatedByConnection { get; set; } = new List<MaCompany>();

    //relationship with CostCategory
    public virtual ICollection<CostCategory> CostCategoryCreatedBy { get; set; }=new List<CostCategory>();
    
    //relationship with MaArea
    public virtual ICollection<MaArea> AreaCreatedBy { get; set; }
    public ICollection<LogInfo> LogInfos { get; set; }
    //relationship with currency
    public virtual ICollection<Currency> Currencies { get; set; }

    //RELATIONSHIP WITH FINANCIAL YEAR
    public ICollection<TblMaFinYear> TblMaFinYears { get; set; }

    public virtual ICollection<Voucher>? FiMaVouchers { get; set; }
}