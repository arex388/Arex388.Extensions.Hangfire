using Hangfire.Server;
using System.Threading;
using System.Threading.Tasks;

namespace Arex388.Extensions.Hangfire {
    /// <summary>
    /// An asynchronous parameterized job.
    /// </summary>
    /// <typeparam name="TParameters">The type of parameters container.</typeparam>
    public interface IAsyncJob<in TParameters> {
        /// <summary>
        /// Job handler method.
        /// </summary>
        /// <param name="console">The console instance.</param>
        /// <param name="parameters">The job's parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task</returns>
        Task HandleAsync(
            PerformContext console,
            TParameters parameters,
            CancellationToken cancellationToken);
    }
}