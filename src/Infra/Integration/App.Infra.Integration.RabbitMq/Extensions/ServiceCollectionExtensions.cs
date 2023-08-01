using System;
using SimpleInjector;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Builder;
using App.Infra.Integration.RabbitMq.Modules;
using App.Infra.Integration.RabbitMq.Factories;
using App.Infra.Integration.RabbitMq.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using App.Infra.Integration.RabbitMq.Core;
using Microsoft.Extensions.Configuration;

namespace App.Infra.Integration.RabbitMq.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="eventBusOptionAction"></param>
        /// <returns></returns>
        public static void AddRabbitMQEventBus(this Container container,
                                               IConfiguration configuration)
        {            
            var config = ConnectionConfiguration.Parse(configuration);
                       
            container.RegisterSingleton<IPersistentConnection>(() =>
            {
                IConnectionFactory factory = new ConnectionFactory
                {
                    AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled,
                    NetworkRecoveryInterval = config.NetworkRecoveryInterval,
                    UserName = config.UserName,
                    Password = config.Password,
                    Uri = new Uri(config.Host)
                };

                var connection = new PersistentConnection(config, factory);

                connection.TryConnect();

                return connection;
            });            
            
            container.RegisterSingleton<IEventHandlerModuleFactory, EventHandlerModuleFactory>();
            container.RegisterSingleton<RabbitMqCore, RabbitMqCore>();
            
            foreach (Type mType in typeof(IEvent).GetAssemblies())
            {
                container.Register(mType);
                
                foreach (Type hType in typeof(IEventHandler<>).GetMakeGenericType(mType))
                {
                    container.Register(hType);
                }
            }            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public static void RabbitMQAutoSubscribe(this IApplicationBuilder app, Container container)
        {
            RabbitMqService eventBus = container.GetRequiredService<RabbitMqService>();

            foreach (Type mType in typeof(IEvent).GetAssemblies())
            {
                foreach (Type hType in typeof(IEventHandler<>).GetMakeGenericType(mType))
                {
                    eventBus.Subscribe(mType, hType);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="moduleOptions"></param>
        public static void RabbitMQEventBusModule(this IApplicationBuilder app, Action<ModuleOption> moduleOptions)
        {
            IEventHandlerModuleFactory factory = app.ApplicationServices.GetRequiredService<IEventHandlerModuleFactory>();
            ModuleOption moduleOption = new ModuleOption(factory, app.ApplicationServices);
            moduleOptions?.Invoke(moduleOption);
        }
    }
}
