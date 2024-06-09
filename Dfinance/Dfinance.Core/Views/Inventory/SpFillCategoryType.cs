using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Inventory
{
   

    public class SpFillCategoryTypeById
    {
        public int ID { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int? CreatedBy {  get; set; }
        public DateTime? CreatedOn { get; set;}
        public byte? ActiveFlag { get; set; }
        public decimal? AvgStockQuantity {  get; set; }
    }

    public class NextCodeCat
    {
        public long Code { get; set; }
    }

}
