namespace Dfinance.AuthCore.Domain
{
    /**
   # Created On: Thursday 18 Aug 2023
   # Not use in this project
   # Updated: Niyas
   **/
    public partial class Company
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string? ServerName { get; set; }
        public string ServerIp { get; set; } = null!;
        public bool IsRemote { get; set; }
        public bool? Active { get; set; }
        public bool? IsDefault { get; set; }
        public virtual ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
        public virtual ICollection<CompanySchedule> CompanySchedules { get; set; } = new List<CompanySchedule>();


    }
}