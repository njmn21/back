using back.Models.DTO;
using back.Service.Interface;

namespace back.Service.Implementation
{
    public class KeyImpl : IMap
    {
        public async Task<ApiResponse> GetKey()
        {
            ApiResponse<string> response = new();

            try
            {
                response.Result = "AIzaSyDd2y8aCf8mizBPeYa5UZc3ZATo7oTjONM";
                response.Message = "Clave obtenida correctamente";
                response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Error al obtener la clave: " + ex.Message;
            }

            return await Task.FromResult(response);
        }
    }
}
