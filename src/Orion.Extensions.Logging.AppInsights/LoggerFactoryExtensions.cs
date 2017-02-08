using System;
using Microsoft.Extensions.Logging;

namespace Orion.Extensions.Logging.AppInsights {
    public static class LoggerFactoryExtensions {
        public static void AddAppInsights(this ILoggerFactory loggerFactory, string instrumentationKey,
            bool developerMode = false) {
            if (string.IsNullOrEmpty(instrumentationKey))
                throw new ArgumentException(nameof(instrumentationKey));
            loggerFactory.AddProvider(new LoggerProvider(null, new Settings {
                InstrumentationKey = instrumentationKey,
                DeveloperMode = developerMode
            }));
        }
    }
}