namespace back.Models.DTO;

public class TailingDepositDto
{
    public string NombreDeposito { get; set; }
    public string Ubicacion { get; set; }
    public DateOnly FechaCreacion { get; set; }
}

public class TopographicLandmarkDto
{
    public string NombreHito { get; set; }
    public int DepositoId { get; set; }
}

public class TopographicMeasurementsDto
{
    public DateOnly FechaMedicion { get; set; }
    public decimal Este { get; set; }
    public decimal Norte { get; set; }
    public decimal Elevacion { get; set; }
    public int HitoId { get; set; }
    public bool EsBase { get; set; }
}

public class GetTailingDepositDto
{
    public int Id { get; set; }
    public string NombreDeposito { get; set; }
    public string Ubicacion { get; set; }
    public DateOnly FechaCreacion { get; set; }
}

public class GetLandmarkWithDepositDto
{
    public int HitoId { get; set; }
    public string NombreHito { get; set; }
    public int DepositoId { get; set; }
    public string NombreDeposito { get; set; }
}

public class GetMeasurementsWithLandmarkDto
{
    public int MedicionId { get; set; }
    public decimal Este { get; set; }
    public decimal Norte { get; set; }
    public decimal Elevacion { get; set; }
    public decimal HorizontalAbsoluto { get; set; }
    public decimal VerticalAbsoluto { get; set; }
    public decimal TotalAbsoluto { get; set; }
    public decimal AcimutAbsoluto { get; set; }
    public decimal BuzamientoAbsoluto { get; set; }
    public decimal HorizontalRelativo { get; set; }
    public decimal VerticalRelativo { get; set; }
    public decimal TotalRelativo { get; set; }
    public decimal AcimutRelativo { get; set; }
    public decimal BuzamientoRelativo { get; set; }
    public decimal HorizontalAcumulado { get; set; }
    public decimal VelocidadMedia { get; set; }
    public decimal InversaVelocidadMedia { get; set; }
    public DateOnly FechaMedicion { get; set; }
    public int HitoId { get; set; }
    public string NombreHito { get; set; }
}

public class GetMeasurementsByLandmarkIdDto
{
    public int HitoId { get; set; }
}

public class GetMeasurementsByLandmarkIdsDto
{
    public List<int> HitoIds { get; set; }
}

public class GetMaxMeasurementByLandmarkIdDto
{
    public decimal TotalAbsoluto { get; set; }
    public decimal AcimutAbsoluto { get; set; }
    public decimal BuzamientoAbsoluto { get; set; }
    public decimal VelocidadMedia { get; set; }
    public DateOnly FechaMedicion { get; set; }
}