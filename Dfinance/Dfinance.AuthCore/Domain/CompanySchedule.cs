namespace Dfinance.AuthCore.Domain
{
    /**
   # Created On: Thursday 18 Aug 2023
   # Not use in this project
   # Updated: Niyas
   **/
    public partial class CompanySchedule
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }
        public virtual Company Company { get; set; } = null!;
    }
}