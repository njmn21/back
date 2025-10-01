using back.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace back.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :
        base(options)
    {
    }
    
    public virtual DbSet<TailingsDeposit> Deposito { get; set; }
    public virtual DbSet<TopographicLandmark> Hito { get; set; }
    public virtual DbSet<TopographicMeasurements> Medicion { get; set; }
}