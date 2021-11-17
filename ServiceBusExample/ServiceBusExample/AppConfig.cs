using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceBusExample
{
    public class AppConfig
    {
        public string ConnectionStringServiceBus { get; set; }
        public string QueueName { get; set; }
    }
}
