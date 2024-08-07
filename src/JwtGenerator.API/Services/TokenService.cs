using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtGenerator.API.Settings;
using Microsoft.IdentityModel.Tokens;

namespace JwtGenerator.API.Services;

public class TokenService(JwtSettings jwtSettings)
{
    private readonly JwtSettings _jwtSettings = jwtSettings;

    public string GenerateToken(string idCompany)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret ?? string.Empty));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, idCompany),
            // new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddYears(_jwtSettings.Expiration),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}