using Hangfire.Server;
using System.Threading;
using System.Threading.Tasks;

namespace Arex388.Extensions.Hangfire {
    /// <summary>
    /// An asynchronous job.
    /// </summary>
    public interface IAsyncJob {
        /// <summary>
        /// Job handler method.
        /// </summary>
        /// <param name="console">The console instance.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task</returns>
        Task HandleAsync(
            PerformContext console,
            CancellationToken cancellationToken);
    }
}