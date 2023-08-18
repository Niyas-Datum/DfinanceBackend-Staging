namespace Dfinance.AuthCore.Domain
{
    /**
   # Created On: Thursday 18 Aug 2023
   # Updated: Niyas
   **/
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ContactName { get; set; }
        public string? Company { get; set; }
        public string Email { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Mobile { get; set; }
        public string? Phone { get; set; }
        public string? BussinessType { get; set; }
        public DateTime? RequestedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<UserCompany> UserCompanies { get; set; }
        public User()
        {
            UserCompanies = new List<UserCompany>();
            Country = null!;
            Mobile = null!;
            City = null!;
            Password = null!; Username = null!;
            ContactName = null!;
            Email = null!;
        }
    }
}