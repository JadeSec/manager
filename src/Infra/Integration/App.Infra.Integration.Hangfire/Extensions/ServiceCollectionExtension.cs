using System;
using Hangfire;
using System.Linq;
using System.Reflection;
using App.Infra.Bootstrap;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using App.Infra.Integration.Hangfire.Core;
using Microsoft.Extensions.DependencyInjection;
using App.Infra.Integration.Hangfire.Attributes;
using App.Infra.Integration.Hangfire.Interfaces;
using Microsoft.Extensions.Configuration;
using App.Infra.Integration.Hangfire.Models;

namespace App.Infra.Integration.Hangfire.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddHangfire(this IServiceCollection  services)
        {
            services.AddHangfire(x => x.UseMemoryStorage());                        
        }

        public static void UseHangfireAutoRegister(this IApplicationBuilder app, IConfiguration configuration)
        {
            //Dashboard
            var option = Option.Parse(configuration);
            if (option.IsValid)
            {
                app.UseHangfireDashboard(option.Path, new DashboardOptions()
                {
                    Authorization = new[] { new DashboardNoAuthorizationFilter() },
                    IgnoreAntiforgeryToken = true,
                    StatsPollingInterval = 60000
                });
                app.UseHangfireServer(new BackgroundJobServerOptions()
                {
                    SchedulePollingInterval = TimeSpan.FromMinutes(1),
                });
            }

            //Register jobs
            var jobService = new JobCore();

            var container = Ioc.RecoverContainer();

            foreach (Type mType in typeof(IJob<>).GetAssemblies())
            {
                if (mType.GetCustomAttributes().Any(x => x.GetType() == typeof(DisableAttribute)))
                    continue;

                foreach (Type hType in typeof(IJob<>).GetMakeGenericType(mType))
                {
                    container.Register(hType);

                    var jobAttr = JobAttribute.Recovery(hType);
                    if (jobAttr == null)
                        jobAttr = new JobAttribute(JobAttribute.Types.ENQUEUE,"",TimeSpan.FromSeconds(0));

                    switch (jobAttr.Type)
                    {
                        case JobAttribute.Types.ENQUEUE: jobService.Enqueue(hType); break;
                        case JobAttribute.Types.SCHEDULE: jobService.Schedule(hType); break;
                        case JobAttribute.Types.CRON: jobService.AddOrUpdate(hType); break;
                    }
                }
            }
        }  
    }
}
