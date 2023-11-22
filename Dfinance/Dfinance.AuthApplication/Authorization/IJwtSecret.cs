using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dfinance.AuthAppllication.Dto;
using Dfinance.Core.Domain;
using Dfinance.Core.Views.PagePermission;
using Dfinance.Shared.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dfinance.AuthAppllication.Authorization;

public interface IJwtSecret
{
    public string GenerateJwtToken(UserInfo user);
    public int? ValidateJwtToken(string? token);
}

public class JwtSecret : IJwtSecret
{
    private readonly AppSettings _appSettings;
    public JwtSecret(IOptions<AppSettings> appsettings)
    {
        _appSettings = appsettings.Value;
        if (string.IsNullOrEmpty(_appSettings.JwtSettings.Secret))

            throw new Exception("JWT Secret key not configured");
    }

    public string GenerateJwtToken(UserInfo user)
    {
        var tokenhandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret!);
        var tokendescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.EmployeeID.ToString()),
                new Claim("branch", user.Company.ToString()),
                new Claim("branchId", user.BranchId.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName.ToString()),
                new Claim("finyearStart", user.FinanceYearStartDate.ToString()),
                new Claim("finyearEnd", user.FinanceYearEndDate.ToString()),
                new Claim("vatNo", user.VatNo.ToString()),
                new Claim("numericFormat", user.NumericFormat.ToString()),
                new Claim(ClaimTypes.Role, user.UserRole.ToString()),
                new Claim("department", user.UserDepartment.ToString()),
                new Claim("hoCompany", user.HOCompanyName.ToString()),

            }),
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
        var key = Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret!);
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
