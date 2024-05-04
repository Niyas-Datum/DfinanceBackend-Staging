namespace Dfinance.Core.Views.General
{
    public class SpFillRoles
    {
        public FillRole? fillRole { get; set; }
        public List<FillRoleRight> fillRoleRights { get; set; }

       
    }

    public class FillRole
    {
        public int Id { get; set; }
        public string Role { get; set; } = null;
        public DateTime? CreatedOn { get; set; }
        public bool Active { get; set; }
       // public int? CreatedBranchId { get; set; }
       
      
    }

    public class FillRoleRight
    {
       // public int? Id { get; set; }
        public int? RoleId { get; set; }
        public string? PageName { get; set; }
        public string? ModuleType { get; set; }
        public int? PageMenuId { get; set; }
        public bool? IsView { get; set; }
        public bool? IsCreate { get; set; }
        public bool? IsEdit { get; set; }
        public bool? IsCancel { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsApprove { get; set; }
        public bool? IsEditApproved { get; set; }
        public bool? IsHigherApprove { get; set; }
        public bool? IsPrint { get; set; }

    }

   


}
