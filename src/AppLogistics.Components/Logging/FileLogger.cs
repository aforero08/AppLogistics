using AppLogistics.Components.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;

namespace AppLogistics.Components.Logging;

public class FileLogger : ILogger
{
    private long RollSize { get; }
    private string LogPath { get; }
    private LogLevel Level { get; }
    private string LogDirectory { get; }
    private Func<int?> AccountId { get; }
    private string RollingFileFormat { get; }
    private static object LogWriting { get; } = new object();

    private sealed class DisposableScope : IDisposable { public static readonly DisposableScope Instance = new DisposableScope(); public void Dispose() { } }

    public FileLogger(string path, LogLevel logLevel, long rollSize)
    {
        IHttpContextAccessor accessor = new HttpContextAccessor();
        string file = Path.GetFileNameWithoutExtension(path);
        string extension = Path.GetExtension(path);
        LogDirectory = Path.GetDirectoryName(path);

        RollingFileFormat = $"{file}-{{0:yyyyMMdd-HHmmss}}{extension}";
        AccountId = () => accessor.HttpContext?.User.Id();
        RollSize = rollSize;
        Level = logLevel;
        LogPath = path;
    }

    public bool IsEnabled(LogLevel logLevel) => Level <= logLevel;

    public IDisposable BeginScope<TState>(TState state) => DisposableScope.Instance;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;
        if (formatter == null) return;

        StringBuilder log = new StringBuilder();
        log.AppendLine($"{logLevel.ToString().PadRight(11)}: {DateTime.Now:yyyy-MM-dd HH:mm:ss.ffffff} [{AccountId()}]");
        log.AppendLine($"Message    : {formatter(state, exception)}");

        if (exception != null)
        {
            log.AppendLine("Stack trace:");
        }

        Exception current = exception;
        while (current != null)
        {
            log.AppendLine($"    {current.GetType()}: {current.Message}");
            if (!string.IsNullOrEmpty(current.StackTrace))
            {
                foreach (string line in current.StackTrace.Split('\n'))
                    log.AppendLine("     " + line.TrimEnd('\r'));
            }
            current = current.InnerException;
        }

        log.AppendLine();

        lock (LogWriting)
        {
            Directory.CreateDirectory(LogDirectory);
            File.AppendAllText(LogPath, log.ToString());
            if (RollSize > 0 && new FileInfo(LogPath).Length >= RollSize)
            {
                File.Move(LogPath, Path.Combine(LogDirectory, string.Format(RollingFileFormat, DateTime.Now)));
            }
        }
    }
}
