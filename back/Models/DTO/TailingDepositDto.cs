namespace back.Models.DTO;

public class TailingDepositDto
{
    public string NombreDeposito { get; set; }
    public string Ubicacion { get; set; }
    public DateOnly FechaCreacion { get; set; }
    public decimal ZonaUtm { get; set; }
    public decimal CoordenadaEste { get; set; }
    public decimal CoordenadaNorte { get; set; }
}

public class GetTailingDepositDto
{
    public int Id { get; set; }
    public string NombreDeposito { get; set; }
    public string Ubicacion { get; set; }
    public DateOnly FechaCreacion { get; set; }
    public decimal ZonaUtm { get; set; }
    public decimal CoordenadaEste { get; set; }
    public decimal CoordenadaNorte { get; set; }
}
