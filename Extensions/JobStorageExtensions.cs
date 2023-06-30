using Hangfire.Storage.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace Hangfire;

/// <summary>
/// JobStorage extensions.
/// </summary>
public static class JobStorageExtensions {
	/// <summary>
	/// Cancels a scheduled background job.
	/// </summary>
	/// <param name="storage">The job storage provider. Typically the `JobStorage.Current` instance.</param>
	/// <param name="sliceName">The slice's class name.</param>
	/// <param name="predicate">The predicate to filter on the job's arguments, if any.</param>
	public static void CancelScheduled(
		this JobStorage storage,
		string sliceName,
		Func<KeyValuePair<string, ScheduledJobDto>, bool>? predicate = null) => CancelScheduled(storage, "JobHandler", sliceName, predicate);

	/// <summary>
	/// Cancels a scheduled background job.
	/// </summary>
	/// <param name="storage">The job storage provider. Typically the `JobStorage.Current` instance.</param>
	/// <param name="handlerName">The handler's class name. Typically the `JobHandler`.</param>
	/// <param name="sliceName">The slice's class name.</param>
	/// <param name="predicate">The predicate to filter on the job's arguments, if any.</param>
	public static void CancelScheduled(
		this JobStorage storage,
		string handlerName,
		string sliceName,
		Func<KeyValuePair<string, ScheduledJobDto>, bool>? predicate = null) {
		var handlers = storage.GetMonitoringApi().ScheduledJobs(0, int.MaxValue).Where(
			_ =>
				_.Value.Job.Method.Name == nameof(IJobHandler.HandleAsync)
				&& _.Value.Job.Method.DeclaringType!.Name == handlerName
				&& _.Value.Job.Method.DeclaringType.DeclaringType.Namespace == sliceName);

		if (predicate is not null) {
			handlers = handlers.Where(predicate);
		}

		foreach (var handler in handlers) {
			BackgroundJob.Delete(handler.Key);
		}
	}
}