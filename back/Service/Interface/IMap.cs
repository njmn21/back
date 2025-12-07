using back.Models.DTO;

namespace back.Service.Interface
{
    public interface IMap
    {
        Task<ApiResponse> GetKey();
    }
}
