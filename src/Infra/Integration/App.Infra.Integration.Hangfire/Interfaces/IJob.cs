using Hangfire;
using System.Threading.Tasks;

namespace App.Infra.Integration.Hangfire.Interfaces
{
    public interface IJob<T>
    {
        Task RunAsync(IJobCancellationToken token);       
    }
}