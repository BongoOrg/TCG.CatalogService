using MapsterMapper;
using TCG.Common.Middlewares.MiddlewareException;
using TCG.CatalogService.Application;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Persistence.DependencyInjection;
using TCG.CatalogService.Persistence.ExternalsApi.PokemonExternalApi.RepositoriesPokemonExternalAPI;
using TCG.Common.Externals;
using TCG.Common.Versioning.SwaggerConfig;
using Asp.Versioning.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;
using Asp.Versioning;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder
                            .WithOrigins("*") // specifying the allowed origin
                            .WithMethods("GET", "POST", "PUT") // defining the allowed HTTP method
                            .AllowAnyHeader(); // allowing any header to be sent
                      });
});

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddMongoPersistence(builder.Configuration);
builder.Services
    .AddApiVersioning(options =>
    {
        //indicating whether a default version is assumed when a client does
        // does not provide an API version.
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.DefaultApiVersion = new ApiVersion(1.0);
    })
    .AddApiExplorer(options =>
    {
        // Add the versioned API explorer, which also adds IApiVersionDescriptionProvider service
        // note: the specified format code will format the version as "'v'major[.minor][-status]"
        options.GroupNameFormat = "'v'VVV";

        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
        // can also be used to control the format of the API version in route templates
        options.SubstituteApiVersionInUrl = true;

        //indicating whether a default version is assumed when a client does
        // does not provide an API version.
        options.AssumeDefaultVersionWhenUnspecified = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>>(provider =>
    new ConfigureSwaggerOptions(provider.GetRequiredService<IApiVersionDescriptionProvider>(), "CatalogService"));
builder.Services.AddSwaggerGen(options =>
{
    // Add a custom operation filter which sets default values
    options.OperationFilter<SwaggerDefaultValues>();
});
builder.Services.AddExternals<IPokemonExternalRepository, PokemonExternalRepository>();
builder.Services.AddMapper("PokemonMapping");
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddPersistence();
builder.Services.AddMassTransitWithRabbitMQQQQ();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        // Build a swagger endpoint for each discovered API version
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

//app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.ConfigureCustomExceptionMiddleware();

app.Run();