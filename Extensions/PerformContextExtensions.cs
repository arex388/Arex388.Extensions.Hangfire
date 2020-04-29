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
            Exception exception) => console.WriteLine($"{exception.Message}\n{exception.StackTrace}");

        /// <summary>
        /// Write an exception and an empty line to the console output.
        /// </summary>
        public static void WriteExceptionAndFlush(
            this PerformContext console,
            Exception exception) {
            console.WriteException(exception);
            console.Flush();
        }

        /// <summary>
        /// Write a value and an empty line to the console output.
        /// </summary>
        public static void WriteLineAndFlush(
            this PerformContext console,
            string value) {
            console.WriteLine(value);
            console.Flush();
        }
    }
}