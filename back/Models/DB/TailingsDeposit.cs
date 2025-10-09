using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Models.DB;

public class TailingsDeposit
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DepositoId { get; set; }
    
    [Column("Nombre_deposito")]
    public string NombreDeposito { get; set; }
    
    [Column("Ubicacion")]
    public string Ubicacion { get; set; }
    
    [Column("Fecha_creacion")]
    public DateOnly FechaCreacion { get; set; }

    [Column("ZonaUtm")]
    public decimal ZonaUtm { get; set; }

    [Column("CoordenadaEste", TypeName = "Decimal(10,5)")]
    public decimal CoordenadaEste { get; set; }

    [Column("CoordenadaNorte", TypeName = "Decimal(10,5)")]
    public decimal CoordenadaNorte { get; set; }

    public virtual ICollection<TopographicLandmark> Hitos { get; set; } = new List<TopographicLandmark>();
}