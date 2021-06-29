#region

using Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Core.Utilities.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

#endregion

namespace Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    public class MongoDbLogger : LoggerServiceBase
    {
        public MongoDbLogger()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();

            var logConfig = configuration.GetSection("SeriLogConfigurations:MongoDbConfiguration")
                .Get<MongoDbConfiguration>();
            Logger = new LoggerConfiguration()
                .WriteTo.MongoDB(logConfig.ConnectionString)
                .WriteTo.Seq(logConfig.SeqConnectionString)
                .CreateLogger();
        }
    }
}