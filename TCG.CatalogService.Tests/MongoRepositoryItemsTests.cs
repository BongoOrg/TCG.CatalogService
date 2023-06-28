using MongoDB.Driver;
using TCG.CatalogService.Domain;

namespace TCG.CatalogService.Tests
{
    [TestClass]
    public class MongoRepositoryItemsTests
    {
        private IMongoDatabase _database;
        private IMongoCollection<Item> _collection;

        [TestInitialize]
        public void Initialize()
        {
            var connectionString = "mongodb://admincsc:ZqJt6bjmL6JsIiQ1@51.145.249.188:27017";

            var client = new MongoClient(connectionString);

            var databaseName = "PokemonCollection";
            _database = client.GetDatabase(databaseName);

            var collectionName = "Items";
            _collection = _database.GetCollection<Item>(collectionName);
        }

        [TestMethod]
        public async Task ShouldReturnCorrectNumberOfItems()
        {
            var items = await _collection.Find(s => true).ToListAsync();

            // Assert
            Assert.IsNotNull(items);
            Assert.AreEqual(14354, items.Count());
        }
    }
}