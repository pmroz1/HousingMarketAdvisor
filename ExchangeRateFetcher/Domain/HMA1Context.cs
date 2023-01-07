using System.Data.Entity;
using ExchangeRateFetcher.Domain.Classes;

namespace ExchangeRateFetcher.Domain;

public class HMA1Context : DbContext
{
    // public DbSet<Student> Students { get; set; }
    // public DbSet<Grade> Grades { get; set; }

    public HMA1Context(string connection) : base(connection)
    {
        Database.SetInitializer(new DropCreateDatabaseIfModelChanges<HMA1Context>());
    }

    public DbSet<CurrencyExchangeRate> CurrencyExchangeRates { get; set; }
    public DbSet<HMA1Config> Configs { get; set; }
}