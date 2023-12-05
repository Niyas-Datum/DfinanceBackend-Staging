using Dfinance.Application.Dto.Common;
using System.ComponentModel.DataAnnotations;

namespace Dfinance.Application.Dto.General
{
    public class CostCentreDto
    {
        [Required(ErrorMessage = "Code is mandatory!!")]
        public string Code {  get; set; }

        [Required(ErrorMessage= "Name is mandatory!!")]
        public string Name { get; set; }        

        [Required(ErrorMessage = "Nature is mandatory!!")]
        public DropDownDtoNature Nature { get; set; }

        public bool? Active { get; set; }
       // public string? Type { get; set; }
        public string? SerialNo { get; set; }
        public string? RegNo { get; set; }
        public int? Consultancy { get; set; }
        public DropdownDto? Status { get; set; }
        public string? Remarks { get; set; }
        public decimal? Rate { get; set; }
        public DateTime? StartDate { get; set; }
        public string? Make { get; set; }
        public string? MakeYear { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? ContractValue { get; set; }
        public decimal? InvoiceValue { get; set; }
        public int? Client { get; set; }
        public int? Engineer { get; set; }       
        public int? Foreman { get; set; }
        public string? Site { get; set; }
        public bool? IsGroup { get; set; }
        public DropDownDtoName? Category { get; set; }//CostCategoryID is passing
        public DropDownDtoName? CreateUnder { get; set; }      
        
       
    }
}
