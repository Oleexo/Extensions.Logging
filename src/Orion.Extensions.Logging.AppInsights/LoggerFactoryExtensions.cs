using System;
using Microsoft.Extensions.Logging;

namespace Orion.Extensions.Logging.AppInsights {
    public static class LoggerFactoryExtensions {
        public static ILoggerFactory AddAppInsights(this ILoggerFactory loggerFactory, string instrumentationKey,
            bool developerMode = false) {
            return loggerFactory.AddAppInsights((category, logLevel) => logLevel >= LogLevel.Information, instrumentationKey, developerMode);
        }

        public static ILoggerFactory AddAppInsights(this ILoggerFactory loggerFactory, LogLevel minLevel, string instrumentationKey, bool developerMode = false) {
            return loggerFactory.AddAppInsights((category, logLevel) => logLevel >= minLevel, instrumentationKey, developerMode);
        }

        public static ILoggerFactory AddAppInsights(this ILoggerFactory loggerFactory, Func<string, LogLevel, bool> filter, string instrumentationKey, bool developerMode = false)
        {
            if (string.IsNullOrEmpty(instrumentationKey))
                throw new ArgumentException(nameof(instrumentationKey));
            loggerFactory.AddProvider(new LoggerProvider(filter, new Settings
            {
                InstrumentationKey = instrumentationKey,
                DeveloperMode = developerMode
            }));
            return loggerFactory;
        }
    }
}