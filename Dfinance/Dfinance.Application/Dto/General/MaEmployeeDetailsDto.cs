using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Dto
{
    public class MaEmployeeDetailsDto
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string EmailID { get; set; }
        public string ResidenceNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string MobileNumber { get; set; }
        public int DesignationID { get; set; }
        public int EmployeeType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string GmailID { get; set; }
        public bool IsLocationRestrictedUser { get; set; }
        public int CreatedBranchID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
       
    }

}
