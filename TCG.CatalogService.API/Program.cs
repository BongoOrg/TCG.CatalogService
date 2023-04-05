using MapsterMapper;
using TCG.CatalogService.Application;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Domain;
using TCG.CatalogService.Persitence.DependencyInjection;
using TCG.CatalogService.Persitence.ExternalsApi.PokemonExternalApi.RepositoriesPokemonExternalAPI;
using TCG.Common.Externals;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddMongo().AddMongoRepository<Item>("Items");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExternals<IPokemonExternalRepository, PokemonExternalRepository>();
//builder.Services.AddExternalServices();
builder.Services.AddMapper("PokemonMapping");
builder.Services.AddScoped<IMapper, ServiceMapper>();


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