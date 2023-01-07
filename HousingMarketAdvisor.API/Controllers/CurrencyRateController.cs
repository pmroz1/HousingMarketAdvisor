using Microsoft.AspNetCore.Mvc;
using Bogus;
using HousingMarketAdvisor.DAL.SqlServer.EfModels;
using HousingMarketAdvisor.DAL.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;


namespace HousingMarketAdvisor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyExchangeRateController : ControllerBase
    {
        private readonly ExchangeRateRepository _context;
        private readonly ILogger<CurrencyExchangeRateController> _logger;

        public CurrencyExchangeRateController(
            ILogger<CurrencyExchangeRateController> logger,
            ExchangeRateRepository context
        )
        {
            _logger = logger;
            _context = context;

            _context.Database.EnsureCreated();
        }

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<CurrencyExchangeRate>>> Get()
        // {
        //     if (!_context.CurrencyExchangeRates.Any())
        //     {
        //         return NotFound();
        //     }
        //
        //     var currencyExchangeRates = await _context.CurrencyExchangeRates.ToListAsync();
        //     return Ok(currencyExchangeRates);
        // }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyExchangeRate>>> Get()
        {
            if (!_context.CurrencyExchangeRates.Any())
            {
                return NotFound();
            }

            // Get all the exchange rates from the database, sorted in descending order by date
            var currencyExchangeRates = await _context.CurrencyExchangeRates
                .OrderByDescending(rate => rate.Date)
                .ToListAsync();

            // Use LINQ to group the exchange rates by base currency, and select the first exchange rate in each group
            var latestRates = currencyExchangeRates
                .GroupBy(rate => rate.BaseCurrency)
                .Select(group => group.First())
                .ToList();

            return Ok(latestRates);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyExchangeRate>> GetCurrencyExchangeRate(int id)
        {
            var currencyExchangeRate = await _context.CurrencyExchangeRates.FindAsync(id);

            var latestRates = _context.CurrencyExchangeRates
                .GroupBy(x => x.BaseCurrency)
                .Select(group => group.OrderByDescending(x => x.Date).FirstOrDefault());

            if (currencyExchangeRate == null)
            {
                return NotFound();
            }

            return Ok(currencyExchangeRate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurrencyExchangeRate(int id, CurrencyExchangeRate currencyExchangeRate)
        {
            try
            {
                var currencyExchangeRateToUpdate = await _context.CurrencyExchangeRates.FindAsync(id);
                if (currencyExchangeRateToUpdate != null)
                {
                    currencyExchangeRateToUpdate.Date = currencyExchangeRate.Date;
                    currencyExchangeRateToUpdate.Rate = currencyExchangeRate.Rate;
                    currencyExchangeRateToUpdate.BaseCurrency = currencyExchangeRate.BaseCurrency;
                    currencyExchangeRateToUpdate.TargetCurrency = currencyExchangeRate.TargetCurrency;
                }

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while updating currency exchange rate");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CurrencyExchangeRate>> DeleteCurrencyExchangeRate(int id)
        {
            var currencyExchangeRate = await _context.CurrencyExchangeRates.FindAsync(id);
            if (currencyExchangeRate == null)
            {
                return NotFound();
            }

            _context.CurrencyExchangeRates.Remove(currencyExchangeRate);
            await _context.SaveChangesAsync();

            return Ok(currencyExchangeRate);
        }

        [HttpPost]
        public async Task<ActionResult<CurrencyExchangeRate>> PostCurrencyExchangeRate(
            CurrencyExchangeRate currencyExchangeRate)
        {
            try
            {
                _context.CurrencyExchangeRates.Add(currencyExchangeRate);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCurrencyExchangeRate", new {id = currencyExchangeRate.Id},
                    currencyExchangeRate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private bool CurrencyExchangeRateExists(int id)
        {
            return _context.CurrencyExchangeRates.Any(e => e.Id == id);
        }
    }
}