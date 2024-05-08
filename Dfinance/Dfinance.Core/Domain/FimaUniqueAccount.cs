namespace Dfinance.Core.Domain
{
    public class FimaUniqueAccount
    {
        public string Keyword { get; set; } = null!;
        public int? AccId { get; set; }

        public virtual FiMaAccount? Acc { get; set; }
    }
}
