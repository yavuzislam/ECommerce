using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Payment.BusinessLayer.Utilities.Tool
{
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
            var accessToken = tokenHandler.WriteToken(token);

            //var refreshToken = GenerateRefreshToken();

            //var refreshTokenEntity = new RefreshToken
            //{
            //    Token = refreshToken,
            //    UserId = result.ID,
            //    Expires = DateTime.UtcNow.AddDays(JwtTokenDefaults.RefreshTokenExpireDays),
            //    IsRevoked = false,
            //    Created = DateTime.UtcNow
            //};

            //await _refreshTokenService.CreateRefreshTokenAsync(refreshTokenEntity);

            return new TokenResponseDto(accessToken, expireDate, result.Provider);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
