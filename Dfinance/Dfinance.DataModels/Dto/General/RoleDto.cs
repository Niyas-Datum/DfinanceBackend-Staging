using System.ComponentModel.DataAnnotations;

namespace Dfinance.DataModels.Dto.General
{
    public class RoleDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Role is mandatory")]
        public string Role { get; set; }
        public bool Active { get; set; }
        public List<RolerightDto> RolerightDto { get; set; }
    }
    public class RolerightDto
    {
        public int? RoleId { get; set; }
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

        public bool? IsEmail { get; set; }
    }

}
