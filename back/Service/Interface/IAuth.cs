using back.Models.DTO;

namespace back.Service.Interface
{
    public interface IAuth
    {
        Task<ApiResponse<int>> Register(RegisterDto registerDto);
        Task<ApiResponse<string>> Login(LoginDto loginDto);
    }
}
