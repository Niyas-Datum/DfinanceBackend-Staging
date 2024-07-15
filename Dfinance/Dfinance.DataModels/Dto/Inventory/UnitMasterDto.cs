using Dfinance.DataModels.Dto.Common;
using System.ComponentModel.DataAnnotations;

namespace Dfinance.DataModels.Dto.Inventory
{
    public class UnitMasterDto
    {
        [Required(ErrorMessage = "Unit is mandatory")]
        public string Unit { get; set; }
        public DropdownDto? BasicUnit { get; set; }

        [Required(ErrorMessage = "Precision is mandatory")]
        public int Precision { get; set; }
        [Required(ErrorMessage = "Description is mandatory")]
        public string Description { get; set; }
        public string? ArabicName { get; set; }
        public bool? Active { get; set; }

        [Required(ErrorMessage = "Factor is mandatory")]
        public decimal Factor { get; set; }
        [Required(ErrorMessage = "IsComplex is mandatory")]
        public bool IsComplex { get; set; }
        
    }
}
