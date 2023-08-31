using Dfinance.Core.Domain;

namespace Dfinance.AuthAppllication.Dto;

public class AuthResponseDto
{
    public int Id { get; set; }
    public string? Username { get; set; } = null!;
    public string Token { get; set; } = null!;

    public AuthResponseDto(MaEmployee user, string token)
    {
        this.Username = user.UserName;
        this.Id = user.Id;
        this.Token = token;


    }
}