using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using TCG.CatalogService.Application.Contracts;
using TCG.Common.Settings;

namespace TCG.CatalogService.Persitence.DependencyInjection;

public static class AddMongoRepo
{
    //Register the IMongoDatabse Instance
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        //On enregistre notre bdd avec localhost + port + création bdd avec Catalog en nom
        services.AddSingleton(sericeProvider =>
        {
            var config = sericeProvider.GetService<IConfiguration>();
            //On récupère le nom de la table Catalog
            
            var serviceSettigns = config.GetSection("ServiceSettings").Get<ServiceSettings>();
            var mongoDbSettings = config.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            return mongoClient.GetDatabase(serviceSettigns.ServiceName);
        });
        return services;
    }

    public static IServiceCollection AddMongoPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Enregistrer IMongoDatabase
        services.AddSingleton(serviceProvider =>
        {
            var mongoDbSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            var serviceSettigns = configuration.GetSection("ServiceSettings").Get<ServiceSettings>();
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
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