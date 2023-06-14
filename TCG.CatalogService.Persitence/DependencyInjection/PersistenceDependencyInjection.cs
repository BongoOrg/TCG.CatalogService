using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.CatalogService.Application.IHelpers;
using TCG.CatalogService.Persitence.Helpers;

namespace TCG.CatalogService.Persitence.DependencyInjection
{
    public static class PersistenceDependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddSingleton<IPictureHelper, PictureHelper>();
            return services;
        }
    }
}
