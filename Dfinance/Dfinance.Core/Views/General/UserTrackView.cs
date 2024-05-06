namespace Dfinance.Core.Views.General
{
    
    public class UserTrackView
    {
        public long ID { get; set; }
        public int? UserID { get; set; }
        public string? UserName { get; set; }
        public string? TableName { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? Reason { get; set; }
        public string? Action { get; set; }
        public long? RowID { get; set; }
        public string? MachineName { get; set; }
        public string? ModuleName { get; set; }
        public string? Reference { get; set; }
        public decimal? Amount { get; set; }
        public string? Details { get; set; }
    }

}
