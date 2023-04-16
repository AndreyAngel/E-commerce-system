using DeliveryAPI.Consumers;
using DeliveryAPI.DataBase;
using DeliveryAPI.Helpers;
using DeliveryAPI.Models;
using DeliveryAPI.Services;
using DeliveryAPI.UnitOfWork;
using DeliveryAPI.UnitOfWork.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers().AddNewtonsoftJson(x =>
            x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

var dataBaseConnection = builder.Configuration.GetConnectionString("DataBaseConnection");
builder.Services.AddDbContext<Context>(option => option.UseSqlite(dataBaseConnection));

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
    options.AddPolicy("Get information by delivery", builder =>
    {
        builder.RequireRole(Role.Admin.ToString(), Role.Courier.ToString());
    });

    options.AddPolicy("Creating delivery", builder =>
    {
        builder.RequireRole(Role.Admin.ToString(), Role.Buyer.ToString());
    });

    options.AddPolicy("Courier", builder =>
    {
        builder.RequireRole(Role.Buyer.ToString());
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, AuthorizeHandler>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<ICourierService, CourierService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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

var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQ");
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateCourierConsumer>();
    x.AddConsumer<CreateDeliveryConsumer>();

    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(rabbitMQSettings["Host"], settings =>
        {
            settings.Username(rabbitMQSettings["Username"]);
            settings.Password(rabbitMQSettings["Password"]);
        });
        cfg.ReceiveEndpoint("createCourierQueue", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 3000));
            ep.ConfigureConsumer<CreateCourierConsumer>(provider);
        });
        cfg.ReceiveEndpoint("createDeliveryQueue", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 3000));
            ep.ConfigureConsumer<CreateDeliveryConsumer>(provider);
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<CustomAuthenticateMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
