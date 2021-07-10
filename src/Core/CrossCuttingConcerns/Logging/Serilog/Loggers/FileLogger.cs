using Serilog;
using System;
using System.IO;
using Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Core.Utilities.IoC;
using Core.Utilities.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    /// <summary>
    /// File logger
    /// </summary>
    public class FileLogger : LoggerServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
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