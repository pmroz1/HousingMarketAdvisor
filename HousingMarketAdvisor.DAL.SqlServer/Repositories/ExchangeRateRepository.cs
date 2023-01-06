using HousingMarketAdvisor.DAL.SqlServer.EfModels;
using Microsoft.EntityFrameworkCore;

namespace HousingMarketAdvisor.DAL.SqlServer.Repositories;

public class ExchangeRateRepository : DbContext
{
    public ExchangeRateRepository(DbContextOptions<ExchangeRateRepository> options) : base(options)
    {
    }

    public DbSet<CurrencyExchangeRate> CurrencyExchangeRates { get; set; }
}