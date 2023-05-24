using System.Linq.Expressions;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Application.Contracts;

public interface IMongoRepositoryExtension : IMongoRepository<Extension>
{
    Task<Extension> GetAsync(string id);
}