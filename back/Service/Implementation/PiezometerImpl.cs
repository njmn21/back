using back.Data;
using back.Models.DB;
using back.Models.DTO;
using back.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace back.Service.Implementation;

public class PiezometerImpl : IPiezometer
{
    private readonly AppDbContext _context;

    public PiezometerImpl(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<int>> AddPiezometer(PiezometerDto piezometerDto)
    {
        var response = new ApiResponse<int>();

        try
        {
            var existePiezometro =
                await _context.Piezometro.AnyAsync(
                    p => p.NombrePiezometro == piezometerDto.NombrePiezometro
                );
            if (existePiezometro)
            {
                response.StatusCode = 400;
                response.Message = "Ya existe un piezómetro con ese nombre";
                return response;
            }

            Piezometer piezometer = new Piezometer
            {
                NombrePiezometro = piezometerDto.NombrePiezometro,
                Este = piezometerDto.Este,
                Norte = piezometerDto.Norte,
                Elevacion = piezometerDto.Elevacion,
                Ubicacion = piezometerDto.Ubicacion,
                StickUp = piezometerDto.StickUp,
                CotaActualBocaTubo = piezometerDto.CotaActualBocaTubo,
                CotaActualTerreno = piezometerDto.CotaActualTerreno,
                CotaFondoPozo = piezometerDto.CotaFondoPozo,
                ProfundidadActualPozo = piezometerDto.CotaActualTerreno - piezometerDto.CotaFondoPozo,
                FechaInstalacion = piezometerDto.FechaInstalacion,
                DepositoId = piezometerDto.DepositoId
            };
            
            await _context.Piezometro.AddAsync(piezometer);
            await _context.SaveChangesAsync();

            response.Result = piezometer.PiezometroId;
            response.Message = "Piezómetro creado correctamente";
            response.StatusCode = 201;
        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al crear el piezómetro" + e.Message;
        }

        return response;
    }

    public async Task<ApiResponse<int>> AddPiezometerMeasurement(PiezometerMeasurementDto piezometerMeasurementDto)
    {
        var response = new ApiResponse<int>();

        try
        {
            var piezometer = await _context.Piezometro
                .Where(p => p.PiezometroId == piezometerMeasurementDto.PiezometerId)
                .Select(p => new
                {
                    p.CotaActualTerreno,
                    p.CotaFondoPozo,
                    p.StickUp
                })
                .FirstOrDefaultAsync();

            if (piezometer == null)
            {
                response.StatusCode = 400;
                response.Message = "El piezómetro especificado no existe.";
                return response;
            }
            
            if (piezometerMeasurementDto.LongitudMedicion < 0)
            {
                response.StatusCode = 400;
                response.Message = "La longitud de la medición no puede ser negativa.";
                return response;
            }
            
            var lastMeasurement = await _context.MedicionPiezometro
                .Where(m => m.PiezometroId == piezometerMeasurementDto.PiezometerId)
                .OrderByDescending(m => m.FechaMedicion)
                .FirstOrDefaultAsync();

            if (lastMeasurement != null && piezometerMeasurementDto.FechaMedicion <= lastMeasurement.FechaMedicion)
            {
                response.StatusCode = 400;
                response.Message = "La fecha de la nueva medición debe ser posterior a la última medición registrada.";
                return response;
            }

            decimal alturaPozo = piezometer.CotaActualTerreno - piezometer.CotaFondoPozo;
            decimal cotaNivelPiezometro = piezometer.CotaFondoPozo +
                                  (alturaPozo - (piezometerMeasurementDto.LongitudMedicion - piezometer.StickUp));

            PiezometerMeasurements measurements = new PiezometerMeasurements
            {
                CotaNivelPiezometro = cotaNivelPiezometro,
                LongitudMedicion = piezometerMeasurementDto.LongitudMedicion,
                Comentario = piezometerMeasurementDto.Comentario,
                FechaMedicion = piezometerMeasurementDto.FechaMedicion,
                PiezometroId = piezometerMeasurementDto.PiezometerId
            };
            
            await _context.MedicionPiezometro.AddAsync(measurements);
            await _context.SaveChangesAsync();
            
            response.Result = measurements.MedicionId;
            response.Message = "Medición del piezómetro agregada correctamente";
            response.StatusCode = 201;
        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al agregar la medición del piezómetro: " + e.Message;
        }
        
        return response;
    }

    public async Task<ApiResponse> GetAllMeasurementsPiezometerById(int piezometerId)
    {
        ApiResponse<List<GetAllMeasurementsPiezometerDto>> response = new();

        try
        {
            var measurements = (
                from mp in _context.MedicionPiezometro.AsNoTracking()
                join p in _context.Piezometro.AsNoTracking()
                on mp.PiezometroId equals p.PiezometroId
                where mp.PiezometroId == piezometerId
                select new GetAllMeasurementsPiezometerDto
                {
                    MedicionId = mp.MedicionId,
                    FechaMedicion = mp.FechaMedicion,
                    CotaActualTerreno = p.CotaActualTerreno,
                    CotaFondoPozo = p.CotaFondoPozo,
                    CotaNivelPiezometro = mp.CotaNivelPiezometro,
                    ProfundidadActualPozo = p.ProfundidadActualPozo,
                    LongitudMedicion = mp.LongitudMedicion,
                    Comentario = mp.Comentario
                }).ToListAsync();

            response.Result = await measurements;
            response.Message = "Mediciones obtenidas exitosamente";
            response.StatusCode = 200;
        }
         catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al obtener las medidas" + e.Message;
        }

        return response;
    }

    public async Task<ApiResponse> GetAllMeasurementsPiezometerByIds(GetMeasurementsPiezometersByIds ids)
    {
        ApiResponse<List<GetAllMeasurementsPiezometerDto>> response = new();

        try
        {
            if (!ids.PiezometersIds.Any())
            {
                response.StatusCode = 400;
                response.Message = "No se proporcionaron Ids de piezometros";
                return response;
            }

            var measurements = await (
                from mp in _context.MedicionPiezometro.AsNoTracking()
                join p in _context.Piezometro.AsNoTracking()
                on mp.PiezometroId equals p.PiezometroId
                where ids.PiezometersIds.Contains(mp.PiezometroId)
                select new GetAllMeasurementsPiezometerDto
                {
                    MedicionId = mp.MedicionId,
                    FechaMedicion = mp.FechaMedicion,
                    CotaFondoPozo = p.CotaFondoPozo,
                    CotaActualTerreno = p.CotaActualTerreno,
                    CotaNivelPiezometro = mp.CotaNivelPiezometro,
                    ProfundidadActualPozo = p.ProfundidadActualPozo,
                    LongitudMedicion = mp.LongitudMedicion,
                    Comentario = mp.Comentario,
                    PiezometroId = mp.PiezometroId
                }).ToListAsync();

            response.Result = measurements;
            response.Message = "Mediciones obtenidas exitosamente";
            response.StatusCode = 200;
        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al obtener las mediciones " + e.Message;
        }

        return response;
    }

    public async Task<ApiResponse> GetAllPiezometers()
    {
        ApiResponse<List<GetAllPiezometersDto>> response = new();

        try
        {
            var piezometros = await _context.Piezometro
                .AsNoTracking()
                .Select(p => new GetAllPiezometersDto
                {
                    PiezometroId = p.PiezometroId,
                    NombrePiezometro = p.NombrePiezometro,
                    Este = p.Este,
                    Norte = p.Norte,
                    Elevacion = p.Elevacion,
                    Ubicacion = p.Ubicacion,
                    Fecha_instalacion = p.FechaInstalacion,
                    Estado = p.Estado
                })
                .ToListAsync();

            response.Result = piezometros;
            response.Message = "Piezometros obtenidos exitosamente";
            response.StatusCode = 200;

        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al obtener los piezometros" + e.Message;
        }

        return response;
    }
}