using MongoDB.Driver;

namespace Core.DataAccess.MongoDb.Configuration
{
    public abstract class MongoDatabaseConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}