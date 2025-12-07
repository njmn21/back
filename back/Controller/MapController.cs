using back.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IMap _map;

        public MapController(IMap map)
        {
            _map = map;
        }

        [HttpGet("get-key")]
        public async Task<IActionResult> GetKey()
        {
            var response = await _map.GetKey();
            return StatusCode(response.StatusCode, response);
        }
    }
}
