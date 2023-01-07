namespace ExchangeRateFetcher.Domain.Classes;

public class CurrencyExchangeRate
{
    public int Id { get; set; }

    // [JsonPropertyName("date")] 
    public DateTime Date { get; set; }

    // [JsonPropertyName("pln")] 
    public double Rate { get; set; }

    public string? BaseCurrency { get; set; }

    public string? TargetCurrency { get; set; }
}