using Hangfire.Server;
using System.Threading;
using System.Threading.Tasks;

namespace Arex388.Extensions.Hangfire {
	/// <summary>
	/// An asynchronous job.
	/// </summary>
	public interface IAsyncJob {
		Task HandleAsync(
			PerformContext console,
			CancellationToken cancellationToken);
	}
}