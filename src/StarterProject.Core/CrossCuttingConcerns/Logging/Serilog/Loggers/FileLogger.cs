using System;
using System.IO;
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

            var logConfig = configuration.GetSection("SeriLogConfigurations:FileLogConfiguration")
                .Get<FileLogConfiguration>() ?? throw new Exception(SerilogMessages.NullOptionsMessage);

            var logFilePath = $"{Directory.GetCurrentDirectory() + logConfig.FolderPath}/{DateTime.Now:yyyy-MM-dd}.txt";
            Logger = new LoggerConfiguration()
                .WriteTo.File(logFilePath,
                    retainedFileCountLimit: 1,
                    fileSizeLimitBytes: 5000000,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
                .WriteTo.Seq(logConfig.SeqConnectionString)
                .CreateLogger();
        }
    }
}