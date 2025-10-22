using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace back.Models.DB;

[Index(nameof(NombreHito), IsUnique = true)]
public class TopographicLandmark
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int HitoId { get; set; }
    
    [Column("Nombre_hito")]
    public string NombreHito { get; set; }
    
    [Column("DepositoId")]
    public int DepositoId { get; set; }

    [Column("Descripcion")]
    public string Descripcion { get; set; }
    
    [ForeignKey("DepositoId")]
    public virtual TailingsDeposit Deposito { get; set; }
    
    public virtual ICollection<TopographicMeasurements> Mediciones { get; set; } = new List<TopographicMeasurements>();
}