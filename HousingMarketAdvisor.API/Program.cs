using HousingMarketAdvisor.DAL.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddDbContext<HousingRepository>(options =>
//     options.UseInMemoryDatabase("HousingMarketAdvisor"));

builder.Services.AddDbContext<HousingRepository>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HMA1")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// builder.Services.AddDbContext<ExchangeRateRepository>(options =>
//     options.UseInMemoryDatabase("HousingMarketAdvisor"));

builder.Services.AddDbContext<ExchangeRateRepository>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HMA2")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


var app = builder.Build();


// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();


app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();