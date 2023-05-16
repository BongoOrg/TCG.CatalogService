using Moq;
using TCG.CatalogService.Persitence.ExternalsApi.PokemonExternalApi.RepositoriesPokemonExternalAPI;

namespace TCG.CatalogService.Tests.Persitence.ExternalsApi.PokemonExternalApi.RepositoriesPokemonExternalAPI
{
    [TestClass]
    public class PokemonExternalRepositoryTests
    {
        private PokemonExternalRepository _pokemonExternalRepository;
        private Mock<IHttpClientFactory> _httpClientFactoryMock;

        [TestInitialize]
        public void Initialize()
        {
            // Cr�er un mock pour IHttpClientFactory
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();

            // Cr�er une instance de PokemonExternalRepository en injectant le mock
            _pokemonExternalRepository = new PokemonExternalRepository(
                mapper: null, // Remplacez null par une instance r�elle de IMapper si n�cessaire
                httpClientFactory: _httpClientFactoryMock.Object
            );
        }

        [TestMethod]
        public async Task GetPokemonExtensionList_ReturnsListOfExtensions()
        {
            // Ex�cuter la m�thode � tester
            var result = await _pokemonExternalRepository.GetPokemonExtensionList();
            Assert.AreEqual(148, result.Count());
        }
    }
}
