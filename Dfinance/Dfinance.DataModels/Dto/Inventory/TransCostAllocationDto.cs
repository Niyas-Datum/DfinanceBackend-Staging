using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Inventory
{
    public class TransCostAllocationDto
    {
        public int Id { get; set; }
        public int Veid { get; set; }
        public int CostCentreId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
