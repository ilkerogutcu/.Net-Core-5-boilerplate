namespace Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels
{
    /// <summary>
    /// Mongo DB Configuration
    /// </summary>
    public abstract class MongoDbConfiguration
    {
        public string ConnectionString { get; set; }

        public string SeqConnectionString { get; set; }
    }
}