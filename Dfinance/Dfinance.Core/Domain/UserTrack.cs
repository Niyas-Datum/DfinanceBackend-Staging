namespace Dfinance.Core.Domain
{
    public class UserTrack
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public string TableName { get; set; } = null!;
        public DateTime ActionDate { get; set; }
        public string? Reason { get; set; }
        public int? ActionId { get; set; }
        public long? RowId { get; set; }
        public string? MachineName { get; set; }
        public string? ModuleName { get; set; }
        public string? Reference { get; set; }
        public decimal? Amount { get; set; }
    }
}
