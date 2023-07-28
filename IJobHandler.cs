using Hangfire.Server;

namespace Hangfire;

/// <summary>
/// Defines a handler for a job without a payload.
/// </summary>
public interface IJobHandler {
	/// <summary>
	/// Handles the job.
	/// </summary>
	/// <param name="console">The handler's console instance.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>Nothing.</returns>
	Task HandleAsync(
		PerformContext console,
		CancellationToken cancellationToken);
}