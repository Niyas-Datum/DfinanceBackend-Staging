namespace Dfinance.Core.Domain
{
    public partial class TransReference
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int RefTransId { get; set; }
        public string? Description { get; set; }

        public virtual FiTransaction RefTrans { get; set; } = null!;
        public virtual FiTransaction Transaction { get; set; } = null!;
    }
}
