using ExchangeRateFetcher;
using ExchangeRateFetcher.Services;
using Serilog;

await using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    log.Information("Starting up");

    var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
        {
            services.AddSingleton<DbService>();
            services.AddHttpClient<FetchingService>();
            services.AddHostedService<Worker>();
        })
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    log.Information(ex.Message);
}
finally
{
    Log.CloseAndFlush();
}