using Hangfire.Console;
using System;

namespace Hangfire.Server {
	public static class PerformContextExtensions {
		/// <summary>
		/// Write an empty line to the console output.
		/// </summary>
		public static void Flush(
			this PerformContext console) => console.WriteLine();

		/// <summary>
		/// Write an exception to the console output.
		/// </summary>
		public static void WriteException(
			this PerformContext console,
			Exception e) => console.WriteLine($"{e.Message}\n{e.StackTrace}");
	}
}