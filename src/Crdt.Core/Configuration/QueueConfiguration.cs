using System.Collections.Generic;

namespace Crdt.Core.Configuration
{
    public class QueueConfiguration
    {
        public string ConsumingQueue { get; set; }
        public IEnumerable<string> SendingQueues { get; set; }
    }
}
