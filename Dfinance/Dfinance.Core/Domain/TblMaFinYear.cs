namespace Dfinance.Core.Domain
{
    public partial class TblMaFinYear
    {
        public int FinYearId { get; set; }

        public string FinYearCode { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? LockTillDate { get; set; }

        public string Status { get; set; } = null!;

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public byte ActiveFlag { get; set; }

        public int CreatedBranchId { get; set; }

        public virtual MaCompany CreatedBranch { get; set; } = null!;

        public virtual MaEmployee CreatedByNavigation { get; set; } = null!;

        public virtual ICollection<FiTransaction> FiTransactions { get; set; } = new List<FiTransaction>();

        //public virtual ICollection<TblMaFinPeriod> TblMaFinPeriods { get; set; } = new List<TblMaFinPeriod>();
    }
}
