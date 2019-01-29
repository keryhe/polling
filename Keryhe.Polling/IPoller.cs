using Keryhe.Messaging;
using System;
using System.Collections.Generic;

namespace Keryhe.Polling
{
    public interface IPoller<T> : IMessageListener<List<T>>
    {
    }

    
}
