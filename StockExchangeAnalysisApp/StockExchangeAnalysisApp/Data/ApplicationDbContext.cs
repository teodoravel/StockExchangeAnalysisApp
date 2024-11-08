using Microsoft.EntityFrameworkCore;
using StockExchangeAnalysisApp.Models; // Ensure you are referencing the models' namespace

namespace StockExchangeAnalysisApp.Data // Moved to Data namespace for consistency
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Issuer> Issuers { get; set; } // DbSet for Issuer model
        public DbSet<StockPrice> StockPrices { get; set; } // DbSet for StockPrice model
    }
}
