using System;
using Hangfire;
using App.Infra.Bootstrap;
using System.Threading.Tasks;
using App.Infra.Integration.Hangfire.Interfaces;

namespace App.Infra.Integration.Hangfire.Modules
{
    public class JobModule
    {
        [JobDisplayName("{0}")]
        public async Task RunAsync(string name, Type type, TimeSpan delay)
        {
            await Task.Delay(delay);

            var instance = Ioc.Get(type);

            Type concreteType = typeof(IJob<>).MakeGenericType(type);

            concreteType.GetMethod(nameof(IJob<object>.RunAsync))
                        .Invoke(instance, new object[] { new JobCancellationToken(false) });

        }
    }
}