using System;
using System.Threading;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Orion.Extensions.Logging.AppInsights
{
    internal class LogScope : IDisposable {
        private static readonly AsyncLocal<LogScope> value =
            new AsyncLocal<LogScope>();

        private readonly IOperationHolder<LogOperationTelemetry> operationHolder;

        private readonly TelemetryClient telemetryClient;

        internal LogScope(TelemetryClient telemetryClient,
                                             IOperationHolder<LogOperationTelemetry> operationHolder) {
            this.telemetryClient = telemetryClient;
            this.operationHolder = operationHolder;
        }

        internal LogScope() {
            this.telemetryClient = null;
            this.operationHolder = null;
        }

        public static LogScope Current {
            set { LogScope.value.Value = value; }
            get { return value.Value; }
        }

        public LogScope Parent { get; private set; }

        public void Dispose() {
//            this.telemetryClient.StopOperation(this.operationHolder);
        }

        public static IDisposable Push(TelemetryClient telemetryClient,
                                       string name,
                                       object state) {
            //if (Current == null) {
            //    var operation = telemetryClient.StartOperation<LogOperationTelemetry>(name);
            //    Current = new LogScope(telemetryClient, operation);
            //}
            //else {
            //    var temp = Current;
            //    Current = new LogScope();
            //    Current.Parent = temp;
            //}

            //return new DisposableScope();
            return new NullDisposable();
        }

        private class NullDisposable : IDisposable {
            public void Dispose() {
            }
        }

        private class DisposableScope : IDisposable {
            public void Dispose() {
                if (Current.Parent == null) {
                    Current.Dispose();
                }
                Current = Current.Parent;
            }
        }
    }
}