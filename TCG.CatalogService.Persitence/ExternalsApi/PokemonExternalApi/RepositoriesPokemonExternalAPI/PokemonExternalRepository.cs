using Newtonsoft.Json;
using TCG.CatalogService.Application.Contracts;
using TCG.CatalogService.Domain;
using TCG.CatalogService.Persitence.ExternalsApi.ModelsExternals;
using MapsterMapper;

namespace TCG.CatalogService.Persitence.ExternalsApi.PokemonExternalApi.RepositoriesPokemonExternalAPI;

public class PokemonExternalRepository : IPokemonExternalRepository
{
    private readonly IMapper _mapper;
    private readonly IHttpClientFactory _httpClientFactory;

    public PokemonExternalRepository(IMapper mapper, IHttpClientFactory httpClientFactory)
    {
        _mapper = mapper;
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IEnumerable<Item>> GetPokemonCardsExtensionList(string idSet)
    {
        List<Item> pokemons = new List<Item>();
        try
        {
            PokemonSet pokemonSet = new PokemonSet();
            HttpClient client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync($"https://api.tcgdex.net/v2/fr/sets/{idSet}");
            var responseContent = await response.Content.ReadAsStringAsync();
            pokemonSet = JsonConvert.DeserializeObject<PokemonSet>(responseContent);
            pokemons = pokemonSet.Cards.Select(p =>
            {
                var item = _mapper.Map<PokemonCardFromJson, Item>(p);
                item.IdExtension = pokemonSet.Id;
                return item;
            }).ToList();
            return pokemons;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    #region GetAllPokemonCardsBySets
    public async Task<List<List<Item>>> GetAllPokemonCardsBySets()
    {
        List<List<Item>> pokemonForDb = new List<List<Item>>();

        HttpClient _client = new HttpClient();
        HttpResponseMessage response = await _client.GetAsync($"https://api.tcgdex.net/v2/fr/sets/");
        var responseContent = await response.Content.ReadAsStringAsync();
        var pokemonSets = JsonConvert.DeserializeObject<List<PokemonSet>>(responseContent);
        foreach (var set in pokemonSets)
        {
            pokemonForDb.Add((await GetPokemonCardsExtensionList(set.Id)).ToList());
        }

        return pokemonForDb;
    }
    #endregion

    internal class CardListDesrializer
    {
        public string id { get; set; }
        public List<PokemonCardFromJson> cards { get; set; }
    }
}