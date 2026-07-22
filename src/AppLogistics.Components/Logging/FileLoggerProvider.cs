using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace AppLogistics.Components.Logging;

public class FileLoggerProvider : ILoggerProvider
{
    private ILogger Logger { get; }

    public FileLoggerProvider(IConfiguration config)
    {
        LogLevel logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), config["Logging:File:LogLevel:Default"]);
        string path = Path.Combine(config["Application:Path"], config["Logging:File:Path"]);
        long rollSize = long.Parse(config["Logging:File:RollSize"]);

        Logger = new FileLogger(path, logLevel, rollSize);
    }

    public ILogger CreateLogger(string categoryName)
    {
        return Logger;
    }

    public void Dispose()
    {
    }
}
