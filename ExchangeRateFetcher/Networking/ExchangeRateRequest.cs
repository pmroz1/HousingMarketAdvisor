using System.Text;

namespace ExchangeRateFetcher.Networking;

public record ExchangeRateRequest : Request
{
    public string BaseCurrency { set; get; }
    public string TargetCurrency { set; get; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(Url);
        sb.Append(BaseCurrency);
        sb.Append('/');
        sb.Append(TargetCurrency + ".min.json");
        return sb.ToString();
    }

    public string ToFullRequest()
    {
        return Url + TargetCurrency + ".min.json";
    }
}