using Dfinance.DataModels.Dto.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Inventory
{
    public class CategoryDto
    {
        [Required(ErrorMessage = "Category Name is mandatory!!")]
        public string CategoryName { get; set; }//Description

        [Required(ErrorMessage = "Category Code is mandatory!!")]
        public string CategoryCode { get; set; }//code

        
        [Required(ErrorMessage = "Category Type is mandatory!!")]
        public PopUpDto CategoryType { get; set; }//TypeofWoodId in database

        [StringLength(1)]        
        public string? Category { get; set; }
      
        public byte ActiveFlag { get; set; }
       

        [StringLength(1)]
        public string? MeasurementType { get; set; }//StockType
        public decimal? MinimumQuantity { get; set; }
        public decimal? MaximumQuantity { get; set; }
        public decimal? FloorRate { get; set; }
        public byte? MinusStock { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? DiscountPerc { get; set; }
    }
}
