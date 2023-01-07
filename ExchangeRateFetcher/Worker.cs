using ExchangeRateFetcher.Domain.Classes;
using ExchangeRateFetcher.DTO;
using ExchangeRateFetcher.Services;
using Newtonsoft.Json;


namespace ExchangeRateFetcher;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly FetchingService _fetchingService;
    private readonly DbService _databaseService;

    public Worker(ILogger<Worker> logger, FetchingService fetchingService, DbService dbService)
    {
        _logger = logger;
        _fetchingService = fetchingService;
        _databaseService = dbService;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            // var requestResponse =
            //     JsonConvert.DeserializeObject<ExchangeRateDto>(await _fetchingService.FetchSingleCurrencyAsync());

            try
            {
                _databaseService.InsertConfig(new HMA1Config
                {
                    Base = "CAD",
                    Target = "PLN"
                });

                _databaseService.InsertConfig(new HMA1Config
                {
                    Base = "EUR",
                    Target = "PLN"
                });

                _databaseService.InsertConfig(new HMA1Config
                {
                    Base = "CHF",
                    Target = "PLN"
                });

                _databaseService.InsertConfig(new HMA1Config
                {
                    Base = "NOK",
                    Target = "PLN"
                });

                _databaseService.InsertConfig(new HMA1Config
                {
                    Base = "GBP",
                    Target = "PLN"
                });

                var configs = _databaseService.GetConfig();

                var result = await _fetchingService.FetchMultipleCurrenciesAsync(configs);

                // show result
                _logger.LogInformation(result.ToString());

                foreach (var rate in result)
                {
                    // get property name from second field in string json in value
                    var propertyName = rate.Values.First().Split(',')[1].Split(':')[0].Replace("\"", "");


                    _logger.LogInformation("Rate: {result}", rate);
                    _databaseService.InsertExchangeRate(new CurrencyExchangeRate()
                    {
                        BaseCurrency = rate.Keys.First(),
                        TargetCurrency = propertyName,
                        Date = DateTime.Now,
                        Rate = JsonConvert.DeserializeObject<ExchangeRateDto>(rate.Values.First()).pln
                    });
                }


                // _databaseService.InsertExchangeRate(new CurrencyExchangeRate()
                // {
                //     BaseCurrency = "EUR",
                //     TargetCurrency = "PLN",
                //     Date = DateTime.Now,
                //     Rate = requestResponse.pln
                // });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            _logger.LogInformation("Worker running at: {time} finished", DateTimeOffset.Now);
            await Task.Delay(60000, stoppingToken);
        }
    }
}