using System;

using Microsoft.Extensions.Logging;

namespace Orion.Extensions.Logging.AppInsights
{
    public class LoggerProvider : ILoggerProvider {
        private readonly Func<string, LogLevel, bool> filter;
        private readonly Settings settings;

        public LoggerProvider(Func<string, LogLevel, bool> filter, Settings settings) {
            this.filter = filter;
            this.settings = settings;
        }

        public void Dispose() {
        }

        public ILogger CreateLogger(string categoryName) {
            return new Logger(categoryName, this.filter, this.settings);
        }
    }
}