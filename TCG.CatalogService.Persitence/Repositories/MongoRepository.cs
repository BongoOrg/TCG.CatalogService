using System.Linq.Expressions;
using MongoDB.Driver;
using TCG.CatalogService.Application.Contracts;

namespace TCG.CatalogService.Persitence.Repositories;

public class MongoRepository<T> : IMongoRepository<T> where T: class
{
    protected readonly IMongoCollection<T> _collection;
    protected virtual string CollectionName => typeof(T).Name + "s"; // Convention : nom de la classe + "s"

    public MongoRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<T>(CollectionName);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _collection.Find(s => true).ToListAsync();
    }

    public Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetAsync(Expression<Func<T, bool>> filter)
    {
        throw new NotImplementedException();
    }

    public async Task CreateAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public Task UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string id)
    {
        throw new NotImplementedException();
    }
    
    public async Task CreateManyAsync(IEnumerable<T> items)
    {
        await _collection.InsertManyAsync(items);
    }
}