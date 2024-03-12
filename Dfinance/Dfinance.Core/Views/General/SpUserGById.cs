using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.General
{
    public class SpUserGById
    {
        public FillUserSpView? UserDetails { get; set; }
        public List<UserBranchDetails>? BranchDetails { get; set; }
        public List<UserRights>? UserRights { get; set; }
        public List<LocationRestriction>? LocationRestrictions { get; set; }
    }

        public class FillUserSpView
        {

            public int ID { get; set; }
            public string? FirstName { get; set; }
            public string? MiddleName { get; set; }
            public string? LastName { get; set; }
            public string? Address { get; set; }
            public string? EmailID { get; set; }
            public string? ResidenceNumber { get; set; }
            public string? OfficeNumber { get; set; }
            public string? MobileNumber { get; set; }
            public int? DesignationID { get; set; }
            public bool? Active { get; set; }
            public int? EmployeeType { get; set; }
            public string? UserName { get; set; }
            public string? Password { get; set; }
            public string? GmailID { get; set; }
            public bool? IsLocationRestrictedUser { get; set; }
            public int? PhotoID { get; set; }
            public int? CreatedBranchID { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? AccountID { get; set; }
            public string? AccountCode { get; set; }
            public string? AccountName { get; set; }
            public string? ImagePath { get; set; }



        }

        public class UserBranchDetails
        {
            public int ID { get; set; }
            public int EmployeeID { get; set; }
            public int BranchID { get; set; }
            public string? BranchName { get; set; }
            public string? EmployeeName { get; set; }
            public string? DepartmentName { get; set; }
            public int? DepartmentID { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? CreatedOn { get; set; }
            public bool? ActiveFlag { get; set; }
            public bool? IsMainBranch { get; set; }
            public int? SupervisorID { get; set; }
            public int? MaRoleID { get; set; }

        }
        public class UserRights
        {
            public int ID { get; set; }
            public int UserDetailsID { get; set; }
            public int PageMenuID { get; set; }
            public string? PageName { get; set; }
            public string? ModuleType { get; set; }
            public bool? IsView { get; set; }
            public bool? IsCreate { get; set; }
            public bool? IsEdit { get; set; }
            public bool? IsCancel { get; set; }
            public bool? IsDelete { get; set; }
            public bool? IsApprove { get; set; }
            public bool? IsEditApproved { get; set; }
            public bool? IsHigherApprove { get; set; }
            public bool? IsPrint { get; set; }
            public bool? IsEmail { get; set; }
            public bool? FrequentlyUsed { get; set; }
            public bool? IsPage { get; set; }

        }
        public class LocationRestriction
        {
            public int ID { get; set; }
            public int EmployeeID { get; set; }
            public int LocationID { get; set; }
        }
        public class SpUser
        {

            public int ID { get; set; }
            public string? FullName { get; set; }
            public string? Role { get; set; }
            public string? ImagePath { get; set; }
            public bool? Active { get; set; }

        }
        public class RoleRightsModel
        {
            public int PageMenuID { get; set; }
            public bool IsPage { get; set; }
            public string PageName { get; set; }
            public string ModuleType { get; set; }
            public bool IsCreate { get; set; }
            public bool IsView { get; set; }
            public bool IsCancel { get; set; }
            public bool IsDelete { get; set; }
            public bool IsEditApproved { get; set; }
            public bool IsEdit { get; set; }
            public bool IsApprove { get; set; }
            public bool IsHigherApprove { get; set; }
            public bool IsPrint { get; set; }
        }
        public class GetRole
        {
            public int ID { get; set; }
            public string Role { get; set; }
        }
    }