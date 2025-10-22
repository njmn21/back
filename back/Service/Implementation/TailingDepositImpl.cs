using back.Data;
using back.Models.DB;
using back.Models.DTO;
using back.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace back.Service.Implementation;

public class TailingDepositImpl : ITailingDeposit
{
    private readonly AppDbContext _context;

    public TailingDepositImpl(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<int>> AddTailingDeposit(TailingDepositDto depositDto)
    {
        var response = new ApiResponse<int>();
        try
        {
            TailingsDeposit deposit = new TailingsDeposit
            {
                NombreDeposito = depositDto.NombreDeposito,
                Ubicacion = depositDto.Ubicacion,
                FechaCreacion = depositDto.FechaCreacion,
                ZonaUtm = depositDto.ZonaUtm,
                CoordenadaEste = depositDto.CoordenadaEste,
                CoordenadaNorte = depositDto.CoordenadaNorte
            };
            
            await _context.Deposito.AddAsync(deposit);
            await _context.SaveChangesAsync();
            
            response.Result = deposit.DepositoId;
            response.Message = "Deposito creado exitosamente";
            response.StatusCode = 201;
        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al crear el deposito" + e.Message;
        }

        return response;
    }

    public async Task<ApiResponse> GetAllTailingDeposits()
    {
        ApiResponse<List<GetTailingDepositDto>> response = new();
        
        try
        {
            var deposits = await _context.Deposito
                .AsNoTracking()
                .Select(d => new GetTailingDepositDto
                {
                    Id = d.DepositoId,
                    NombreDeposito = d.NombreDeposito,
                    Ubicacion = d.Ubicacion,
                    FechaCreacion = d.FechaCreacion,
                    ZonaUtm = d.ZonaUtm,
                    CoordenadaEste = d.CoordenadaEste,
                    CoordenadaNorte = d.CoordenadaNorte
                })
                .ToListAsync();

            response.Result = deposits;
            response.Message = "Depositos obtenidos exitosamente";
            response.StatusCode = 200;
        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al obtener los depositos" + e.Message;
        }
        return response;
    }

}