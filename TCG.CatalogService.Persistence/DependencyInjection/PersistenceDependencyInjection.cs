using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TCG.CatalogService.Application.IHelpers;
using TCG.CatalogService.Persistence.ExternalsApi.PokemonExternalApi;
using TCG.CatalogService.Persistence.Helpers;
using TCG.Common.Settings;

namespace TCG.CatalogService.Persistence.DependencyInjection
{
    public static class PersistenceDependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AWSSettings>(configuration.GetSection("AWSSettings"));
            services.AddSingleton<IPictureHelper, PictureHelper>();
            services.AddSingleton<IPokemonExtApiHelper, PokemonExtApiHelper>();
            return services;
        }
    }
}
