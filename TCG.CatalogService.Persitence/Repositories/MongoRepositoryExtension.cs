using MongoDB.Driver;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Persitence.Repositories;

public class MongoRepositoryExtension : MongoRepository<Extension>, IMongoRepositoryExtension
{
    public MongoRepositoryExtension(IMongoDatabase database) : base(database)
    {
    }

    public async Task<Extension> GetAsync(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
}