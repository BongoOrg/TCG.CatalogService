using SharpCompress.Compressors.PPMd;

namespace TCG.CatalogService.Persistence.ExternalsApi.ModelsExternals;

public class PokemonCardFromJson
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string Language { get; set; }
}