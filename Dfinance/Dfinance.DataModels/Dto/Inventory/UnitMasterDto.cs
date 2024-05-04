using Dfinance.Core.Views;
using Dfinance.Core.Views.Inventory;
using System.ComponentModel.DataAnnotations;

namespace Dfinance.DataModels.Dto.Inventory
{
    public class UnitMasterDto
    {
        [Required(ErrorMessage = "Unit is mandatory")]
        public string Unit { get; set; }
        public DropDownView? BasicUnit { get; set; }

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
        [Required(ErrorMessage = "AllowDelete is mandatory")]
        public bool AllowDelete { get; set; }
        //public int pageid { get; set; }
        //public int pagemethod { get; set; }
    }
}
