using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Domain;
using TCG.CatalogService.Persitence.MongoSettings;
using TCG.CatalogService.Persitence.Repositories;

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

    public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)
        where T : class, IEntity
    {
        //On vient recurperer le service bdd register juste avant car on veut en plus de lui dire dimplementer IRepo
        //que on lui passe le nom de la colelction (demande en param de L'impelemnt (MongoRepo)
        services.AddSingleton<IMongoRepository<T>>(serviceProvider =>
        {
            var database = serviceProvider.GetService<IMongoDatabase>();
            return new MongoRepository<T>(database, collectionName);
        });
        return services;
    }
}