using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;

namespace Keryhe.Polling.Delay
{
    public class IntervalDelay : IDelay, IDisposable
    {
        private readonly int _wait;
        private static ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private readonly ILogger<IntervalDelay> _logger;

        public IntervalDelay(IOptions<IntervalOptions> options, ILogger<IntervalDelay> logger)
        {
            _wait = options.Value.Wait;
            _logger = logger;
        }

        public void Wait()
        {
            _logger.LogDebug("Waiting " + _wait + " seconds");
            _resetEvent.WaitOne(TimeSpan.FromSeconds(_wait));
        }

        public void Cancel()
        {
            _logger.LogDebug("Cancelling IntervalDelay");
            _resetEvent.Set();
        }

        public void Reset()
        {
            _logger.LogDebug("Resetting IntervalDelay");
        }

        public void Dispose()
        {
            _resetEvent.Close();
        }
    }

    public class IntervalOptions
    {
        public int Wait { get; set; }
    }
}
