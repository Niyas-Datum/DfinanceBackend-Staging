using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Inventory
{
    public class SpFillCategoryByIdG
    {
        public int Id { get; set; }
        public string Description {  get; set; }
        public int? TypeofWoodId {  get; set; }
        public string? TypeCode {  get; set; }
        public string? TypeName { get; set; }        
        public string? Category { get; set; }
        public string? CategoryName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte ActiveFlag { get; set; }
        public string Code { get; set; }
        public string? MeasurementType { get; set; }
        public decimal? MinQty { get; set;}
        public decimal? MaxQty { get; set;}    
        public decimal? FloorRate { get; set; }
        public byte? MinusStock { get; set;}
        public DateTime? StartDate { get; set;}
        public DateTime? EndDate { get; set; }
        public decimal? Discount { get; set; }
    }
    public class SpFillCategoryG
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
