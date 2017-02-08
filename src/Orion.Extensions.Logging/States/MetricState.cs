using System;

namespace Orion.Extensions.Logging.States {
    public class MetricState {
        public DateTimeOffset EventTime { get; internal set; }

        public string Name { get; internal set; }

        public double Value { get; internal set; }

        public static string Formatter(MetricState state, Exception exception) {
            return $"[Metric][{state.EventTime}]  {state.Name} => {state.Value}";
        }
    }
}