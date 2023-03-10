using Microsoft.EntityFrameworkCore;
using RedisDemo.Repositories;
using RedisDemo.Repositories.Interfaces;
using DbContext = RedisDemo.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Entity Framework Core
builder.Services.AddDbContext<DbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDB"));
});

// Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "redis-master:6379, redis-slave:6380";
});

builder.Services.AddScoped<IProductsRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
