using System;
using System.Text;

namespace Orion.Extensions.Logging.States {
    public class HttpRequestState {
        public DateTimeOffset EventTime { get; internal set; }
        public TimeSpan ResponseTime { get; internal set; }
        public string ResponseCode { get; internal set; }
        public string RequestUrl { get; internal set; }
        public string HttpMethod { get; internal set; }
        public bool Success { get; internal set; }

        public static string Formatter(HttpRequestState state, Exception exception) {
            var sb = new StringBuilder("[HTTP]");
            if (!string.IsNullOrEmpty(state.HttpMethod)) {
                sb.Append($"[{state.HttpMethod}]");
            }
            if (!string.IsNullOrEmpty(state.ResponseCode)) {
                sb.Append($"[{state.ResponseCode}]");
            }
            sb.Append($"{state.EventTime}:{state.ResponseTime} {state.RequestUrl}");
            return sb.ToString();
        }
    }
}