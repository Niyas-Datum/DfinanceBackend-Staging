using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Domain
{
    public class Categories
    {
        public int Id { get; set; }
        public string Description { get; set; }    
        public string CategoryCode { get; set; }//Code
        public int? CategoryTypeId { get; set; }//TypeofWoodId

        [StringLength(1)]
        public string? Category {  get; set; }
        public int CreatedBy {  get; set; }
        public DateTime CreatedOn { get; set; }
        public byte ActiveFlag { get; set; }
        public int? CreatedBranchId { get; set; }

        [StringLength(1)]
        public string? StockType { get; set; }
        public decimal? MinQty { get; set; }
        public decimal? MaxQty { get; set; }
        public decimal? FloorRate { get; set; }
        public byte? MinusStock {  get; set; }
        public DateTime? StartDate {  get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Discount { get; set; }

        //relationships
        public ICollection<ItemMaster> Items { get; set; }
        



    }
}
