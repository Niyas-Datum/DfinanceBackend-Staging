
using System.ComponentModel.DataAnnotations;


namespace Dfinance.DataModels.Dto.Inventory
{
    public class CategoryTypeDto
    {
        [Required(ErrorMessage ="Description is Mandatory!!")]
        [RegularExpression(@"^[^*/]+$", ErrorMessage = "Category Name cannot contain special characters")]
        public string Description {  get; set; }

        [Required(ErrorMessage = "Code is mandatory!!")]       
        public string Code {  get; set; }

        public decimal? AvgStockQuantity {  get; set; } 
    }
}
