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
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret ?? string.Empty);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("idCompany", idCompany)
            }),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}