using back.Models.DTO;
using back.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace back.Controller;

[ApiController]
[Route("api/[controller]")]
public class TailingController : ControllerBase
{
    private readonly ITailingDeposit _tailingDeposit;

    public TailingController(ITailingDeposit tailingDeposit)
    {
        _tailingDeposit = tailingDeposit;
    }

    //POST
    [HttpPost("create-deposit")]
    public async Task<IActionResult> CreateTailingDeposit([FromBody] TailingDepositDto depositDto)
    {
        var response = await _tailingDeposit.AddTailingDeposit(depositDto);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPost("add-landmark")]
    public async Task<IActionResult> AddLandmark([FromBody] TopographicLandmarkDto hitoDto)
    {
        var response = await _tailingDeposit.AddTailingDepositWithLandmarks(hitoDto);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPost("add-measurement")]
    public async Task<IActionResult> AddMeasurement([FromBody] TopographicMeasurementsDto measurementsDto)
    {
        var response = await _tailingDeposit.AddMeasurement(measurementsDto);
        return StatusCode(response.StatusCode, response);
    }
    
    //GET
    [HttpGet("get-all-deposits")]
    public async Task<IActionResult> GetAllTailingDeposits()
    {
        var response = await _tailingDeposit.GetAllTailingDeposits();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-all-landmarks")]
    public async Task<IActionResult> GetAllLandmarksWithTailings()
    {
        var response = await _tailingDeposit.GetAllLandmarksWithTailings();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-all-landmarks-with-coordinates")]
    public async Task<IActionResult> GetAllLandmarksWithCoordinates()
    {
        var response = await _tailingDeposit.GetAllLandmarksWithCoordinates();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-all-measurements")]
    public async Task<IActionResult> GetAllMeasurementsWithLandmark()
    {
        var response = await _tailingDeposit.GetAllMeasurementsWithLandmark();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-measurements-by-landmark-id/{landmarkId}")]
    public async Task<IActionResult> GetMeasurementsByLandmarkId([FromRoute] int landmarkId)
    {
        var landmarkIdDto = new GetMeasurementsByLandmarkIdDto { HitoId = landmarkId };
        var response = await _tailingDeposit.GetMeasurementsByLandmarkId(landmarkIdDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("get-measurements-by-landmark-ids")]
    public async Task<IActionResult> GetMeasurementsByLandmarkIds([FromBody] GetMeasurementsByLandmarkIdsDto getMeasurementsByLandmarkIdsDto)
    {
        var response = await _tailingDeposit.GetMeasurementsByLandmarkIds(getMeasurementsByLandmarkIdsDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-max-measurement-by-landmark-id/{landmarkId}")]
    public async Task<IActionResult> GetMeasurementWithMaxTotalLandmarkId([FromRoute] int landmarkId)
    {
        var landmarkIdDto = new GetMeasurementsByLandmarkIdDto { HitoId = landmarkId };
        var response = await _tailingDeposit.GetMeasurementWithMaxTotalLandmarkId(landmarkIdDto);
        return StatusCode(response.StatusCode, response);
    }
    
    //PUT 
    [HttpPut("edit-measurement/{medicionId}")]
    public async Task<IActionResult> EditMeasurement(
        [FromRoute] int medicionId,
        [FromBody] TopographicMeasurementsDto measurementsDto
    )
    {
        var response = await _tailingDeposit.EditMeasurementAndRecalculate(medicionId, measurementsDto);
        return StatusCode(response.StatusCode, response);
    }
}