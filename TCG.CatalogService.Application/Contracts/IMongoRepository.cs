using System.Linq.Expressions;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Application.Contracts;

public interface IMongoRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    //Task<T> GetAsync(string id);
    Task<T> GetAsync(Expression<Func<T, bool>> filter);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task RemoveAsync(string id);
    Task CreateManyAsync(IEnumerable<T> items);

}