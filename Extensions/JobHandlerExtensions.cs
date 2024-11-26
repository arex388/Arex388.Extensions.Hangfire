namespace Hangfire;

/// <summary>
/// IJobHandler and IJobHandler&lt;TJob&gt; extensions.
/// </summary>
public static class JobHandlerExtensions {
    /// <summary>
    /// Enqueue a fire-and-forget background job without a payload.
    /// </summary>
    /// <param name="handler">The job's handler.</param>
    /// <returns>The enqueued background job's id.</returns>
    public static string Enqueue(
        this IJobHandler handler) => BackgroundJob.Enqueue(() => handler.HandleAsync(null!, CancellationToken.None));

    /// <summary>
    /// Enqueue a fire-and-forget background job with a payload.
    /// </summary>
    /// <typeparam name="TJob">The job's payload type.</typeparam>
    /// <param name="handler">The job's handler.</param>
    /// <param name="job">The job's payload.</param>
    /// <returns>The enqueued background job's id.</returns>
    public static string Enqueue<TJob>(
        this IJobHandler<TJob> handler,
        TJob job)
        where TJob : IJob => BackgroundJob.Enqueue(() => handler.HandleAsync(null!, job, CancellationToken.None));

    /// <summary>
    /// Adds a recurring background job without a payload.
    /// </summary>
    /// <param name="handler">The job's handler.</param>
    /// <param name="name">The job's name.</param>
    /// <param name="cron">The cron expression.</param>
    /// <param name="timeZone">The time zone to use when enqueuing the job.</param>
    public static void Recur(
        this IJobHandler handler,
        string name,
        string cron,
        TimeZoneInfo? timeZone = null) => RecurringJob.AddOrUpdate(name, () => handler.HandleAsync(null!, CancellationToken.None), cron, timeZone);

    /// <summary>
    /// Adds a recurring background job with a payload.
    /// </summary>
    /// <typeparam name="TJob">The job's payload type.</typeparam>
    /// <param name="handler">The job's handler.</param>
    /// <param name="job">The job's payload.</param>
    /// <param name="name">the job's name.</param>
    /// <param name="cron">The cron expression.</param>
    /// <param name="timeZone">The time zone to use when enqueuing the job.</param>
    public static void Recur<TJob>(
        this IJobHandler<TJob> handler,
        TJob job,
        string name,
        string cron,
        TimeZoneInfo? timeZone = null)
        where TJob : IJob => RecurringJob.AddOrUpdate(name, () => handler.HandleAsync(null!, job, CancellationToken.None), cron, timeZone);

    /// <summary>
    /// Schedule a fire-and-forget background job without a payload into the future.
    /// </summary>
    /// <param name="handler">The job's handler.</param>
    /// <param name="delay">The time delay to occur before enqueuing the job.</param>
    /// <returns>The scheduled background job's id.</returns>
    public static string Schedule(
        this IJobHandler handler,
        TimeSpan delay) => BackgroundJob.Schedule(() => handler.HandleAsync(null!, CancellationToken.None), delay);

    /// <summary>
    /// Schedule a fire-and-forget background job with a payload into the future.
    /// </summary>
    /// <typeparam name="TJob">The job's payload type.</typeparam>
    /// <param name="handler">The job's handler.</param>
    /// <param name="job">The job's payload.</param>
    /// <param name="delay">The time delay to occur before enqueuing the job.</param>
    /// <returns>The scheduled background job's id.</returns>
    public static string Schedule<TJob>(
        this IJobHandler<TJob> handler,
        TJob job,
        TimeSpan delay)
        where TJob : IJob => BackgroundJob.Schedule(() => handler.HandleAsync(null!, job, CancellationToken.None), delay);
}