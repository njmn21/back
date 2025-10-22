using back.Data;
using back.Models.DB;
using back.Models.DTO;
using back.Security;
using back.Security.Jwt;
using back.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace back.Service.Implementation;

public class AuthImpl : IAuth
{
    private readonly AppDbContext _context;
    private readonly HashPassword _hashPassword;
    private readonly IJwtToken _jwtToken;

    public AuthImpl(
        AppDbContext context,
        HashPassword hashPassword,
        IJwtToken jwtToken
        )
    {
        _context = context;
        _hashPassword = hashPassword;
        _jwtToken = jwtToken;
    }

    public async Task<ApiResponse<int>> Register(RegisterDto registerDto)
    {
        var response = new ApiResponse<int>();

        try
        {
            var modelUser = new User()
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = registerDto.Password,
                HashPassword = _hashPassword.encryptSHA256(registerDto.Password),
            };
            
            await _context.User.AddAsync(modelUser);
            await _context.SaveChangesAsync();

            response.Result = modelUser.IdUser;
            response.Message = "Registro exitoso";
            response.StatusCode = 200;
        }
        catch (Exception e)
        {
            response.Message = "Error Register: " + e.Message;
            response.StatusCode = 500;
        }

        return response;
    }

    public async Task<ApiResponse<string>> Login(LoginDto loginDto)
    {
        var response = new ApiResponse<string>();

        try
        {
            var user = await _context.User
                .Where(u =>
                    u.Email == loginDto.Email &&
                    u.HashPassword == _hashPassword.encryptSHA256(loginDto.Password)
                ).FirstOrDefaultAsync();

            if (user != null)
            {
                response.Result = _jwtToken.GenerateToken(user);
                response.Message = "Login exitoso";
                response.StatusCode = 200;
            }
            else
            {
                response.Result = null;
                response.Message = "Usuario o contraseña incorrectos";
                response.StatusCode = 401;
            }
        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error Login: " + e.Message;
        }
    
        return response;
    }

}