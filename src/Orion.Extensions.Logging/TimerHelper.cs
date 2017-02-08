using System;
using System.Diagnostics;

namespace Orion.Extensions.Logging {
    public sealed class TimerHelper {
        private readonly Stopwatch _stopwatch;

        public TimerHelper() {
            StartTime = DateTimeOffset.UtcNow;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Stop() {
            _stopwatch.Stop();
        }

        public DateTimeOffset StartTime { get; private set; }
        public TimeSpan Duration => _stopwatch.Elapsed;
    }
}