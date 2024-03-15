using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public partial class CostCentre
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public string PType { get; set; }
        public string? Type {  get; set; }
        public string? SerialNo { get; set; }
        public string? RegNo { get; set; }
        public int? SupplierId { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
        public decimal? Rate { get; set; }
        public DateTime? SDate { get; set; }
        public string? Make { get; set; }
        public string? MYear { get; set; }
        public DateTime? EDate { get; set; }
        public decimal? ContractValue { get; set; }
        public decimal? InvoicedAmt { get; set; }
        public int? ClientId { get; set; }
        public int? StaffId { get; set; }
        public bool? IsPaid { get; set; }
        public int? StaffId1 { get; set; }
        public string? Site { get; set; }
        public bool? IsGroup { get; set; }
        public int? CostCategoryId { get; set; }
        public int? ParentId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? CreatedBranchId { get; set; }

        //Relationships

        // public virtual FiMaAccount FimaAccountClient { get; set; }
        // public virtual FiMaAccount FiMaAccountSupplier {  get; set; }
        public virtual ICollection<FiTransaction> FiTransactions { get; set; }
        
    }
}
