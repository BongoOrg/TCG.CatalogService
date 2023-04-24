using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Application.Contracts;

public interface IPokemonExternalRepository
{
    Task<IEnumerable<Item>> GetPokemonCardsExtensionList(string idSet);
    Task<List<List<Item>>> GetAllPokemonCardsBySets();
}