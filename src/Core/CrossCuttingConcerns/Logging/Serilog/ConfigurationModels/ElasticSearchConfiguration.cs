namespace Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels
{
    /// <summary>
    /// Elasticsearch Configuration
    /// </summary>
    public abstract class ElasticSearchConfiguration
    {
        public string ConnectionString { get; set; }

        public string TemplateName { get; set; }

        public string IndexFormat { get; set; }

        public string SeqConnectionString { get; set; }
    }
}