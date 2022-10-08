using Core.Interfaces;
using Core.Settings;
using Infrastructure.Extensions;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;
// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ITenantService, TenantService>();
builder.Services.AddTransient<IProductService,ProductService>();
builder.Services.Configure<TenantSettings>(config.GetSection(nameof(TenantSettings)));
builder.Services.AdddAndMigrateTenantDatabases(config);
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
