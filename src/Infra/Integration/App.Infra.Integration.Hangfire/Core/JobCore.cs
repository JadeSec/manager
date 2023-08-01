using App.Infra.Integration.Hangfire.Attributes;
using App.Infra.Integration.Hangfire.Modules;
using Hangfire;
using System;

namespace App.Infra.Integration.Hangfire.Core
{    
    internal class JobCore
    {
        /// <summary>
        /// Fire-and-forget jobs
        //  Fire-and-forget jobs are executed only once and almost immediately after creation.
        /// </summary>
        public string Enqueue(Type type)
        {
            var jobAttr = JobAttribute.Recovery(type);

            jobAttr.Validate();

            var jobId = BackgroundJob.Enqueue<JobModule>(job => job.RunAsync(jobAttr.Name, type, TimeSpan.FromMinutes(1)));

            return jobId;
        }                       
        
        /// <summary>
        /// Delayed jobs
        /// Delayed jobs are executed only once too, but not immediately, after a certain time interval
        /// </summary>
        public string Schedule(Type type)
        {
            var jobAttr = JobAttribute.Recovery(type);

            jobAttr.Validate();

            var jobId =  BackgroundJob.Schedule<JobModule>(job => job.RunAsync(jobAttr.Name, type, TimeSpan.FromSeconds(0)), jobAttr.Interval);
            
            return jobId;
        }

        /// <summary>
        /// Recurring jobs
        /// Recurring jobs fire many times on the specified CRON schedule.
        /// </summary>
        public void AddOrUpdate(Type type)
        {
            var jobAttr = JobAttribute.Recovery(type);

            jobAttr.Validate();

            RecurringJob.RemoveIfExists(type.FullName);
            RecurringJob.AddOrUpdate<JobModule>(type.FullName, job => job.RunAsync(jobAttr.Name, type, TimeSpan.FromSeconds(30)), jobAttr.Cron);
        }      
    }
}
