using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Item
{
    public class PriceCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public decimal SellingPrice { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
    }
}
