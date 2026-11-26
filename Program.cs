using System.Text.Json.Serialization;
using OnePlatform.Clearing;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddClearingApplication()
    .AddClearingInfrastructure(builder.Configuration);

builder.Services.AddOpenApi();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
