using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Models.DB;

public class Piezometer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PiezometroId { get; set; }

    [Column("Nombre_piezometro")]
    public string NombrePiezometro { get; set; }
    
    [Column("Este", TypeName = "Decimal(15,5)")]
    public decimal Este { get; set; }
    
    [Column("Norte", TypeName = "Decimal(15,5)")]
    public decimal Norte { get; set; }
    
    [Column("Elevacion", TypeName = "Decimal(15,5)")]
    public decimal Elevacion { get; set; }
    
    [Column("Ubicacion")]
    public string Ubicacion { get; set; }

    [Column("Stick_up", TypeName = "Decimal(10,5)")]
    public decimal StickUp { get; set; }

    [Column("Cota_actual_boca_tubo", TypeName = "Decimal(10,5)")]
    public decimal CotaActualBocaTubo { get; set; }

    [Column("Cota_actual_terreno", TypeName = "Decimal(10,5)")]
    public decimal CotaActualTerreno { get; set; }

    [Column("Cota_fondo_pozo", TypeName = "Decimal(10,5)")]
    public decimal CotaFondoPozo { get; set; }

    [Column("Profundidad_actual_pozo", TypeName = "Decimal(10,5)")]
    public decimal ProfundidadActualPozo { get; set; }

    [Column("Estado")]
    public string Estado { get; set; } = "Operativo"; // Operativo, Inactivo

    [Column("Fecha_instalacion")]
    public DateOnly FechaInstalacion { get; set; }
    
    [Column("DepositoId")]
    public int DepositoId { get; set; }
    
    [ForeignKey("DepositoId")]
    public virtual TailingsDeposit Deposito { get; set; }
    
    public virtual ICollection<PiezometerMeasurements> Mediciones { get; set; } = new List<PiezometerMeasurements>();
}