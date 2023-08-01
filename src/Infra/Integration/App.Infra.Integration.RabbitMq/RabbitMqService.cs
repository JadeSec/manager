using System;
using System.Text;
using RabbitMQ.Client;
using App.Infra.Bootstrap;
using RabbitMQ.Client.Events;
using App.Infra.Integration.RabbitMq.Core;
using App.Infra.Bootstrap.Attributes;
using App.Infra.Integration.RabbitMq.Attributes;
using App.Infra.Integration.RabbitMq.Extensions;
using System.Collections.Generic;
using App.Infra.Integration.RabbitMq.Models;
using System.Linq;

namespace App.Infra.Integration.RabbitMq
{
    [Singleton]
    public class RabbitMqService : IService<RabbitMqService>
    {       
        public readonly static List<OptionSubscribeModel> _options = new List<OptionSubscribeModel>();

        public void Publish<TMessage>(TMessage message)
        {
            using (var channel = RabbitMqCore.Connection.CreateModel())
            {
                var option = RabbitMQAttribute.Parse(typeof(TMessage));
                var body = message.Serialize().GetBytes();

                RabbitMqCore.BuildContext(channel, option);
                RabbitMqCore.DeadLetter(channel, option);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.DeliveryMode = option.DeliveryMode;                

                channel.BasicPublish(option.Exchange,
                                     option.RoutingKey,
                                     option.Mandatory,
                                     properties,
                                     body);
            }
        }

        public void Subscribe<TEvent, THandler>()
            => Subscribe(typeof(TEvent), typeof(THandler));

        internal void Subscribe(Type mType, Type hType)
        {
            var attributes = mType.GetCustomAttributes(typeof(RabbitMQAttribute), true);

            var channel = RabbitMqCore.Connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);          

            foreach (var attribute in attributes)
            {
                if (attribute is RabbitMQAttribute option)
                {
                    RabbitMqCore.BuildContext(channel, option);
                    RabbitMqCore.DeadLetter(channel, option);

                    channel.BasicQos(0, 1, false);

                    _options.Add(new OptionSubscribeModel(option.Queue, mType, hType, option));

                    channel.BasicConsume(option.Queue,
                                         autoAck: false,
                                         consumer: consumer);
                }
            }

            consumer.Received += async (model, ea) =>
            {
                var opt = _options.Where(x => x.Attribute.RoutingKey.Equals(ea.RoutingKey) &&
                                 x.Attribute.Exchange.Equals(ea.Exchange)).FirstOrDefault();
                try
                {
                    if (opt == null)
                        throw new AccessViolationException($"Not subscribed");

                    long attempts = RabbitMqCore.GetAttempts(ea.BasicProperties.Headers);
                    string body = Encoding.UTF8.GetString(ea.Body);
                     
                    if (attempts > opt.Attribute.Retry)
                        throw new AccessViolationException("Number of attempts exceeded.");          

                    await RabbitMqCore.ProcessEvent(body, opt.Event, opt.Handler, ea);

                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (AccessViolationException)
                {
                    channel.BasicReject(ea.DeliveryTag, opt.Attribute.RejectRequeue);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                    channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };
        }
    }
}