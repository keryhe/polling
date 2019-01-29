using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;

namespace Keryhe.Polling.Delay
{
    public class ExponentialDelay : IDelay, IDisposable
    {
        private int _wait;
        private int _minWait;
        private int _maxWait;
        private static ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private readonly ILogger<ExponentialDelay> _logger;

        public ExponentialDelay(IOptions<ExponentialOptions> options, ILogger<ExponentialDelay> logger)
        { 
            _minWait = options.Value.MinWait;
            _maxWait = options.Value.MaxWait;
            _wait = _minWait;
            _logger = logger;
        }

        public void Wait()
        {
            _logger.LogDebug("Waiting " + _wait + " seconds");
            _resetEvent.WaitOne(TimeSpan.FromSeconds(_wait));

            if (_wait < _maxWait)
            {
                _wait = _wait * 2;
            }
        }

        public void Cancel()
        {
            _logger.LogDebug("Cancelling ExponentialDelay");
            _resetEvent.Set();
        }

        public void Reset()
        {
            _logger.LogDebug("Resetting ExponentialDelay");
            _wait = _minWait;
        }

        public void Dispose()
        {
            _resetEvent.Close();
        }
    }

    public class ExponentialOptions
    {
        public int MinWait;
        public int MaxWait;
    }
}
