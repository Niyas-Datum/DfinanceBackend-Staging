namespace Dfinance.Core.Domain;


/// <summary>
/// need to update
/// </summary>
public partial class FiMaAccount
{

    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; } = null!;
    public string Alias { get; set; }
    public string? Narration { get; set; }
    public int? AccountTypeId { get; set; }
    public bool? IsBillWise { get; set; }
    public bool Active { get; set; }
    public int? CreatedBranchId { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    //FK
    public int? Parent { get; set; }
    public bool IsGroup { get; set; }
    public int? Level { get; set; }
    public bool FinalAccount { get; set; }
    public int? SortField { get; set; }
    public int AccountGroup { get; set; }
    public int? SubGroup { get; set; }
    public bool SystemAccount { get; set; }
    //FK
    public int? AccountCategory { get; set; }
    public int? GroupOrder { get; set; }
    public bool? PreventExtraPay { get; set; }
    public bool? IsItemwise { get; set; }
    public bool? IsCollection { get; set; }
    public bool? IsCostCentre { get; set; }
    public bool? ValueOfGoods { get; set; }
    public string? AlternateName { get; set; }

    //Relationships
   // public  ICollection<CostCentre>? CostCentreClientAccount { get; set; } = new List<CostCentre>();

    // public  ICollection<CostCentre>? CostCentreSupplierAccount { get; set; } = new List<CostCentre>();
    //selfReference
    public virtual FiMaAccountCategory AccountCategoryNavigation { get; set; }
    public virtual FiMaAccount ParentNavigation { get; set; }
    public virtual ICollection<FiMaAccount> InverseParentNavigation { get; set; }

    public virtual ICollection<Voucher>? FiMaVoucherCashAccounts { get; set; }
    public virtual ICollection<Voucher>? FiMaBankAcount {  get; set; }
    public virtual ICollection<Voucher>? FiMaCardAccount {  get; set; }
    public virtual ICollection<Voucher>? FiMaPostAccount { get; set; }

    public Parties Parties { get; set; }

}