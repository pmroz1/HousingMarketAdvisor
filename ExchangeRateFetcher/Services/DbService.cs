using ExchangeRateFetcher.Domain;
using ExchangeRateFetcher.Domain.Classes;

namespace ExchangeRateFetcher.Services;

public class DbService
{
    private readonly HMA1Context _dbContext;

    public DbService(IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:DefaultConnection"];
        _dbContext = new HMA1Context(connectionString);
        Console.WriteLine("DbService created");
    }

    public void InsertExchangeRate(CurrencyExchangeRate exchangeRate)
    {
        Console.WriteLine("Inserting exchange rate");
        _dbContext.CurrencyExchangeRates.Add(exchangeRate);
        _dbContext.SaveChanges();
    }

    public List<HMA1Config> GetConfig()
    {
        Console.WriteLine("Getting config");
        // get distinct configs
        var config = _dbContext.Configs
            .Distinct()
            .ToList();

        // log configs
        foreach (var c in config)
        {
            Console.WriteLine($"Config: {c.Base} {c.Target}");
        }

        return config;
    }

    public void InsertConfig(HMA1Config config)
    {
        Console.WriteLine("Inserting config");
        _dbContext.Configs.Add(config);
        _dbContext.SaveChanges();
    }
}