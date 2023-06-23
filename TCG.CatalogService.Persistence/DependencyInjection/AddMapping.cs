using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using TCG.CatalogService.Domain;
using TCG.CatalogService.Persistence.ExternalsApi.ModelsExternals;

namespace TCG.CatalogService.Persistence.DependencyInjection;

public static class AddMapping
{
    public static IServiceCollection AddMapper(this IServiceCollection services, string configName)
    {
        var config = new TypeAdapterConfig();

        if (configName == "PokemonMapping")
        {
            config.NewConfig<PokemonCardFromJson, Item>()
                .Map(dest => dest.IdCard, src => src.Id)
                .Map(dest => dest.Language, src => "fr");
        }
        else if (configName == "OtherMapping")
        {
            // autres configurations de mapping ici
        }

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}