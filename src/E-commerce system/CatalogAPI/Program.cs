using MassTransit;
using CatalogAPI.Consumers;
using Microsoft.EntityFrameworkCore;
using CatalogAPI.Services.Interfaces;
using CatalogAPI.Services;
using CatalogAPI.Models.DataBase;
using CatalogAPI.UnitOfWork.Interfaces;
using CatalogAPI.UnitOfWork;
using CatalogAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(x =>
            x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(option => option.UseSqlite("Data Source = Catalog.db"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Issuer"],
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Secret"])),
                });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", builder =>
    {
        builder.RequireClaim(ClaimTypes.Role, "Admin");
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, AuthorizeHandler>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CheckProductsConsumer>();
    x.AddConsumer<GetProductConsumer>();

    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host("rabbitmq://localhost", settings =>
        {
            settings.Username("guest");
            settings.Password("guest");
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<AuthorizeMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
