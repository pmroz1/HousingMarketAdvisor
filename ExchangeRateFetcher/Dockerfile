FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ExchangeRateFetcher/ExchangeRateFetcher.csproj", "ExchangeRateFetcher/"]
RUN dotnet restore "ExchangeRateFetcher/ExchangeRateFetcher.csproj"
COPY . .
WORKDIR "/src/ExchangeRateFetcher"
RUN dotnet build "ExchangeRateFetcher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeRateFetcher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRateFetcher.dll"]
