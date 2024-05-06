
using Dfinance.DataModels.Validation;
namespace Dfinance.DataModels.Dto.Inventory
{
    public class ParentItemPopupDto
    {
       
        public int ID { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
    }

    public class UnitPopupDto
    {
        [NullValidation(ErrorMessage ="Unit Cannot be null")]
        public string Unit { get; set; }
        public string? BasicUnit { get; set; }
        public decimal? Factor { get; set; }
    }

    public class CatPopupDto
    {
        //[NullValidation(ErrorMessage ="Category Id Cannot be null")]
        public int? ID { get; set; }
        public string? Code { get; set; }
        public string? Category { get; set; }
        //public string? CategoryType { get; set; }
    }
}
