using back.Models.DTO;
using back.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace back.Controller;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
public class PiezometerController : ControllerBase
{
    private readonly IPiezometer _piezometer;

    public PiezometerController(IPiezometer piezometer)
    {
        _piezometer = piezometer;
    }
    
    // POST
    [HttpPost("create-piezometer")]
    public async Task<IActionResult> CreatePiezometer([FromBody] PiezometerDto piezometerDto)
    {
        var response = await _piezometer.AddPiezometer(piezometerDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("add-piezometer-measurement")]
    public async Task<IActionResult> AddPiezometerMeasurement(
        [FromBody] PiezometerMeasurementDto piezometerMeasurementDto)
    {
        var response = await _piezometer.AddPiezometerMeasurement(piezometerMeasurementDto);
        return StatusCode(response.StatusCode, response);
    }

    // GET
    [HttpGet("get-all-piezometers")]
    public async Task<IActionResult> GetAllPiezometers()
    {
        var response = await _piezometer.GetAllPiezometers();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-all-measurementes-piezometer-id/{piezometerId}")]
    public async Task<IActionResult> GetAllMeasurementsPiezometersById([FromRoute] int piezometerId)
    {
        var response = await _piezometer.GetAllMeasurementsPiezometerById(piezometerId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("get-measurements-piezometer-by-ids")]
    public async Task<IActionResult> GetMeasurementsPiezometerByIds([FromBody] GetMeasurementsPiezometersByIds ids)
    {
        var response = await _piezometer.GetAllMeasurementsPiezometerByIds(ids);
        return StatusCode(response.StatusCode, response);
    }
}