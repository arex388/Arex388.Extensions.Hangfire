using Hangfire.Console;
using System.Text.Json;

namespace Hangfire.Server;

/// <summary>
/// PerformContext extensions.
/// </summary>
public static class PerformContextExtensions {
	private static readonly JsonSerializerOptions _jsonSerializerOptions = new() {
		WriteIndented = true
	};
	
	/// <summary>
	/// Write an exception to the console output.
	/// </summary>
	/// <param name="console">The handler's console instance.</param>
	/// <param name="exception">The exception to write.</param>
	public static void WriteExceptionAndFlush(
		this PerformContext console,
		Exception exception) {
		console.WriteLine($"{exception.Message}\n{exception.StackTrace}");
		console.WriteLine();
	}

	/// <summary>
	/// Write a value to the console output.
	/// </summary>
	/// <param name="console">The handler's console instance.</param>
	/// <param name="value">The value to write.</param>
	public static void WriteLineAndFlush(
		this PerformContext console,
		string value) {
		console.WriteLine(value);
		console.WriteLine();
	}

	/// <summary>
	/// Write an object as JSON to the console output.
	/// </summary>
	/// <param name="console">The handler's console instance.</param>
	/// <param name="object">The object to write.</param>
	/// <param name="jsonSerializerOptions">The JSON serialization options, if any. Defaults to indentent JSON.</param>
	public static void WriteObjectAndFlush<TObject>(
		this PerformContext console,
		TObject @object,
		JsonSerializerOptions? jsonSerializerOptions = null) {
		var json = JsonSerializer.Serialize(@object, jsonSerializerOptions ?? _jsonSerializerOptions);

		console.WriteLineAndFlush(json);
	}
}