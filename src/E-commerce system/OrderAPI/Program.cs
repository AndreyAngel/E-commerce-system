using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI;
using OrderAPI.Models.DataBase;
using OrderAPI.Services;
using OrderAPI.Services.Interfaces;
using OrderAPI.UnitOfWork;
using OrderAPI.UnitOfWork.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(x =>
            x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(option => option.UseSqlite("Data Source = Order.db"));

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartProductService, CartProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddMassTransit(x =>
{
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        config.Host("rabbitmq://localhost", settings =>
        {
            settings.Username("guest");
            settings.Password("guest");
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
