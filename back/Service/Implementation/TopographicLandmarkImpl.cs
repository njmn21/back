using back.Data;
using back.Models.DB;
using back.Models.DTO;
using back.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace back.Service.Implementation;

public class TopographicLandmarkImpl : ITopographicLandmark
{
    private readonly AppDbContext _context;

    public TopographicLandmarkImpl(AppDbContext context)
    {
        _context = context;
    }
    public async Task<ApiResponse<int>> AddTailingDepositWithLandmarks(TopographicLandmarkDto hitoDto)
    {
        var response = new ApiResponse<int>();

        try
        {
            var existeHito = await _context.Hito.AnyAsync(h => h.NombreHito == hitoDto.NombreHito);
            if (existeHito)
            {
                response.StatusCode = 400;
                response.Message = "Ya existe un hito con ese nombre.";
                return response;
            }

            TopographicLandmark hito = new TopographicLandmark
            {
                NombreHito = hitoDto.NombreHito,
                DepositoId = hitoDto.DepositoId,
                Descripcion = hitoDto.Descripcion
            };

            await _context.Hito.AddAsync(hito);
            await _context.SaveChangesAsync();

            response.Result = hito.HitoId;
            response.Message = "Hito agregado exitosamente";
            response.StatusCode = 201;
        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al agregar el hito" + e.Message;
        }

        return response;
    }

    public async Task<ApiResponse<int>> AddMeasurement(TopographicMeasurementsDto measurementsDto)
    {
        var response = new ApiResponse<int>();

        try
        {
            var existeInicial = await _context.MedicionHito
                .AnyAsync(m =>
                    m.HitoId == measurementsDto.HitoId && m.EsBase
                );

            if (measurementsDto.EsBase)
            {
                if (existeInicial)
                {
                    response.StatusCode = 400;
                    response.Message = "Ya existe una lectura base para este hito.";
                    return response;
                }
            }
            else
            {
                if (!existeInicial)
                {
                    response.StatusCode = 400;
                    response.Message = "Debe registrar primero la lectura base para este hito.";
                    return response;
                }

                var lastMeasurement = await _context.MedicionHito
                    .Where(m => m.HitoId == measurementsDto.HitoId)
                    .OrderByDescending(m => m.Fecha)
                    .FirstOrDefaultAsync();

                if (lastMeasurement != null && measurementsDto.FechaMedicion <= lastMeasurement.Fecha)
                {
                    response.StatusCode = 400;
                    response.Message = "La fecha de la nueva medición debe ser posterior a la última medición registrada.";
                    return response;
                }
            }

            var baseMeasurement = await _context.MedicionHito
                .Where(m =>
                    m.HitoId == measurementsDto.HitoId && m.EsBase)
                .Select(m => new
                {
                    m.Este,
                    m.Norte,
                    m.Elevacion,
                    m.Fecha
                })
                .FirstOrDefaultAsync();

            var lastMeasurementForCalc = await _context.MedicionHito
                .Where(m => m.HitoId == measurementsDto.HitoId)
                .OrderByDescending(m => m.Fecha)
                .Select(m => new
                {
                    m.Este,
                    m.Norte,
                    m.Elevacion,
                    m.HorizontalAcmulado,
                    m.Fecha,
                    m.TiempoDias
                })
                .FirstOrDefaultAsync();

            TopographicMeasurements medida = new TopographicMeasurements
            {
                Fecha = measurementsDto.FechaMedicion,
                Este = measurementsDto.Este,
                Norte = measurementsDto.Norte,
                Elevacion = measurementsDto.Elevacion,
                HitoId = measurementsDto.HitoId,
                EsBase = measurementsDto.EsBase,
            };

            if (baseMeasurement != null && !measurementsDto.EsBase)
            {
                medida.FrecuenciaMonitoreo = CalculateFrecuenciaMonitoreo(
                    measurementsDto.FechaMedicion,
                    lastMeasurementForCalc.Fecha
                );
                medida.TiempoDias = CalculateTiempoDias(
                    baseMeasurement.Fecha,
                    measurementsDto.FechaMedicion
                );
                medida.HorizontalAbsoluto = CalculateHorizontalAbsolute(
                    baseMeasurement.Este, baseMeasurement.Norte,
                    measurementsDto.Este, measurementsDto.Norte
                );
                medida.VerticalAbsoluto = CalculateVerticalAbsolute(
                    baseMeasurement.Elevacion, measurementsDto.Elevacion
                );
                medida.TotalAbsoluto = CalculateTotalAbsolute(
                    medida.HorizontalAbsoluto,
                    medida.VerticalAbsoluto
                );
                medida.AcimutAbsoluto = CalculateAcimutAbsolute(
                    baseMeasurement.Este, measurementsDto.Este,
                    baseMeasurement.Norte, measurementsDto.Norte
                );
                medida.BuzamientoAbsoluto = CalculateBuzamientoAbsolute(
                    medida.HorizontalAbsoluto,
                    medida.VerticalAbsoluto
                );
                medida.HorizontalRelativo = CalculateHorizontalRelative(
                    lastMeasurementForCalc.Este, lastMeasurementForCalc.Norte,
                    measurementsDto.Este, measurementsDto.Norte
                );
                medida.VerticalRelativo = CalculateVerticalRelative(
                    lastMeasurementForCalc.Elevacion, measurementsDto.Elevacion
                );
                medida.TotalRelativo = CalculateTotalRelative(
                    medida.HorizontalRelativo,
                    medida.VerticalRelativo
                );
                medida.AcimutRelativo = CalculateAcimutRelative(
                    lastMeasurementForCalc.Este, measurementsDto.Este,
                    lastMeasurementForCalc.Norte, measurementsDto.Norte
                );
                medida.BuzamientoRelativo = CalculateBuzamientoRelative(
                    medida.HorizontalRelativo,
                    medida.VerticalRelativo
                );
                medida.HorizontalAcmulado = CalculateHorizontalAccumulated(
                    medida.TotalRelativo,
                    lastMeasurementForCalc.HorizontalAcmulado
                );
                medida.VelocidadMedia = CalculateVelocidadMedia(
                    medida.TotalRelativo,
                    medida.TiempoDias,
                    lastMeasurementForCalc.TiempoDias
                );
                medida.InversaVelocidadMedia = CalculateInversaVelocidadMedia(
                    medida.VelocidadMedia
                );

            }

            await _context.MedicionHito.AddAsync(medida);
            await _context.SaveChangesAsync();

            response.Result = medida.MedicionId;
            response.Message = "Medicion agregada exitosamente";
            response.StatusCode = 201;
        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al agregar la medicion" + e.Message;
        }

        return response;
    }

    public async Task<ApiResponse> GetAllLandmarksWithTailings()
    {
        ApiResponse<List<GetLandmarkWithDepositDto>> response = new();

        try
        {
            var hitos = await (
                from h in _context.Hito.AsNoTracking()
                join d in _context.Deposito.AsNoTracking()
                    on h.DepositoId equals d.DepositoId
                select new GetLandmarkWithDepositDto
                {
                    HitoId = h.HitoId,
                    NombreHito = h.NombreHito,
                    DepositoId = d.DepositoId,
                    NombreDeposito = d.NombreDeposito
                }).ToListAsync();

            response.Result = hitos;
            response.Message = "Hitos obtenidos exitosamente";
            response.StatusCode = 200;
        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al obtener los hitos" + e.Message;
        }

        return response;
    }

    public async Task<ApiResponse> GetAllLandmarksWithCoordinates()
    {
        ApiResponse<List<GetAllLandmarksWithCoordinatesDto>> response = new();

        try
        {
            var result = await (
                from h in _context.Hito.AsNoTracking()
                join d in _context.Deposito.AsNoTracking() on h.DepositoId equals d.DepositoId
                join m in _context.MedicionHito.AsNoTracking()
                    .Where(x =>
                        x.EsBase
                        ) on h.HitoId equals m.HitoId into medidas
                from m in medidas.DefaultIfEmpty()
                select new GetAllLandmarksWithCoordinatesDto
                {
                    HitoId = h.HitoId,
                    NombreHito = h.NombreHito,
                    DepositoId = d.DepositoId,
                    NombreDeposito = d.NombreDeposito,
                    Este = m != null ? m.Este : 0,
                    Norte = m != null ? m.Norte : 0,
                    Elevacion = m != null ? m.Elevacion : 0,
                    Descripcion = h.Descripcion
                }
            ).ToListAsync();

            response.Result = result;
            response.Message = "Hitos obtenidos exitosamente";
            response.StatusCode = 200;
        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al obtener los hitos con coordenadas" + e.Message;
        }

        return response;
    }

    public async Task<ApiResponse> GetAllMeasurementsWithLandmark()
    {
        ApiResponse<List<GetMeasurementsWithLandmarkDto>> response = new();
        try
        {
            var measurements = (
                from m in _context.MedicionHito.AsNoTracking()
                join h in _context.Hito.AsNoTracking()
                    on m.HitoId equals h.HitoId
                select new GetMeasurementsWithLandmarkDto
                {
                    MedicionId = m.MedicionId,
                    Este = m.Este,
                    Norte = m.Norte,
                    Elevacion = m.Elevacion,
                    HorizontalAbsoluto = m.HorizontalAbsoluto,
                    VerticalAbsoluto = m.VerticalAbsoluto,
                    TotalAbsoluto = m.TotalAbsoluto,
                    AcimutAbsoluto = m.AcimutAbsoluto,
                    BuzamientoAbsoluto = m.BuzamientoAbsoluto,
                    HorizontalRelativo = m.HorizontalRelativo,
                    VerticalRelativo = m.VerticalRelativo,
                    TotalRelativo = m.TotalRelativo,
                    AcimutRelativo = m.AcimutRelativo,
                    BuzamientoRelativo = m.BuzamientoRelativo,
                    HorizontalAcumulado = m.HorizontalAcmulado,
                    VelocidadMedia = m.VelocidadMedia,
                    InversaVelocidadMedia = m.InversaVelocidadMedia, //m.VelocidadMedia != 0 ? 1 / m.VelocidadMedia : 0,
                    FechaMedicion = m.Fecha,
                    HitoId = h.HitoId,
                    NombreHito = h.NombreHito
                }).ToListAsync();

            response.Result = await measurements;
            response.Message = "Mediciones obtenidas exitosamente";
            response.StatusCode = 200;
        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al obtener las mediciones" + e.Message;
        }

        return response;
    }

    public async Task<ApiResponse> GetMeasurementsByLandmarkId(GetMeasurementsByLandmarkIdDto measurementsByLandmarkIdDto)
    {
        ApiResponse<List<GetMeasurementsWithLandmarkDto>> response = new();

        try
        {
            var measurements = (
                from m in _context.MedicionHito.AsNoTracking()
                join h in _context.Hito.AsNoTracking()
                    on m.HitoId equals h.HitoId
                where m.HitoId == measurementsByLandmarkIdDto.HitoId
                select new GetMeasurementsWithLandmarkDto
                {
                    MedicionId = m.MedicionId,
                    Este = m.Este,
                    Norte = m.Norte,
                    Elevacion = m.Elevacion,
                    HorizontalAbsoluto = m.HorizontalAbsoluto,
                    VerticalAbsoluto = m.VerticalAbsoluto,
                    TotalAbsoluto = m.TotalAbsoluto,
                    AcimutAbsoluto = m.AcimutAbsoluto,
                    BuzamientoAbsoluto = m.BuzamientoAbsoluto,
                    HorizontalRelativo = m.HorizontalRelativo,
                    VerticalRelativo = m.VerticalRelativo,
                    TotalRelativo = m.TotalRelativo,
                    AcimutRelativo = m.AcimutRelativo,
                    BuzamientoRelativo = m.BuzamientoRelativo,
                    HorizontalAcumulado = m.HorizontalAcmulado,
                    VelocidadMedia = m.VelocidadMedia,
                    InversaVelocidadMedia = m.InversaVelocidadMedia,//m.VelocidadMedia != 0 ? 1 / m.VelocidadMedia : 0,
                    FechaMedicion = m.Fecha,
                    HitoId = h.HitoId,
                    NombreHito = h.NombreHito
                }).ToListAsync();

            response.Result = await measurements;
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

    public async Task<ApiResponse> GetMeasurementsByLandmarkIds(GetMeasurementsByLandmarkIdsDto getMeasurementsByLandmarkIdsDto)
    {
        ApiResponse<List<GetMeasurementsWithLandmarkDto>> response = new();

        try
        {
            if (!getMeasurementsByLandmarkIdsDto.HitoIds.Any())
            {
                response.StatusCode = 400;
                response.Message = "No se proporcionaron IDs de hitos.";
                return response;
            }

            var measurements = await (
                from m in _context.MedicionHito.AsNoTracking()
                join h in _context.Hito.AsNoTracking()
                on m.HitoId equals h.HitoId
                where getMeasurementsByLandmarkIdsDto.HitoIds.Contains(m.HitoId)
                select new GetMeasurementsWithLandmarkDto
                {
                    MedicionId = m.MedicionId,
                    Este = m.Este,
                    Norte = m.Norte,
                    Elevacion = m.Elevacion,
                    HorizontalAbsoluto = m.HorizontalAbsoluto,
                    VerticalAbsoluto = m.VerticalAbsoluto,
                    TotalAbsoluto = m.TotalAbsoluto,
                    AcimutAbsoluto = m.AcimutAbsoluto,
                    BuzamientoAbsoluto = m.BuzamientoAbsoluto,
                    HorizontalRelativo = m.HorizontalRelativo,
                    VerticalRelativo = m.VerticalRelativo,
                    TotalRelativo = m.TotalRelativo,
                    AcimutRelativo = m.AcimutRelativo,
                    BuzamientoRelativo = m.BuzamientoRelativo,
                    HorizontalAcumulado = m.HorizontalAcmulado,
                    VelocidadMedia = m.VelocidadMedia,
                    InversaVelocidadMedia = m.InversaVelocidadMedia,
                    FechaMedicion = m.Fecha,
                    HitoId = h.HitoId,
                    NombreHito = h.NombreHito
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

    public async Task<ApiResponse> GetMeasurementWithMaxTotalLandmarkId(GetMeasurementsByLandmarkIdDto landmarkId)
    {
        ApiResponse<GetMaxMeasurementByLandmarkIdDto> response = new();

        try
        {
            bool existsLandmark = await _context.Hito
                .AsNoTracking()
                .AnyAsync(h => h.HitoId == landmarkId.HitoId);

            if (!existsLandmark)
            {
                response.StatusCode = 400;
                response.Message = "El hito proporcionado no existe.";
                return response;
            }

            var maxMeasurement = await _context.MedicionHito
                .AsNoTracking()
                .Where(m => m.HitoId == landmarkId.HitoId)
                .OrderByDescending(m => m.TotalAbsoluto)
                .Select(m => new GetMaxMeasurementByLandmarkIdDto
                {
                    TotalAbsoluto = m.TotalAbsoluto,
                    AcimutAbsoluto = m.AcimutAbsoluto,
                    BuzamientoAbsoluto = m.BuzamientoAbsoluto,
                    VelocidadMedia = m.VelocidadMedia,
                    FechaMedicion = m.Fecha
                })
                .FirstOrDefaultAsync();

            response.Result = maxMeasurement;
            response.Message = "Medición con mayor total absoluto obtenida exitosamente";
            response.StatusCode = 200;
        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al obtener la medicion " + e.Message;
        }

        return response;
    }

    public async Task<ApiResponse> EditMeasurementAndRecalculate(int medicionId, TopographicMeasurementsDto measurementsDto)
    {
        var response = new ApiResponse<int>();

        try
        {
            var medida = await _context.MedicionHito.FindAsync(medicionId);
            if (medida == null)
            {
                response.StatusCode = 400;
                response.Message = "La medición proporcionada no existe.";
                return response;
            }

            medida.Fecha = measurementsDto.FechaMedicion;
            medida.Este = measurementsDto.Este;
            medida.Norte = measurementsDto.Norte;
            medida.Elevacion = measurementsDto.Elevacion;

            await _context.SaveChangesAsync();

            if (medida.EsBase)
            {
                var mediciones = await _context.MedicionHito
                    .Where(m => m.HitoId == medida.HitoId)
                    .ToListAsync();

                if (mediciones.Count == 1)
                {
                    response.Result = medida.MedicionId;
                    response.Message = "Medición inicial editada exitosamente";
                    response.StatusCode = 200;
                    return response;
                }
            }

            var medicionesRecalcular = await _context.MedicionHito
                .Where(m => m.HitoId == medida.HitoId)
                .OrderBy(m => m.Fecha)
                .ToListAsync();

            var baseMeasurement = medicionesRecalcular.FirstOrDefault(m => m.EsBase);

            TopographicMeasurements? anterior = baseMeasurement;
            foreach (var m in medicionesRecalcular)
            {
                if (m.Fecha < medida.Fecha)
                {
                    anterior = m;
                    continue;
                }

                m.FrecuenciaMonitoreo = CalculateFrecuenciaMonitoreo(m.Fecha, anterior.Fecha);
                m.TiempoDias = CalculateTiempoDias(baseMeasurement.Fecha, m.Fecha);
                m.HorizontalAbsoluto = CalculateHorizontalAbsolute(baseMeasurement.Este, baseMeasurement.Norte, m.Este, m.Norte);
                m.VerticalAbsoluto = CalculateVerticalAbsolute(baseMeasurement.Elevacion, m.Elevacion);
                m.TotalAbsoluto = CalculateTotalAbsolute(m.HorizontalAbsoluto, m.VerticalAbsoluto);
                m.AcimutAbsoluto = CalculateAcimutAbsolute(baseMeasurement.Este, m.Este, baseMeasurement.Norte, m.Norte);
                m.BuzamientoAbsoluto = CalculateBuzamientoAbsolute(m.HorizontalAbsoluto, m.VerticalAbsoluto);
                m.HorizontalRelativo = CalculateHorizontalRelative(anterior.Este, anterior.Norte, m.Este, m.Norte);
                m.VerticalRelativo = CalculateVerticalRelative(anterior.Elevacion, m.Elevacion);
                m.TotalRelativo = CalculateTotalRelative(m.HorizontalRelativo, m.VerticalRelativo);
                m.AcimutRelativo = CalculateAcimutRelative(anterior.Este, m.Este, anterior.Norte, m.Norte);
                m.BuzamientoRelativo = CalculateBuzamientoRelative(m.HorizontalRelativo, m.VerticalRelativo);
                m.HorizontalAcmulado = CalculateHorizontalAccumulated(m.TotalRelativo, anterior.HorizontalAcmulado);
                m.VelocidadMedia = CalculateVelocidadMedia(m.TotalRelativo, m.TiempoDias, anterior.TiempoDias);
                m.InversaVelocidadMedia = CalculateInversaVelocidadMedia(m.VelocidadMedia);

                anterior = m;
            }

            await _context.SaveChangesAsync();

            response.Result = medida.MedicionId;
            response.Message = "Medición editada exitosamente";
            response.StatusCode = 200;
        }
        catch (Exception e)
        {
            response.StatusCode = 500;
            response.Message = "Error al editar la medicion" + e.Message;
        }

        return response;
    }

    private decimal CalculateHorizontalAbsolute(decimal esteBase, decimal norteBase, decimal esteActual, decimal norteActual)
    {
        double deltaEste = (double)(esteActual - esteBase);
        double deltaNorte = (double)(norteActual - norteBase);
        double distancia = Math.Sqrt(Math.Pow(deltaNorte, 2) + Math.Pow(deltaEste, 2));
        return 100 * (decimal)distancia;
    }

    private decimal CalculateVerticalAbsolute(decimal elevacionBase, decimal elevacionActual)
    {
        return 100 * (elevacionActual - elevacionBase);
    }

    private decimal CalculateTotalAbsolute(decimal horizontal, decimal vertical)
    {
        double horizontalD = (double)horizontal;
        double verticalD = (double)vertical;
        double total = Math.Sqrt(Math.Pow(horizontalD, 2) + Math.Pow(verticalD, 2));
        return (decimal)total;
    }

    private decimal CalculateAcimutAbsolute(decimal esteBase, decimal esteActual, decimal norteBase, decimal norteActual)
    {
        decimal DEste = esteActual - esteBase;
        decimal DNorte = norteActual - norteBase;

        int cuadranteAbs = 0;
        if (DNorte == 0 && DEste == 0)
            cuadranteAbs = 0;
        else if (DNorte == 0 && DEste > 0)
            cuadranteAbs = 90;
        else if (DNorte == 0 && DEste < 0)
            cuadranteAbs = 270;
        else if (DEste == 0 && DNorte > 0)
            cuadranteAbs = 0;
        else if (DEste == 0 && DNorte < 0)
            cuadranteAbs = 180;
        else if (DNorte > 0 && DEste > 0)
            cuadranteAbs = 1;
        else if (DNorte > 0 && DEste < 0)
            cuadranteAbs = 2;
        else if (DNorte < 0 && DEste < 0)
            cuadranteAbs = 3;
        else if (DNorte < 0 && DEste > 0)
            cuadranteAbs = 4;

        double anguloAtanAbs = 0;
        if (cuadranteAbs != 0 && DNorte != 0)
            anguloAtanAbs = (Math.Atan((double)Math.Abs(DEste / DNorte)) * 180) / 3.141592654;

        decimal acimut = 0;
        if (cuadranteAbs == 0)
            acimut = 0;
        else if (cuadranteAbs == 90)
            acimut = 90;
        else if (cuadranteAbs == 180)
            acimut = 180;
        else if (cuadranteAbs == 270)
            acimut = 270;
        else if (cuadranteAbs == 1)
            acimut = 0 + (decimal)anguloAtanAbs;
        else if (cuadranteAbs == 2)
            acimut = 360 - (decimal)anguloAtanAbs;
        else if (cuadranteAbs == 3)
            acimut = 180 + (decimal)anguloAtanAbs;
        else if (cuadranteAbs == 4)
            acimut = 180 - (decimal)anguloAtanAbs;
        return acimut;
    }

    private decimal CalculateBuzamientoAbsolute(decimal deltaHorizontal, decimal deltaVertical)
    {
        if (deltaHorizontal > 0)
        {
            double PI = 3.141592654;
            return (decimal)((180 / PI) * Math.Atan((double)deltaVertical / ((double)deltaHorizontal * -1)));
        }
        return 0;
    }

    private decimal CalculateHorizontalRelative(decimal esteAnterior, decimal norteAnterior, decimal esteActual, decimal norteActual)
    {
        double deltaEste = (double)(esteActual - esteAnterior);
        double deltaNorte = (double)(norteActual - norteAnterior);
        double distancia = Math.Sqrt(Math.Pow(deltaNorte, 2) + Math.Pow(deltaEste, 2));
        return 100 * (decimal)distancia;
    }

    private decimal CalculateVerticalRelative(decimal elevacionAnterior, decimal elevacionActual)
    {
        return 100 * (elevacionActual - elevacionAnterior);
    }

    private decimal CalculateTotalRelative(decimal horizontal, decimal vertical)
    {
        double horizontalD = (double)horizontal;
        double verticalD = (double)vertical;
        double total = Math.Sqrt(Math.Pow(horizontalD, 2) + Math.Pow(verticalD, 2));
        return (decimal)total;
    }

    private decimal CalculateAcimutRelative(decimal esteAnterior, decimal esteActual, decimal norteAnterior, decimal norteActual)
    {
        decimal dEste = esteActual - esteAnterior;
        decimal dNorte = norteActual - norteAnterior;

        int cuadranteRel = 0;
        if (dNorte == 0 && dEste == 0)
            cuadranteRel = 0;
        else if (dNorte == 0 && dEste > 0)
            cuadranteRel = 90;
        else if (dNorte == 0 && dEste < 0)
            cuadranteRel = 270;
        else if (dEste == 0 && dNorte > 0)
            cuadranteRel = 0;
        else if (dEste == 0 && dNorte < 0)
            cuadranteRel = 180;
        else if (dNorte > 0 && dEste > 0)
            cuadranteRel = 1;
        else if (dNorte > 0 && dEste < 0)
            cuadranteRel = 2;
        else if (dNorte < 0 && dEste < 0)
            cuadranteRel = 3;
        else if (dNorte < 0 && dEste > 0)
            cuadranteRel = 4;

        double anguloAtanRel = 0;
        if (cuadranteRel != 0 && dNorte != 0)
            anguloAtanRel = (Math.Atan((double)Math.Abs(dEste / dNorte)) * 180) / 3.141592654;

        decimal acimut = 0;
        if (cuadranteRel == 0)
            acimut = 0;
        else if (cuadranteRel == 90)
            acimut = 90;
        else if (cuadranteRel == 180)
            acimut = 180;
        else if (cuadranteRel == 270)
            acimut = 270;
        else if (cuadranteRel == 1)
            acimut = 0 + (decimal)anguloAtanRel;
        else if (cuadranteRel == 2)
            acimut = 360 - (decimal)anguloAtanRel;
        else if (cuadranteRel == 3)
            acimut = 180 + (decimal)anguloAtanRel;
        else if (cuadranteRel == 4)
            acimut = 180 - (decimal)anguloAtanRel;
        return acimut;
    }

    private decimal CalculateBuzamientoRelative(decimal deltaHorizontal, decimal deltaVertical)
    {
        if (deltaHorizontal > 0)
        {
            double pi = 3.141592654;
            return (decimal)((180 / pi) * Math.Atan((double)deltaVertical / ((double)deltaHorizontal * -1)));
        }
        return 0;
    }

    private decimal CalculateHorizontalAccumulated(decimal totalRelativo, decimal horizontalAcumuladoAnterior)
    {
        return totalRelativo + horizontalAcumuladoAnterior;
    }

    private int CalculateFrecuenciaMonitoreo(DateOnly fechaActual, DateOnly fechaAnterior)
    {
        TimeSpan diferencia = fechaActual.ToDateTime(new TimeOnly()) - fechaAnterior.ToDateTime(new TimeOnly());
        return (int)diferencia.TotalDays;
    }

    private int CalculateTiempoDias(DateOnly fechaBase, DateOnly fechaActual)
    {
        TimeSpan diferencia = fechaActual.ToDateTime(new TimeOnly()) - fechaBase.ToDateTime(new TimeOnly());
        return (int)diferencia.TotalDays;
    }

    private decimal CalculateVelocidadMedia(decimal totalRelativo, int tiempoDiasActual, int tiempoDiasAnterior)
    {
        if (totalRelativo == 0 || (tiempoDiasActual - tiempoDiasAnterior) == 0)
            return 0;

        return totalRelativo / (tiempoDiasActual - tiempoDiasAnterior);
    }

    private decimal CalculateInversaVelocidadMedia(decimal velocidadMedia)
    {
        if (velocidadMedia == 0)
            return 0;
        return 1 / velocidadMedia;
    }

}
