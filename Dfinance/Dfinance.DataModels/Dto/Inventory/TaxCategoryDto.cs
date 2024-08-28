using Dfinance.DataModels.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Inventory
{
    public class TaxCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }          // Name (MaTax)
        public string Description { get; set; }           // Note (MaTax)
        public bool Active { get; set; }                  // Active (MaTax)

        public List<DropdownDto> TaxTypeIds { get; set; }         // List of TaxTypeId (MaTaxDetails)
        public List<decimal> SalesPercentages { get; set; } // List of SalesPerc (MaTaxDetails)
        public List<decimal> PurchasePercentages { get; set; } // List of PurchasePerc (MaTaxDetails)

        public TaxCategoryDto()
        {
            TaxTypeIds = new List<DropdownDto>();
            SalesPercentages = new List<decimal>();
            PurchasePercentages = new List<decimal>();
        }
    }
}
