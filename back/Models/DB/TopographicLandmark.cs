using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Models.DB;

public class TopographicLandmark
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int HitoId { get; set; }
    
    [Column("Nombre_hito")]
    public string NombreHito { get; set; }
    
    [Column("DepositoId")]
    public int DepositoId { get; set; }
    
    [ForeignKey("DepositoId")]
    public virtual TailingsDeposit Deposito { get; set; }
    
    public virtual ICollection<TopographicMeasurements> Mediciones { get; set; } = new List<TopographicMeasurements>();
}