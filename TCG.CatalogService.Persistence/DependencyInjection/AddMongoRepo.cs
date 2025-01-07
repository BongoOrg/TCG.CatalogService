using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using TCG.CatalogService.Application.Contracts;
using TCG.Common.Settings;

namespace TCG.CatalogService.Persistence.DependencyInjection;

public static class AddMongoRepo
{
    //Register the IMongoDatabse Instance
    public static IServiceCollection AddMongoPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Enregistrer IMongoDatabase
        services.AddSingleton(serviceProvider =>
        {
            var mongoDbSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            var serviceSettigns = configuration.GetSection("ServiceSettings").Get<ServiceSettings>();
            var mongoClient = new MongoClient($"mongodb://{mongoDbSettings.Host}:{mongoDbSettings.Port}/?ssl=false");
            return mongoClient.GetDatabase(serviceSettigns.ServiceName);
        });

        // Enregistrer automatiquement tous les MongoRepository
        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(c => c.AssignableTo(typeof(IMongoRepository<>)))
            .AsMatchingInterface()
            .WithScopedLifetime());

        return services;
    }

}