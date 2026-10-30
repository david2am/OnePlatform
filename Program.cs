using OnePlatform.Clearing.Application;
using OnePlatform.Clearing.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddClearingApplication()
    .AddClearingInfrastructure();

builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
