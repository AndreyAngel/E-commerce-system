using MassTransit;
using CatalogAPI.Consumers;
using Microsoft.EntityFrameworkCore;
using CatalogAPI.Services.Interfaces;
using CatalogAPI.Services;
using CatalogAPI;
using CatalogAPI.Models.DataBase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(x =>
            x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(option => option.UseSqlite("Data Source = Catalog.db"));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped(typeof(IRepositoryService<>), typeof(RepositoryService<>));

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

app.UseAuthorization();

app.MapControllers();

app.Run();
