using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StarterProject.Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using StarterProject.Core.Utilities.IoC;
using StarterProject.Core.Utilities.Messages;

namespace StarterProject.Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    public class MongoDbLogger : LoggerServiceBase
    {
        public MongoDbLogger()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            
            var logConfig =
                configuration.GetSection("SeriLogConfigurations:MongoDbConfiguration") as MongoDbConfiguration
                ?? throw new Exception(SerilogMessages.NullOptionsMessage);
            
            Logger = new LoggerConfiguration()
                .WriteTo.MongoDB(logConfig.ConnectionString)
                .WriteTo.Seq(logConfig.SeqConnectionString)
                .CreateLogger();
        }
    }
}