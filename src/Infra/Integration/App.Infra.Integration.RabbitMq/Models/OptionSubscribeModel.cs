using App.Infra.Integration.RabbitMq.Attributes;
using System;

namespace App.Infra.Integration.RabbitMq.Models
{
    public class OptionSubscribeModel
    {
        public string Queue { get; set; }
        public Type Event { get; set; }
        public Type Handler { get; set; }
        public RabbitMQAttribute Attribute { get; set; }

        public OptionSubscribeModel(string queue, Type @event, Type handler, RabbitMQAttribute attr)
        {
            Queue = queue;
            Event = @event;
            Handler = handler;
            Attribute = attr;
        }
    }
}