using back.Models.DB;

namespace back.Security.Jwt
{
    public interface IJwtToken
    {
        string GenerateToken(User user);
    }
}
