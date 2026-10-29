using OnePlatform.Clearing.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddClearingApplication();
    // .AddClearingInfrastructure(builder.Configuration); TODO: uncomment later

builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
