using System.ComponentModel.DataAnnotations;

namespace Dfinance.Application.Dto.Finance
{
    public class FinanceYearDto
    {
        [Required(ErrorMessage = "FinanceYear is required")]
        public string FinanceYear { get; set; }

        [Required(ErrorMessage = "Startdate is required")]
        public DateTime Startdate { get; set; }

        [Required(ErrorMessage = "Enddate is required")]
        public DateTime Enddate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "LockTillDate is required")]
        public DateTime LockTillDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }
    }
}
