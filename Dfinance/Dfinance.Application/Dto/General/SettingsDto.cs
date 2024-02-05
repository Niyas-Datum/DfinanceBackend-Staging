using System.ComponentModel.DataAnnotations;

namespace Dfinance.Application.Dto.General
{
    public class SettingsDto
    {
        [Required(ErrorMessage = "Key is mandatory!!")]
        public string? Key { get; set; }
        [Required(ErrorMessage = "Value is mandatory!!")]
        public string? Value { get; set; }
        public string? Description { get; set; }
        public bool SystemSetting { get; set; }
    }
}
