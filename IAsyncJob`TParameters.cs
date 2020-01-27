using Hangfire.Server;
using System.Threading;
using System.Threading.Tasks;

namespace Arex388.Extensions.Hangfire {
	/// <summary>
	/// An asynchronous parameterized job.
	/// </summary>
	/// <typeparam name="TParameters">The type of parameters container.</typeparam>
	public interface IAsyncJob<in TParameters> {
		Task HandleAsync(
			PerformContext console,
			TParameters parameters,
			CancellationToken cancellationToken);
	}
}