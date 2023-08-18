namespace Dfinance.AuthCore.Domain
{

    /**
    # Created On: Thursday 18 Aug 2023
    # Updated: Niyas
    **/
    public partial class UserCompany
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }

        public virtual Company Company { get; set; }
        public virtual User User { get; set; }
        public UserCompany()
        {
            Company = null!;
            User = null!;
        }
    }


}