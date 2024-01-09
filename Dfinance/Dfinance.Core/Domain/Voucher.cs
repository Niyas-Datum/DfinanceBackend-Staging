namespace Dfinance.Core.Domain
{
    public partial class Voucher
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public int? PrimaryVoucherId { get; set; }
        public string? Type { get; set; }
        public bool? Active { get; set; }
        public string? Code { get; set; }
        public byte? DevCode { get; set; }
        public int CreatedBranchId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set;}
        public int? DocumentType { get; set; }
        public int? Numbering { get; set; }
        public bool? FinanceUpdate { get; set; }
        public bool? RateUpdate { get; set;}
        public int? RowType { get; set; }
        public bool? ApprovalRequired { get; set; }
        public int? WorkflowDays { get; set; }
        public int? ApprovalDays { get; set; }
        public byte? ModuleType { get; set; }
        public bool? IsLocInv { get; set; }
        public string? Nature { get; set; }
        public string? ReportPath { get; set; }
        public int? CashAccountId { get; set; }
        public int? CardAccountId { get; set; }
        public int? PostAccountId { get; set;}
        public int? BankAccountId { get; set;}
        public int? ItemUpdate { get; set; }
        public bool? IsGroupItemEntry { get; set; }
        public bool? IsGroupItemReport { get; set;}
        public int? FormId { get;set; }

        public virtual FiMaAccount? CashAccount { get; set; }

        public virtual MaCompany CreatedBranch { get; set; } = null!;

        public virtual MaEmployee CreatedByNavigation { get; set; } = null!;

        public virtual FiMaAccount?  BankAccount { get; set; } 
        public virtual FiMaAccount? CardAccount { get; set; }
        public virtual FiMaAccount? PostAccount { get; set; }
        public virtual MaNumbering? NumberingNavigation { get; set; }
        public virtual TaxFormMaster? Form { get; set; }

    }
}
