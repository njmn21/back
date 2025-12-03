using back.Data;
using back.Models.DB;
using back.Models.DTO;
using back.Service.Interface;
using back.Utils;
using Microsoft.EntityFrameworkCore;

namespace back.Service.Implementation;

public class TailingDepositImpl : ITailingDeposit
{
    private readonly AppDbContext _context;
    private readonly CacheService _cacheService;

    public TailingDepositImpl(
        AppDbContext context,
        CacheService cacheService
        )
    {
        _context = context;
        _cacheService = cacheService;
    }

    // POST
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

            _cacheService.Remove("allDepositos_cache");
            
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

    // PUT
    public async Task<ApiResponse> EditTailingDeposit(int id, TailingDepositDto depositDto)
    {
        var response = new ApiResponse<int>();

        try
        {
            var deposito = await _context.Deposito.FindAsync(id);
            if (deposito == null)
            {
                response.StatusCode = 400;
                response.Message = "El deposito proporcionado no existe";
                return response;
            }

            deposito.NombreDeposito = depositDto.NombreDeposito;
            deposito.Ubicacion = depositDto.Ubicacion;
            deposito.FechaCreacion = depositDto.FechaCreacion;
            deposito.ZonaUtm = depositDto.ZonaUtm;
            deposito.CoordenadaEste = depositDto.CoordenadaEste;
            deposito.CoordenadaNorte = depositDto.CoordenadaNorte;

            await _context.SaveChangesAsync();

            _cacheService.Remove("allDepositos_cache");

            response.StatusCode = 200;
            response.Message = "Deposito actualizado exitosamente";
        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al editar" + e.Message;
        }

        return response;
    }

    // GET
    public async Task<ApiResponse> GetAllTailingDeposits()
    {
        ApiResponse<List<GetTailingDepositDto>> response = new();
        
        try
        {
            if (_cacheService.TryGet("allDepositos_cache", out List<GetTailingDepositDto> cachedData))
            {
                response.Result = cachedData;
                response.StatusCode = 200;
                response.Message = "Datos obtenidos desde caché";
                return response;
            }

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

            _cacheService.Set("allDepositos_cache", deposits, TimeSpan.FromMinutes(3));

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