using back.Models.DTO;
using back.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back.Controller;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    
    //GET
    [HttpGet("get-all-deposits")]
    public async Task<IActionResult> GetAllTailingDeposits()
    {
        var response = await _tailingDeposit.GetAllTailingDeposits();
        return StatusCode(response.StatusCode, response);
    }

    //PUT
    [HttpPut("update-deposit/{depositoId}")]
    public async Task<IActionResult> UpdateTailingDeposit(
        [FromRoute] int depositoId,
        [FromBody] TailingDepositDto depositDto)
    {
        var response = await _tailingDeposit.EditTailingDeposit(depositoId, depositDto);
        return StatusCode(response.StatusCode, response);
    }
}