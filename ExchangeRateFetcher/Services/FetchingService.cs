using ExchangeRateFetcher.Domain.Classes;
using ExchangeRateFetcher.DTO;
using ExchangeRateFetcher.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExchangeRateFetcher.Services;

public class FetchingService
{
    private readonly ILogger<FetchingService> _logger;
    private readonly ExchangeRateRequest _exchangeRateApiUrl;
    private readonly HttpClient _httpClient;

    public FetchingService(ILogger<FetchingService> logger, IConfiguration config, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _logger = logger;
        _exchangeRateApiUrl = new ExchangeRateRequest()
        {
            BaseCurrency = config["ExchangeRateApi:BaseCurrency"],
            TargetCurrency = config["ExchangeRateApi:TargetCurrency"],
            Url = config["ExchangeRateApi:Url"]
        };
    }

    public async Task<string> FetchSingleCurrencyAsync()
    {
        try
        {
            _logger.LogInformation("Fetching exchange rate from {Url}", _exchangeRateApiUrl.ToString());
            var response = await _httpClient.GetStringAsync(_exchangeRateApiUrl.ToString());
            _logger.LogInformation(response);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching exchange rate");
            throw;
        }
    }

    public async Task<List<Dictionary<string, string>>> FetchMultipleCurrenciesAsync(List<HMA1Config> configs)
    {
        try
        {
            var finalDataSet = new List<Dictionary<string, string>>();
            foreach (var config in configs)
            {
                _exchangeRateApiUrl.TargetCurrency = config.Target.ToLower();
                _exchangeRateApiUrl.BaseCurrency = config.Base.ToLower();
                _logger.LogInformation("Fetching exchange rate from {Url}", _exchangeRateApiUrl.ToString());
                var response = await _httpClient.GetStringAsync(_exchangeRateApiUrl.ToString());
                _logger.LogInformation(response);

                var res = JsonConvert.DeserializeObject(response);
                finalDataSet.Add(new Dictionary<string, string>()
                {
                    {config.Base, res.ToString()}
                });
            }

            return finalDataSet;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching exchange rate");
            throw;
        }
    }

    public static void WalkFullObject(string requestResponse)
    {
        dynamic data = JObject.Parse(requestResponse);
        // iterate
        foreach (var jproperty in data)
        {
            // check if nested
            if (jproperty.Name == "pln")
            {
                var pln = jproperty.Value;
                foreach (var item in pln)
                {
                    // round to 2 decimal places
                    var rounded = Math.Round(item.Value, 2);
                    Console.WriteLine("currency: {0} ,value: {1} ", item.Name, item.Value);
                }
            }

            Console.WriteLine("\njproperty.Name = {0}", jproperty.Name);
        }
    }
}