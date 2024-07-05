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
	/// <param name="console">The console.</param>
	/// <param name="exception">The exception to write.</param>
	public static void WriteExceptionAndFlush(
		this PerformContext console,
		Exception exception) {
		console.SetTextColor(ConsoleTextColor.Red);
		console.WriteLineAndFlush(exception.Message);
		console.WriteLineAndFlush(exception.StackTrace);
		console.ResetTextColor();
	}

	/// <summary>
	/// Write a value to the console output and wrap it with "=[ {value} ]=", and optionally specify text color.
	/// </summary>
	/// <param name="console">The console.</param>
	/// <param name="value">The value to write.</param>
	/// <param name="textColor">The text color, if different from default.</param>
	public static void WriteHeaderAndFlush(
		this PerformContext console,
		string value,
		ConsoleTextColor? textColor = null) {
		var text = $"=[ {value} ]{new string('=', 72 - value.Length)}";

		if (textColor is null) {
			console.WriteLineAndFlush(text);

			return;
		}

		console.SetTextColor(textColor);
		console.WriteLineAndFlush(text);
		console.ResetTextColor();
	}

	/// <summary>
	/// Write a value to the console output.
	/// </summary>
	/// <param name="console">The console.</param>
	/// <param name="value">The value to write.</param>
	/// <param name="textColor">The text color, if different from default.</param>
	public static void WriteLineAndFlush(
		this PerformContext console,
		string value,
		ConsoleTextColor? textColor = null) {
		if (textColor is null) {
			console.WriteLine(value);
			console.WriteLine();

			return;
		}

		console.SetTextColor(textColor);
		console.WriteLine(value);
		console.WriteLine();
		console.ResetTextColor();
	}

	/// <summary>
	/// Write an object as JSON to the console output.
	/// </summary>
	/// <param name="console">The console.</param>
	/// <param name="obj">The object to write.</param>
	/// <param name="jsonSerializerOptions">The JSON serialization options, if any.</param>
	/// <param name="textColor">The text color, if different from default.</param>
	public static void WriteObjectAndFlush<TObject>(
		this PerformContext console,
		TObject obj,
		JsonSerializerOptions? jsonSerializerOptions = null,
		ConsoleTextColor? textColor = null) {
		var json = JsonSerializer.Serialize(obj, jsonSerializerOptions ?? _jsonSerializerOptions);

		if (textColor is null) {
			console.WriteLineAndFlush(json);

			return;
		}

		console.SetTextColor(textColor);
		console.WriteLineAndFlush(json);
		console.ResetTextColor();
	}
}