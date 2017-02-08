using System;
using System.Collections.Generic;

using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace Orion.Extensions.Logging.AppInsights
{
    public class LogOperationTelemetry : OperationTelemetry
    {
        public LogOperationTelemetry()
        {
            this.Context = new TelemetryContext();
            this.Id = Guid.NewGuid().ToString();
        }

        public override string Id { get; set; }
        public override string Name { get; set; }
        public override bool? Success { get; set; }
        public override TimeSpan Duration { get; set; }
        public override IDictionary<string, string> Properties { get; }
        public override DateTimeOffset Timestamp { get; set; }
        public override TelemetryContext Context { get; }
        public override string Sequence { get; set; }
    }
}