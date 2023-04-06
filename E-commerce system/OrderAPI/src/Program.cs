using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderAPI.Consumers;
using OrderAPI.Helpers;
using OrderAPI.Services;
using OrderAPI.Services.Interfaces;
using OrderAPI.UnitOfWork;
using OrderAPI.UnitOfWork.Interfaces;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Reflection;
using OrderAPI.DataBase;
using OrderAPI.Models.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Issuer"],
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Secret"])),
                });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Cart", builder =>
    {
        builder.RequireRole(Role.Buyer.ToString());
    });

    options.AddPolicy("LimitedAccessToOrders", builder =>
    {
        builder.RequireRole(Role.Admin.ToString(), Role.Salesman.ToString(), Role.Buyer.ToString());
    });

    options.AddPolicy("FullAccessToOrders", builder =>
    {
        builder.RequireRole(Role.Admin.ToString(), Role.Salesman.ToString());
    });
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateCartConsumer>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host("rabbitmq://localhost", settings =>
        {
            settings.Username("guest");
            settings.Password("guest");
        });

        cfg.ReceiveEndpoint("createCartQueue", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 3000));
            ep.ConfigureConsumer<CreateCartConsumer>(provider);
        });
    }));
});

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(x =>
            x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<Context>(option => option.UseSqlite("Data Source = Order.db"));

builder.Services.AddSingleton<IAuthorizationHandler, AuthorizeHandler>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartProductService, CartProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

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