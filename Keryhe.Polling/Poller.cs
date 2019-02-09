using Keryhe.Polling.Delay;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Keryhe.Polling
{
    public abstract class Poller<T> : IPoller<T>
    {
        private readonly IDelay _delay;
        private readonly ILogger<Poller<T>> _logger;
        private bool _status;

        public Poller(IDelay delay, ILogger<Poller<T>> logger)
        {
            _delay = delay;
            _logger = logger;
            _status = false;
        }

        public void Start(Action<List<T>> callback)
        {
            _status = true;
            _logger.LogDebug("Polling started");

            while (_status)
            {
                List<T> items = Poll();

                if (items.Count == 0)
                {
                    _delay.Wait();
                }
                else
                {
                    callback(items);
                    _delay.Reset();
                }
            }

            _logger.LogDebug("Polling stopped");
        }

        public void Stop()
        {
            _status = false;
            _delay.Cancel();
        }

        public virtual void Dispose()
        {
        }

        protected abstract List<T> Poll();
    }
}
