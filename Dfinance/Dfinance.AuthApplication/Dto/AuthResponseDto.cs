namespace Dfinance.AuthAppllication.Dto;

public class AuthResponseDto
{
    public int Id { get; set; }
    public string? Username { get; set; } = null!;
    public string Token { get; set; } = null!;


}