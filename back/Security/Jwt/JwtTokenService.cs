using back.Models.DB;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace back.Security.Jwt
{
    public class JwtTokenService(IConfiguration configuration) : IJwtToken
    {
        public string GenerateToken(User user)
        {
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtConfig = new JwtSecurityToken(
                                    claims: userClaims,
                                    expires: DateTime.UtcNow.AddMinutes(30),
                                    signingCredentials: credentials
                                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
