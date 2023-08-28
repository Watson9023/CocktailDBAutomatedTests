using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CocktailDBTests.Logging
{
    public class TestLogger : ILogger
    {
        private readonly List<LogEntry> _capturedLogs;

        public TestLogger(List<LogEntry> capturedLogs)
        {
            _capturedLogs = capturedLogs;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            var logEntry = new LogEntry
            {
                LogLevel = logLevel,
                Message = message,
                Exception = exception
            };
            _capturedLogs.Add(logEntry);

            // Print the JSON representation of the log entry
            Console.WriteLine($"Log Entry: {logEntry.ToJsonString()}");
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}