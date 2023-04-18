using Microsoft.EntityFrameworkCore;
using OrderService.AsyncDataServices;
using OrderService.Data;
using OrderService.EventProcessing;
using OrderService.SyncDataServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opts =>
    opts.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));

// untuk memanggil dan menyingkronkan data
builder.Services.AddHttpClient<IProductDataClient, HttpProductDataClient>();
builder.Services.AddScoped<IProductOrderRepo, ProductOrderRepo>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHostedService<MessageBusSubscriber>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
