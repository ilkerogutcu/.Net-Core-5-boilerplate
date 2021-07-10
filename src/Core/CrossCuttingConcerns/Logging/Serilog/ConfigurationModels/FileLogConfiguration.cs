namespace Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels
{
    /// <summary>
    /// File log configuration
    /// </summary>
    public class FileLogConfiguration
    {
        public string FolderPath { get; set; }

        public string SeqConnectionString { get; set; }
    }
}