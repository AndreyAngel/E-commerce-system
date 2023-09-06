using CatalogAPI.Consumers;
using CatalogAPI.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.ResponseCompression;
using CatalogAPI.Domain.Repositories.Implementation;
using CatalogAPI.Contracts.Enums;
using CatalogAPI.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CatalogAPI.UseCases;
using CatalogAPI.UseCases.GetProductList;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(x =>
            x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();

var dataBaseConnection = builder.Configuration.GetConnectionString("DataBaseConnection");
builder.Services.AddDbContext<Context>(option => option.UseSqlite(dataBaseConnection));

builder.Services.AddMemoryCache();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;

    options.ExcludedMimeTypes = new[] { "text/plain" };

    options.Providers.Add(new GzipCompressionProvider(new GzipCompressionProviderOptions()));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Authentication:Issuer"],
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Secret"])),
                });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Public", builder =>
    {
        builder.RequireRole(
            Role.Admin.ToString(),
            Role.Salesman.ToString(),
            Role.Courier.ToString(),
            Role.Buyer.ToString());
    });

    options.AddPolicy("ChangingOfCatalog", builder =>
    {
        builder.RequireRole(Role.Admin.ToString() , Role.Salesman.ToString());
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, AuthorizeHandler>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProductListQuery).Assembly));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQ");
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CheckProductsConsumer>();
    x.AddConsumer<GetProductConsumer>();

    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(rabbitMQSettings["Host"], settings =>
        {
            settings.Username(rabbitMQSettings["Username"]);
            settings.Password(rabbitMQSettings["Password"]);
        });
        cfg.ReceiveEndpoint("checkProductsQueue", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 3000));
            ep.ConfigureConsumer<CheckProductsConsumer>(provider);
        });
        cfg.ReceiveEndpoint("getProductQueue", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 3000));
            ep.ConfigureConsumer<GetProductConsumer>(provider);
        });
    }));
});

builder.Services.AddSwaggerGen(options =>
{
    var basePath = AppContext.BaseDirectory;
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(basePath, xmlFile);
    options.IncludeXmlComments(xmlPath);
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Enter access token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
          },
          new List<string>()
        }
    });
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

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
