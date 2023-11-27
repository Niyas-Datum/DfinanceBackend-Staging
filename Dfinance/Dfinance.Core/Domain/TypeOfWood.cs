using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class TypeOfWood
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal? AvgStockQuantity { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte ActiveFlag { get; set; }
        public int CreatedBranchId { get; set; }
        public string Code { get; set; }

    }
}
