namespace Dfinance.Core.Views.Finance
{
    public  class FinanceYearView
    {
        public int FinYearID {  get; set; } 
        public string FinYearCode {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.MinValue;
        public string Status {  get; set; } 
    }
    public class FinanceYearViewByID
    {
        public int FinYearID { get; set; }
        public string FinYearCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public DateTime LockTillDate { get; set; }
        public int CreatedBranchID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte ActiveFlag {  get; set; }

    }
}
