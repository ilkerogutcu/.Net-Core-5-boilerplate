using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StarterProject.Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using StarterProject.Core.Utilities.IoC;
using StarterProject.Core.Utilities.Messages;

namespace StarterProject.Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    public class FileLogger : LoggerServiceBase
    {
        public FileLogger()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();

            var logConfig =
                configuration.GetSection("SeriLogConfigurations:FileLogConfiguration") as FileLogConfiguration
                ?? throw new Exception(SerilogMessages.NullOptionsMessage);

            var logFilePath = $"{logConfig.FolderPath}/.log";

            Logger = new LoggerConfiguration()
                .WriteTo.File(logFilePath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: null,
                    fileSizeLimitBytes: 5000000,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
                .CreateLogger();
        }
    }
}