using MongoDB.Driver;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Persistence.Repositories;

public class MongoRepositoryItem : MongoRepository<Item>, IMongoRepositoryItem
{
    public MongoRepositoryItem(IMongoDatabase database) : base(database)
    {
    }

    public async Task<Item> GetAsync(string id)
    {
        return await _collection.Find(x => x.IdCard == id).FirstOrDefaultAsync();
    }
}