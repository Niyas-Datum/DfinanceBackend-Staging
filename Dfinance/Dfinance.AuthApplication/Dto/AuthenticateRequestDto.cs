using System.ComponentModel.DataAnnotations;

namespace Dfinance.AuthAppllication.Dto;

public class AuthenticateRequestDto
{

    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }
}