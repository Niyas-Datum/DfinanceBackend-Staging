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
   public  ICollection<CostCentre>? CostCentreAccount { get; set; } = new List<CostCentre>();

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
	   //relationship with itemmaster
    public ICollection<ItemMaster> Items { get; set; }
    public virtual ICollection<FiTransaction>? FiTransactions { get; set; }

    //relationship with InvTransItem
    public ICollection<InvTransItems>? InvTransItems { get; set; }
    public virtual ICollection<FiVoucherAllocation>? FiVoucherAllocations { get; set; }
    public virtual ICollection<FiTransactionEntry>? FiTransactionEntries { get; set; }
    public virtual ICollection<FimaUniqueAccount>? FimaUniqueAccounts { get; set; }
    public virtual ICollection<MaChargeType>? MaChargeTypeAccounts { get; set; }
    public virtual ICollection<MaChargeType>? MaChargeTypePayableAccounts { get; set; }

}