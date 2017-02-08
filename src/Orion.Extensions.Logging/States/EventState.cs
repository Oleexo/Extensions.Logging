using System;

namespace Orion.Extensions.Logging.States {
    public class EventState {
        public DateTimeOffset EventTime { get; internal set; }

        public string Name { get; internal set; }

        public static string Formatter(EventState state, Exception exception) {
            return $"[Event][{state.EventTime}]  {state.Name}";
        }
    }
}