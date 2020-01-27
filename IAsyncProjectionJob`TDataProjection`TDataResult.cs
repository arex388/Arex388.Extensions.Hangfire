using Hangfire.Server;
using System.Threading;
using System.Threading.Tasks;

namespace Arex388.Extensions.Hangfire {
	/// <summary>
	/// An asynchronous projection job.
	/// </summary>
	/// <typeparam name="TDataProjection">The type of data projection container.</typeparam>
	/// <typeparam name="TDataResult">The type of data result container.</typeparam>
	public interface IAsyncProjectionJob<out TDataProjection, out TDataResult> {
		Task HandleAsync(
			PerformContext console,
			CancellationToken cancellationToken);

		TDataProjection GetDataProjection();

		TDataResult GetDataResult();
	}
}