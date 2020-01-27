using Hangfire.Server;
using System.Threading;
using System.Threading.Tasks;

namespace Arex388.Extensions.Hangfire {
	/// <summary>
	/// An asynchronous parameterized projection job.
	/// </summary>
	/// <typeparam name="TParameters">The type of parameters container.</typeparam>
	/// <typeparam name="TDataProjection">The type of data projection container.</typeparam>
	/// <typeparam name="TDataResult">The type of data result container.</typeparam>
	public interface IAsyncProjectionJob<in TParameters, out TDataProjection, out TDataResult> {
		Task HandleAsync(
			PerformContext console,
			TParameters parameters,
			CancellationToken cancellationToken);

		TDataProjection GetDataProjection(
			TParameters parameters);

		TDataResult GetDataResult(
			TParameters parameters);
	}
}