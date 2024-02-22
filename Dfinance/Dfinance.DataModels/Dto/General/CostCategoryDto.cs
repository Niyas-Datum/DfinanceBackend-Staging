using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.General
{
    public class CostCategoryDto
    {
        public CostCategoryDto()
        {
            AllocateRevenue = false;
            AllocateNonRevenue= false;
            Active= false;           
        }
        [Required(ErrorMessage = "Name is mandatory!!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is mandatory!!")]
        public string Description { get; set; }

        public bool? AllocateRevenue { get; set; }

        public bool? AllocateNonRevenue { get; set; }     
       
        public bool? Active { get; set; }
    }
}
