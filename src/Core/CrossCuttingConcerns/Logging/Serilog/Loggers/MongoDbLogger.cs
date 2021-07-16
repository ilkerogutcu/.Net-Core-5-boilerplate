using Serilog;
using Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Core.Utilities.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    /// <summary>
    /// Mongo DB Logger
    /// </summary>
    public class MongoDbLogger : LoggerServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbLogger"/> class.
        /// </summary>
        public MongoDbLogger()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();

            var logConfig = configuration.GetSection("SeriLogConfigurations:MongoDbConfiguration").Get<MongoDbConfiguration>();
            Logger = new LoggerConfiguration()
                .WriteTo.MongoDB(logConfig.ConnectionString)
                .WriteTo.Seq(logConfig.SeqConnectionString)
                .CreateLogger();
        }
    }
}