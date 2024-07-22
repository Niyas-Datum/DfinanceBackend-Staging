using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.PagePermission
{
    public class UserInfo
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LoginTime { get; set; }
        public int LoginStatus { get; set; }
        public int LoginSetting { get; set; }
        public int? BranchId { get; set; }
        public string Company { get; set; }
        public DateTime FinanceYearStartDate { get; set; }
        public DateTime? FinanceYearEndDate { get; set; }
        public string? NumericFormat { get; set; }
        public string? UserDepartment { get; set; }
        public string? UserRole { get; set; }
        public string? VatNo { get; set; }
        public string? MobileNumber { get; set; }
        public string? ArabicName { get; set; }
        public int? AccountID { get; set; }
        public string HOCompanyName { get; set; }
        public string HOCompanyNameArabic { get; set; }
        public DateTime? ExpDate { get; set; }
      
    }
}
