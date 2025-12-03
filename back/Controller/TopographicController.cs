using back.Models.DB;
using back.Models.DTO;
using back.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back.Controller;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
public class TopographicController : ControllerBase
{
    private readonly ITopographicLandmark _topographicLandmark;

    public TopographicController(ITopographicLandmark topographicLandmark)
    {
        _topographicLandmark = topographicLandmark;
    }

    // POST

    [HttpPost("add-landmark")]
    public async Task<IActionResult> AddLandmark([FromBody] TopographicLandmarkDto hitoDto)
    {
        var response = await _topographicLandmark.AddTailingDepositWithLandmarks(hitoDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("add-measurement")]
    public async Task<IActionResult> AddMeasurement([FromBody] TopographicMeasurementsDto measurementsDto)
    {
        var response = await _topographicLandmark.AddMeasurement(measurementsDto);
        return StatusCode(response.StatusCode, response);
    }

    // GET
    [HttpGet("get-all-landmarks")]
    public async Task<IActionResult> GetAllLandmarksWithTailings()
    {
        var response = await _topographicLandmark.GetAllLandmarksWithTailings();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-all-landmarks-with-coordinates")]
    public async Task<IActionResult> GetAllLandmarksWithCoordinates()
    {
        var response = await _topographicLandmark.GetAllLandmarksWithCoordinates();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-all-measurements")]
    public async Task<IActionResult> GetAllMeasurementsWithLandmark()
    {
        var response = await _topographicLandmark.GetAllMeasurementsWithLandmark();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-measurements-by-landmark-id/{landmarkId}")]
    public async Task<IActionResult> GetMeasurementsByLandmarkId([FromRoute] int landmarkId)
    {
        var landmarkIdDto = new GetMeasurementsByLandmarkIdDto { HitoId = landmarkId };
        var response = await _topographicLandmark.GetMeasurementsByLandmarkId(landmarkIdDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("get-measurements-by-landmark-ids")]
    public async Task<IActionResult> GetMeasurementsByLandmarkIds([FromBody] GetMeasurementsByLandmarkIdsDto getMeasurementsByLandmarkIdsDto)
    {
        var response = await _topographicLandmark.GetMeasurementsByLandmarkIds(getMeasurementsByLandmarkIdsDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-max-measurement-by-landmark-id/{landmarkId}")]
    public async Task<IActionResult> GetMeasurementWithMaxTotalLandmarkId([FromRoute] int landmarkId)
    {
        var landmarkIdDto = new GetMeasurementsByLandmarkIdDto { HitoId = landmarkId };
        var response = await _topographicLandmark.GetMeasurementWithMaxTotalLandmarkId(landmarkIdDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-converted-landmarks")]
    public async Task<IActionResult> GetConvertedLandmarks()
    {
        var response = await _topographicLandmark.GetConvertLandMark();
        return StatusCode(response.StatusCode, response);
    }

    //PUT 
    [HttpPut("update-landmark/{hitoId}")]
    public async Task<IActionResult> EditHito(
        [FromRoute] int hitoId,
        [FromBody] TopographicLandmarkDto landmarkDto)
    {
        var response = await _topographicLandmark.EditLandmarkDeposit(hitoId, landmarkDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("edit-measurement/{medicionId}")]
    public async Task<IActionResult> EditMeasurement(
        [FromRoute] int medicionId,
        [FromBody] TopographicMeasurementsDto measurementsDto
    )
    {
        var response = await _topographicLandmark.EditMeasurementAndRecalculate(medicionId, measurementsDto);
        return StatusCode(response.StatusCode, response);
    }
}
