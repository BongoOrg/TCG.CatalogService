namespace TCG.CatalogService.Persitence.MongoSettings;

public class MongoDbSettings
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string ConnectionString => $"mongodb://{Host}:{Port}";

}