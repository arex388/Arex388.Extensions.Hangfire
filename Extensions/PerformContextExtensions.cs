using Hangfire.Console;
using System.Runtime.CompilerServices;
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
    public static PerformContext WriteExceptionAndFlush(
        this PerformContext console,
        Exception exception) {
        console.SetTextColor(ConsoleTextColor.Red);
        console.WriteLineAndFlush(exception.Message)
               .ResetTextColor();

        return console.WriteLineAndFlush(exception.StackTrace);
    }

    /// <summary>
    /// Write a value to the console output.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="value">The value to write.</param>
    public static PerformContext WriteLineAndFlush(
        this PerformContext console,
        string value) {
        console.WriteLine(value);
        console.WriteLine();

        return console;
    }

    /// <summary>
    /// Write an object as JSON to the console output.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="obj">The object to write.</param>
    /// <param name="label">The object's label to write.</param>
    /// <param name="jsonSerializerOptions">The JSON serialization options, if any.</param>
    public static PerformContext WriteObjectAndFlush<TObject>(
        this PerformContext console,
        TObject obj,
        [CallerArgumentExpression(nameof(obj))] string? label = "",
        JsonSerializerOptions? jsonSerializerOptions = null) {
        var json = JsonSerializer.Serialize(obj, jsonSerializerOptions ?? _jsonSerializerOptions);

        console.SetTextColor(ConsoleTextColor.Cyan);
        console.WriteLineAndFlush(label!)
               .ResetTextColor();

        return console.WriteLineAndFlush(json);
    }
}