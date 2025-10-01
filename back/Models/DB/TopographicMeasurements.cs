using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Models.DB;

public class TopographicMeasurements
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MedicionId { get; set; }
    [Column("Este", TypeName = "decimal(15,5)")]
    public decimal Este { get; set; }
    [Column("Norte", TypeName = "decimal(15,5)")]
    public decimal Norte { get; set; }
    [Column("Elevacion", TypeName = "decimal(15,5)")]
    public decimal Elevacion { get; set; }
    
    //Desplazamientos Absolutos
    [Column("HorizontalAbsoluto", TypeName = "decimal(15,5)")]
    public decimal HorizontalAbsoluto { get; set; }
    [Column("VerticalAbsoluto", TypeName = "decimal(15,5)")]
    public decimal VerticalAbsoluto { get; set; }
    [Column("TotalAbsoluto", TypeName = "decimal(15,5)")]
    public decimal TotalAbsoluto { get; set; }
    [Column("AcimutAbsoluto", TypeName = "decimal(15,5)")]
    public decimal AcimutAbsoluto { get; set; }
    [Column("BuzamientoAbsoluto", TypeName = "decimal(15,5)")]
    public decimal BuzamientoAbsoluto { get; set; }
    
    //Desplazamientos Relativos
    [Column("HorizontalRelativo", TypeName = "decimal(15,5)")]
    public decimal HorizontalRelativo { get; set; }
    [Column("VerticalRelativo", TypeName = "decimal(15,5)")]
    public decimal VerticalRelativo { get; set; }
    [Column("TotalRelativo", TypeName = "decimal(15,5)")]
    public decimal TotalRelativo { get; set; }
    [Column("AcimutRelativo", TypeName = "decimal(15,5)")]
    public decimal AcimutRelativo { get; set; }
    [Column("BuzamientoRelativo", TypeName = "decimal(15,5)")]
    public decimal BuzamientoRelativo { get; set; }
    
    [Column("HorizontalAcmulado", TypeName = "decimal(15,5)")]
    public decimal HorizontalAcmulado { get; set; }
    [Column("VelocidadMedia", TypeName = "decimal(15,5)")]
    public decimal VelocidadMedia { get; set; }
    [Column("InversaVelocidadMedia", TypeName = "decimal(15,5)")]
    public decimal InversaVelocidadMedia { get; set; }
    
    [Column("Fecha")]
    public DateOnly Fecha { get; set; }
    [Column("FrecuenciaMonitoreo")]
    public int FrecuenciaMonitoreo { get; set; }
    [Column("TiempoDias")]
    public int TiempoDias { get; set; }
    
    [Column("HitoId")]
    public int HitoId { get; set; }
    [ForeignKey("HitoId")]
    public virtual TopographicLandmark Hito { get; set; }
    
    [Column("EsBase")]
    public bool EsBase { get; set; }
}