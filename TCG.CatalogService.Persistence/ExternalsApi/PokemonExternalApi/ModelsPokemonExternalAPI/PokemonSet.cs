namespace TCG.CatalogService.Persistence.ExternalsApi.ModelsExternals;

public class PokemonSet
{
    public string Id { get; set; }
    public IEnumerable<PokemonCardFromJson> Cards { get; set; }
}