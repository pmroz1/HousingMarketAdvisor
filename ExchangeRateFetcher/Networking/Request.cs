namespace ExchangeRateFetcher.Networking;

public abstract record Request
{
    public string Url { get; init; }
}