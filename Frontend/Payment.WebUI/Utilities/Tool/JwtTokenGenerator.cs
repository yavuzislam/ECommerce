using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Payment.WebUI.Utilities.Tool;

public class JwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TokenResponseDto GenerateToken(GetCheckAppUserViewModel result)
    {
        var jwtKey = _configuration["JWT_KEY"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new Exception("JWT_KEY is not configured.");
        }

        var claims = new List<Claim>();

        // Roller ekleniyor
        if (result.Roles != null && result.Roles.Count > 0)
        {
            foreach (var role in result.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        claims.Add(new Claim(ClaimTypes.NameIdentifier, result.ID.ToString()));

        if (!string.IsNullOrWhiteSpace(result.Email))
            claims.Add(new Claim(ClaimTypes.Email, result.Email));

        if (!string.IsNullOrWhiteSpace(result.Provider))
            claims.Add(new Claim("Provider", result.Provider));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        var signinCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expireDate = result.Provider == "Google"
            ? DateTime.UtcNow.AddMinutes(JwtTokenDefaults.GoogleTokenExpire)
            : DateTime.UtcNow.AddMinutes(JwtTokenDefaults.Expire);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: JwtTokenDefaults.ValidIssuer,
            audience: JwtTokenDefaults.ValidAudience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expireDate,
            signingCredentials: signinCredentials
            );

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        return new TokenResponseDto(tokenHandler.WriteToken(token), expireDate, result.Provider);
    }
}
