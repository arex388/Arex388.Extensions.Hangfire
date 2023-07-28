using Hangfire.Server;

namespace Hangfire;

/// <summary>
/// Defines a handler for a job with a payload.
/// </summary>
/// <typeparam name="TJob">The job's payload.</typeparam>
public interface IJobHandler<in TJob>
	where TJob : IJob {
	/// <summary>
	/// Handles the job.
	/// </summary>
	/// <param name="console">The handler's console instance.</param>
	/// <param name="job">The job's payload.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>Nothing.</returns>
	Task HandleAsync(
		PerformContext console,
		TJob job,
		CancellationToken cancellationToken);
}