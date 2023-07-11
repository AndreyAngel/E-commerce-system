using GatewayAPI.Authorization;
using GatewayAPI.Consumers;
using GatewayAPI.DataBase;
using GatewayAPI.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
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

var dataBaseConnection = builder.Configuration.GetConnectionString("DataBaseConnection");
builder.Services.AddDbContext<Context>(option => option.UseSqlite(dataBaseConnection));

builder.Services.AddScoped<ITokenService, TokenService>();

var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQ");
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AddTokenConsumer>();
    x.AddConsumer<DeleteTokenConsumer>();

    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(rabbitMQSettings["Host"], settings =>
        {
            settings.Username(rabbitMQSettings["Username"]);
            settings.Password(rabbitMQSettings["Password"]);
        });
        cfg.ReceiveEndpoint("addTokenQueue", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 3000));
            ep.ConfigureConsumer<AddTokenConsumer>(provider);
        });
        cfg.ReceiveEndpoint("deleteTokenQueue", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 3000));
            ep.ConfigureConsumer<DeleteTokenConsumer>(provider);
        });
    }));
});

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

app.UseMiddleware<CustomAuthenticateMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerForOcelotUI(builder.Configuration);
}

app.UseOcelot().Wait();

app.Run();
