using System.ComponentModel.DataAnnotations;

namespace Dfinance.DataModels.Dto
{
    public class DepartmentTypeDto
    {
        public int Id { get; set; } = 0;
        public int DepId { get; set; }
        [Required(ErrorMessage = "Department is mandatory!!")]
        public string Department {  get; set; }
        public List<BranchDropdownDto> Branch { get; set; } = null!;


    }
}
