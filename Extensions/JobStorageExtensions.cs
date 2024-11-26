using Hangfire.Storage.Monitoring;

namespace Hangfire;

/// <summary>
/// JobStorage extensions.
/// </summary>
public static class JobStorageExtensions {
    /// <summary>
    /// Cancels a scheduled background job.
    /// </summary>
    /// <typeparam name="THandler">The job handler's type.</typeparam>
    /// <param name="storage">The job storage provider. Typically the `JobStorage.Current` instance.</param>
    /// <param name="predicate">The predicate to filter on the job's arguments, if any.</param>
    public static void CancelScheduled<THandler>(
        this JobStorage storage,
        Func<KeyValuePair<string, ScheduledJobDto>, bool>? predicate = null) {
        var handlers = storage.GetMonitoringApi().ScheduledJobs(0, int.MaxValue).Where(
            sj =>
                sj.Value.Job.Method.Name == nameof(IJobHandler.HandleAsync)
                && sj.Value.Job.Method.DeclaringType!.FullName == typeof(THandler).FullName);

        if (predicate is not null) {
            handlers = handlers.Where(predicate);
        }

        foreach (var handler in handlers) {
            BackgroundJob.Delete(handler.Key);
        }
    }
}