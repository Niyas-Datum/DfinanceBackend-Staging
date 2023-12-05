

using System.ComponentModel.DataAnnotations;

namespace Dfinance.Application.Dto.Finance
{
    public class CurrencyDto
    {
        [Required(ErrorMessage = "CurrencyName is mandatory!!")]
        public string CurrencyName { get; set; }
        [Required(ErrorMessage = "CurrencyCode is mandatory!!")]
        [StringLength(3, ErrorMessage = "CurrencyCode must be exactly 3 characters.")]
        public string CurrencyCode { get; set; }
        [Required(ErrorMessage = "CurrencyRate is mandatory!!")]
        public float CurrencyRate { get;set; }
        public bool IsDefault { get; set; }
        public string Coin {  get; set; }
    }
}
