using HousingMarketAdvisor.DAL.SqlServer.EfModels;
using Microsoft.EntityFrameworkCore;

namespace HousingMarketAdvisor.DAL.SqlServer.Repositories;

public class HousingRepository : DbContext
{
    public HousingRepository(DbContextOptions<HousingRepository> options)
        : base(options)
    {
    }

    public DbSet<HousingOffer> HousingOffers { get; set; }
}