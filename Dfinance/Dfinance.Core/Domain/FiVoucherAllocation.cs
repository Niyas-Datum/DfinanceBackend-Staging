

using Dfinance.Core.Domain;

namespace Dfinance.Core
{
    public  class FiVoucherAllocation
    {
        public int Id { get; set; }
        public int? Vid { get; set; }
        public int? Veid { get; set; }
        public int? AccountId { get; set; }
        public decimal? Amount { get; set; }
        public int? RefTransId { get; set; }

        public virtual FiMaAccount? Account { get; set; }
        public virtual FiTransaction? RefTrans { get; set; }
        public virtual FiTransactionEntry? Ve { get; set; }
        public virtual FiTransaction? VidNavigation { get; set; }
        public virtual ICollection<TransCollnAllocation> TransCollnAllocations { get; set; }
    }
}
