using System;

namespace Orion.Extensions.Logging.States {
    public class SqlDependencyState {
        public DateTimeOffset EventTime { get; internal set; }
        public string Request { get; internal set; }
        public TimeSpan ResponseTime { get; internal set; }

        public string ServerName { get; internal set; }
        public bool Success { get; internal set; }

        public static string Formatter(SqlDependencyState state, Exception exception) {
            var successString = state.Success ? "Success" : "Fail";
            return $"[Sql][{state.ServerName}][{state.EventTime}][{state.ResponseTime}][{successString}] {state.Request}";
        }
    }
}