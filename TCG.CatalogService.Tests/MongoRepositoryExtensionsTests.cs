using MongoDB.Driver;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Tests
{
    [TestClass]
    public class MongoRepositoryExtensionsTests
    {
        private IMongoDatabase _database;
        private IMongoCollection<Extension> _collection;

        [TestInitialize]
        public void Initialize()
        {
            var connectionString = "mongodb://admincsc:ZqJt6bjmL6JsIiQ1@51.145.249.188:27017";

            var client = new MongoClient(connectionString);

            var databaseName = "PokemonCollection";
            _database = client.GetDatabase(databaseName);

            // Récupération de la collection / Création d'une collection pour les tests si elle n'existe pas
            var collectionName = "Extensions";
            _collection = _database.GetCollection<Extension>(collectionName);

        }

        [TestMethod]
        public async Task ShouldReturnCorrectNumberOfExtensions()
        {
            var items = await _collection.Find(s => true).ToListAsync();

            // Assert
            Assert.IsNotNull(items);
            Assert.AreEqual(148, items.Count());
        }

    }
}
