using System.ComponentModel.DataAnnotations;

namespace Dfinance.AuthAppllication.Dto;

public class AuthenticateRequestDto
{

    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }

    //[Required]
    public int CompanyId { get; set; }
    //[Required]
    public string CompanyName { get; set; } = null!;
    //[Required]
    public int BranchId { get; set; }
    //[Required]
    public string BranchName { get; set; } = null!;
}