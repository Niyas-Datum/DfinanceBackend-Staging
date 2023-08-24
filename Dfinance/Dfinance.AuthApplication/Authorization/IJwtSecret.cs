using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dfinance.Core.Domain;
using Dfinance.Shared.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dfinance.AuthAppllication.Authorization;

public interface IJwtSecret
{
    public string GenerateJwtToken(MaEmployee user);
    public int? ValidateJwtToken(string? token);
}

public class JwtSecret : IJwtSecret
{
    private readonly AppSettings _appSettings;
    public JwtSecret(IOptions<AppSettings> appsettings)
    {
        _appSettings = appsettings.Value;
        if (string.IsNullOrEmpty(_appSettings.Secret))

            throw new Exception("JWT Secret key not configured");
    }

    public string GenerateJwtToken(MaEmployee user)
    {
        var tokenhandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
        var tokendescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

        };
        var token = tokenhandler.CreateToken(tokendescriptor);
        return tokenhandler.WriteToken(token);
    }

    public int? ValidateJwtToken(string? token)
    {
        if (token == null)
            return null;
        var tokenhandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
        try
        {
            tokenhandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero

            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            return userId;
        }
        catch
        {
            return null;
        }
    }
}
