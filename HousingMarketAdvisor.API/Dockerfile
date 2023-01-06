FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HousingMarketAdvisor.API/HousingMarketAdvisor.API.csproj", "HousingMarketAdvisor.API/"]
RUN dotnet restore "HousingMarketAdvisor.API/HousingMarketAdvisor.API.csproj"
COPY . .
WORKDIR "/src/HousingMarketAdvisor.API"
RUN dotnet build "HousingMarketAdvisor.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HousingMarketAdvisor.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HousingMarketAdvisor.API.dll"]
