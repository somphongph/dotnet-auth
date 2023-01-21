namespace Infrastructure.Persistence
{
    public class MongoSettings : IMongoSettings
    {
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
    }
}