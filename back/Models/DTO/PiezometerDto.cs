namespace back.Models.DTO;

public class PiezometerDto
{
    public string NombrePiezometro { get; set; }
    public decimal Este { get; set; }
    public decimal Norte { get; set; }
    public decimal Elevacion { get; set; }
    public decimal StickUp { get; set; }
    public decimal CotaActualBocaTubo { get; set; }
    public decimal CotaActualTerreno { get; set; }
    public decimal CotaFondoPozo { get; set; }
    public int DepositoId { get; set; }
    public DateOnly FechaInstalacion { get; set; }
    public string Ubicacion { get; set; }
}

public class PiezometerMeasurementDto
{
    public decimal LongitudMedicion { get; set; }
    public string Comentario { get; set; }
    public DateOnly FechaMedicion { get; set; }
    public int PiezometerId { get; set; }
}

public class GetAllPiezometersDto : PiezometerDto
{
    public int PiezometroId { get; set; }
    public string Estado { get; set; }
}

public class GetAllMeasurementsPiezometerDto
{
    public int MedicionId { get; set; }
    public DateOnly FechaMedicion { get; set; }
    public decimal CotaActualTerreno { get; set; }
    public decimal CotaFondoPozo { get; set; }
    public decimal CotaNivelPiezometro { get; set; }
    public decimal ProfundidadActualPozo { get; set; }
    public decimal LongitudMedicion { get; set; }
    public string Comentario { get; set; }
    public int PiezometroId { get; set; }
}

public class GetMeasurementsPiezometersByIds
{
    public List<int> PiezometersIds { get; set; }
}

public class GetConvertPiezometerDto
{
    public string NombrePiezometro { get; set; }
    public double Latitud { get; set; }
    public double Longitud { get; set; }
}

public class PutPiezometerDto : PiezometerDto
{
    public string Estado { get; set; }
}

public class PutMeasurementPiezometerDto
{
    public decimal LongitudMedicion { get; set; }
    public string Comentario { get; set; }
    public DateOnly FechaMedicion { get; set; }
}