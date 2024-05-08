using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class MaChargeType
    {
        public int Id { get; set; }
        public string ChargeType { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int DueDays { get; set; }
        public string Mode { get; set; } = null!;
        public int? PayableAccountId { get; set; }
        public int? AccountId { get; set; }
        public string CostingType { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte ActiveFlag { get; set; }
        public bool IsLcrelatedExp { get; set; }
        public string? DueDaysRemarks { get; set; }
        public int? CreatedBranchId { get; set; }
        public int? DevCode { get; set; }
        public bool IsPurchaseRelatedExp { get; set; }
        public bool? IsSalesExp { get; set; }

        public virtual FiMaAccount? Account { get; set; }
        public virtual MaCompany? CreatedBranch { get; set; }
        public virtual MaEmployee CreatedByNavigation { get; set; } = null!;
       
        public virtual FiMaAccount? PayableAccount { get; set; }
        public virtual ICollection<TransExpense> TransExpenses { get; set; }
       // public virtual ICollection<TransItemExpense> TransItemExpenses { get; set; }
    }
}
