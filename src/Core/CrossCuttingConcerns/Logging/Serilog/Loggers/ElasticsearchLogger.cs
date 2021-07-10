using System;
using Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Core.Utilities.IoC;
using Core.Utilities.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    /// <summary>
    /// Elasticsearch Logger
    /// </summary>
    public class ElasticsearchLogger : LoggerServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticsearchLogger"/> class.
        /// </summary>
        public ElasticsearchLogger()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();

            var logConfig = configuration.GetSection("SeriLogConfigurations:ElasticsearchConfiguration")
                .Get<ElasticSearchConfiguration>() ?? throw new Exception(SerilogMessages.NullOptionsMessage);
            Logger = new LoggerConfiguration()
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(
                        new Uri(logConfig.ConnectionString))
                    {
                        CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                        AutoRegisterTemplate = true,
                        TemplateName = logConfig.TemplateName,
                        IndexFormat = logConfig.IndexFormat,
                    })
                .MinimumLevel.Verbose()
                .WriteTo.Seq(logConfig.SeqConnectionString)
                .CreateLogger();
        }
    }
}