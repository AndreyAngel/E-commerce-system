using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOcelot();
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddSwaggerForOcelot(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var kestrelSettings = builder.Configuration.GetSection("Kestrel");
var kestrelLimits = kestrelSettings.GetSection("Limits");
var kestrelEndPoints = kestrelSettings.GetSection("EndPoints");

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxConcurrentConnections = int.Parse(kestrelLimits["MaxConcurrentConnections"]);

    options.Limits.MaxRequestBodySize = int.Parse(kestrelLimits["MaxRequestBodySizeInBytes"]);

    options.Limits.MinRequestBodyDataRate = new MinDataRate(

        bytesPerSecond: double.Parse(kestrelLimits["MinRequestBodyDataRate:BytesPerSecond"]),
        gracePeriod: TimeSpan.FromSeconds(double.Parse(
            kestrelLimits["MinRequestBodyDataRate:GracePeriodInSeconds"])));

    options.Limits.MinResponseDataRate = new MinDataRate(

        bytesPerSecond: double.Parse(kestrelLimits["MinResponseDataRate:BytesPerSecond"]),
        gracePeriod: TimeSpan.FromSeconds(double.Parse(kestrelLimits["MinResponseDataRate:GracePeriodInSeconds"])));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerForOcelotUI(builder.Configuration);
    app.UseOcelot().Wait();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
