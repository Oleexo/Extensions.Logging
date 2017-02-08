using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Orion.Extensions.Logging.States;

namespace Orion.Extensions.Logging.AppInsights {
    public class Logger : ILogger {
        private readonly Func<string, LogLevel, bool> filter;
        private readonly string name;
        private readonly Settings settings;
        private readonly TelemetryClient telemetryClient;

        public Logger(string categoryName,
            Func<string, LogLevel, bool> filter,
            Settings settings) {
            this.filter = filter;
            this.settings = settings;
            name = string.IsNullOrEmpty(categoryName) ? nameof(Logger) : categoryName;
            telemetryClient = new TelemetryClient();

            if (this.settings.DeveloperMode.HasValue)
                TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = this.settings.DeveloperMode;
            if (this.settings.DeveloperMode == null || !this.settings.DeveloperMode.Value) {
                if (string.IsNullOrWhiteSpace(this.settings.InstrumentationKey))
                    throw new ArgumentNullException(nameof(this.settings.InstrumentationKey));

                TelemetryConfiguration.Active.InstrumentationKey = this.settings.InstrumentationKey;
                telemetryClient.InstrumentationKey = this.settings.InstrumentationKey;
            }
        }

        public void Log<TState>(LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter) {
            if (!IsEnabled(logLevel))
                return;

            if (exception != null) {
                telemetryClient.TrackException(new ExceptionTelemetry(exception));
                return;
            }

            if (IsSpecialState(state)) {
                TrackSpecialState(state);
                return;
            }

            var message = string.Empty;
            if (formatter != null)
                message = formatter(state, exception);
            else
                message = state?.ToString();
            if (!string.IsNullOrEmpty(message))
                telemetryClient.TrackTrace(message, GetSeverityLevel(logLevel));
        }

        public bool IsEnabled(LogLevel logLevel) {
            return filter == null || filter(name, logLevel);
        }

        public IDisposable BeginScope<TState>(TState state) {
            return LogScope.Push(telemetryClient, name, state);
        }


        private static bool IsSpecialState<TState>(TState state) {
            return ManagedStateToTelemetry.ContainsKey(typeof(TState));
        }

        private void TrackSpecialState<TState>(TState state) {
            var telemetry = ManagedStateToTelemetry[typeof(TState)](state);
            telemetryClient.Track(telemetry);
        }

        private static SeverityLevel GetSeverityLevel(LogLevel logLevel) {
            switch (logLevel) {
                case LogLevel.Critical:
                    return SeverityLevel.Critical;
                case LogLevel.Error:
                    return SeverityLevel.Error;
                case LogLevel.Warning:
                    return SeverityLevel.Warning;
                case LogLevel.Information:
                    return SeverityLevel.Information;
                case LogLevel.Trace:
                default:
                    return SeverityLevel.Verbose;
            }
        }

        #region Logger Managed States

        private static readonly IDictionary<Type, Func<object, ITelemetry>> ManagedStateToTelemetry =
            new Dictionary<Type, Func<object, ITelemetry>>();

        public static void AddStateToTelemetry<TState>(Func<TState, ITelemetry> function) where TState : class {
            ManagedStateToTelemetry.Add(typeof(TState),
                @object => {
                    var state = @object as TState;
                    return function(state);
                });
        }

        static Logger() {
            AddStateToTelemetry<HttpRequestState>(
                state => new RequestTelemetry($"{state.HttpMethod} {state.RequestUrl}",
                    state.EventTime,
                    state.ResponseTime,
                    state.ResponseCode,
                    state.Success) {
                    Url = new Uri(state.RequestUrl)
                });
            AddStateToTelemetry<SqlDependencyState>(state => new DependencyTelemetry("SQL",
                state.ServerName,
                state.ServerName,
                state.Request,
                state.EventTime,
                state.ResponseTime,
                string.Empty,
                state.Success));
            AddStateToTelemetry<MetricState>(state => new MetricTelemetry(state.Name, state.Value));
            AddStateToTelemetry<EventState>(state => new EventTelemetry(state.Name));
        }

        #endregion
    }
}