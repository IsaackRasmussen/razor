using Microsoft.EntityFrameworkCore;

namespace ShowCharts.DataSchema;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<TokenPrice> TokenPrices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }    
}