using Hangfire;
using System;
using System.Threading;

namespace Arex388.Extensions.Hangfire {
    /// <summary>
    /// IAsyncJob and IAsyncJob&lt;TParameters&gt; extensions.
    /// </summary>
    public static class AsyncJobExtensions {
        /// <summary>
        /// Enqueue a fire-and-forget background job immediately.
        /// </summary>
        public static string Enqueue(
            this IAsyncJob job) => BackgroundJob.Enqueue(
                () => job.HandleAsync(null, CancellationToken.None)
            );

        /// <summary>
        /// Enqueue a fire-and-forget background job immediately.
        /// </summary>
        public static string Enqueue<TParameters>(
            this IAsyncJob<TParameters> job,
            TParameters parameters) => BackgroundJob.Enqueue(
                () => job.HandleAsync(null, parameters, CancellationToken.None)
            );

        /// <summary>
        /// Recur a background job.
        /// </summary>
        public static void Recur(
            this IAsyncJob job,
            string name,
            string cron,
            TimeZoneInfo timeZone = null) => RecurringJob.AddOrUpdate(
            name,
                () => job.HandleAsync(null, CancellationToken.None),
                cron,
                timeZone
            );

        /// <summary>
        /// Recur a background job.
        /// </summary>
        public static void Recur<TParameters>(
            this IAsyncJob<TParameters> job,
            string name,
            string cron,
            TimeZoneInfo timeZone = null,
            TParameters parameters = null)
            where TParameters : class => RecurringJob.AddOrUpdate(
                () => job.HandleAsync(null, parameters, CancellationToken.None),
                cron,
                timeZone
            );

        /// <summary>
        /// Schedule a fire-and-forget background job in the future.
        /// </summary>
        public static string Schedule(
            this IAsyncJob job,
            TimeSpan delay) => BackgroundJob.Schedule(
                () => job.HandleAsync(null, CancellationToken.None),
                delay
            );

        /// <summary>
        /// Schedule a fire-and-forget background job in the future.
        /// </summary>
        public static string Schedule<TParameters>(
            this IAsyncJob<TParameters> job,
            TimeSpan delay,
            TParameters parameters)
            where TParameters : class => BackgroundJob.Schedule(
                () => job.HandleAsync(null, parameters, CancellationToken.None),
                delay
            );
    }
}
