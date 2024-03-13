using Dfinance.Core.Views.Common;
using System.ComponentModel.DataAnnotations;
namespace Dfinance.DataModels.Dto
{
    public class WarehouseDto
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Type is mandatory!!")]
        public DropDownViewName Type { get; set; }
        [Required(ErrorMessage = "Code is mandatory!!")]
        public string? Code { get; set; }
        [Required(ErrorMessage = "Name is mandatory!!")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Address is mandatory!!")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Remarks is mandatory!!")]
        public string? Remarks { get; set; }
        public decimal? Active { get; set; }
        public decimal? IsDefault { get; set; }
        public decimal? ClearingChargePerCFT { get; set; }
        public decimal? GroundRentPerCFT { get; set; }
        public decimal? LottingPerPiece { get; set; }
        public decimal? LorryHirePerCFT { get; set; }
    }
}
