using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Inventory
{
    public class DosageDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Dosage is mandatory!!")]
        public string? Dosage { get; set; }
        public string? Remarks { get; set; }
        public bool? Active { get; set; }
    }
}
