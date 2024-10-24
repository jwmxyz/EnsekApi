using Ensek.DataAccess.DbModels;
using Ensek.DataAccess.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Ensek.DataAccess;

public class EnsekDbContext : DbContext
{
    public DbSet<MeterReading> MeterReadings { get; set; }
    public DbSet<Account> Accounts { get; set; }
    
    public EnsekDbContext() : base()
    {
    }

    public EnsekDbContext(DbContextOptions<EnsekDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DataSeeder.SeedAccounts(modelBuilder);
    }
}