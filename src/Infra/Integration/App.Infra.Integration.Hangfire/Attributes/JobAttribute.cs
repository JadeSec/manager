using System;
using System.Reflection;

namespace App.Infra.Integration.Hangfire.Attributes
{
    [AttributeUsage(System.AttributeTargets.Class)]
    public class JobAttribute : Attribute
    {
        public string Name { get; set; }

        /// <summary>
        /// Only use : SCHEDULE
        /// </summary>
        public TimeSpan Interval { get; set; }

        /// <summary>
        /// Only use: CRON
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// Use JobAttribute.Types.[ENQUEUE, SCHEDULE, CRON]
        /// </summary>
        public Types Type { get; set; }

        public enum Types
        {
            /// <summary>
            /// Fire-and-forget jobs
            //  Fire-and-forget jobs are executed only once and almost immediately after creation.
            /// </summary>
            ENQUEUE,
            /// <summary>
            /// Delayed jobs
            /// Delayed jobs are executed only once too, but not immediately, after a certain time interval
            /// </summary>
            SCHEDULE,
            /// <summary>
            /// Recurring jobs
            /// Recurring jobs fire many times on the specified CRON schedule.
            /// </summary>
            CRON
        }

        public JobAttribute(){ }

        public JobAttribute(Types type, string cron, TimeSpan interval)
        {
            Type = type;
            Cron = cron;
            Interval = interval;
        }

        public void Validate()
        {
            if (Name == null)
                throw new ArgumentException("Job not null name.");

            if(Type != Types.ENQUEUE && Interval == null)
                throw new ArgumentException("Job not null interval.");
        }

        public static JobAttribute Recovery(Type type)
            => type.GetCustomAttribute<JobAttribute>() ?? new JobAttribute();
    }
}
