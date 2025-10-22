using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Models.DB;

public class PiezometerMeasurements
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MedicionId { get; set; }

    [Column("Cota_Nivel_Piezometro", TypeName = "Decimal(10,5)")]
    public decimal CotaNivelPiezometro { get; set; }

    [Column("Longitud_medicion", TypeName = "Decimal(10,5)")]
    public decimal LongitudMedicion { get; set; }

    [Column("Comentario")]
    public string Comentario { get; set; }
    
    [Column("Fecha_medicion")]
    public DateOnly FechaMedicion { get; set; }

    [Column("PiezometroId")]
    public int PiezometroId { get; set; }
    
    [ForeignKey("PiezometroId")]
    public virtual Piezometer Piezometro { get; set; }
}