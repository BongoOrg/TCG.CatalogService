using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Application.Contracts;

public interface IMongoRepositoryItem : IMongoRepository<Item>
{
    Task<Item> GetAsync(string id);
}