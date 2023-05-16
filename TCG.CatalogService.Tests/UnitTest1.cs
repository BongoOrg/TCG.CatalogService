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
            // Créer un mock pour IHttpClientFactory
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();

            // Créer une instance de PokemonExternalRepository en injectant le mock
            _pokemonExternalRepository = new PokemonExternalRepository(
                mapper: null, // Remplacez null par une instance réelle de IMapper si nécessaire
                httpClientFactory: _httpClientFactoryMock.Object
            );
        }

        [TestMethod]
        public async Task GetPokemonExtensionList_ReturnsListOfExtensions()
        {
            // Exécuter la méthode à tester
            var result = await _pokemonExternalRepository.GetPokemonExtensionList();
            Assert.AreEqual(148, result.Count());
        }
    }
}
