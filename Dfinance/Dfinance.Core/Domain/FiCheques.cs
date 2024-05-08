namespace Dfinance.Core.Domain
{
    public class FiCheques
    {
        public int Id { get; set; }
        public int Veid { get; set; }
        public string? CardType { get; set; }
        public decimal? Commission { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public int? ClrDays { get; set; }
        public int? BankId { get; set; }
        public string? BankName { get; set; }
        public string? Status { get; set; }
        public int? PartyId { get; set; }

        //public virtual FiMaAccount? Party { get; set; }
        //public virtual FiTransactionEntry Ve { get; set; } = null!;

    }
}
