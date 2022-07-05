# Arex388.Extensions.Hangfire

> **NOTICE**
>
> - Version 1.0.7 is the last version that uses Newtonsoft.Json.
> - Version 2.0.0 replaces Newtonsoft.Json with System.Text.Json and is a breaking change. Also removes the deprecated `IAsyncProjectionJob<TDataProjection, TDataResult>` and `IAsyncProjectionJob<TParameters, TDataProjection, TDataResult>` interfaces.



Arex388.Extension.Hangfire is a small set of interfaces and extensions to make working with [Hangfire](https://github.com/HangfireIO/Hangfire) jobs easier. Originally it was made to implement the [Projection-Result pattern](https://arex388.com/blog/projection-result-pattern-improving-on-the-projection-view-pattern) I had written about, but it has since diverged and I've changed the way I implement that pattern. It is a bit opinionated and requires the [Hangfire.Console](https://github.com/pieceofsummer/Hangfire.Console) NuGet package to extend Hangfire's `PerformContext` for easier console interactions.



#### Interfaces

- `IAsyncJob` - An asynchronous job.
- `IAsyncJob<TParameters>` - An asynchronous parameterized job.



#### Extensions

- `IAsyncJob.Enqueue` - Enqueue a fire-and-forget background job immediately.

- `IAsyncJob.Recur` - Recur a background job.
- `IAsyncJob.Schedule` - Schedule a fire-and-forget background job in the future.
- `PerformConsole.Flush` - Write an empty line to the console output.
- `PerformConsole.WriteException` - Write an exception to the console output.
- `PerformConsole.WriteExceptionAndFlush` - Write an exception and an empty line to the console output.
- `PerformConsole.WriteLineAndFlush` - Write a value and an empty line to the console output.
- `PerformConsole.WriteObjectAndFlush` - Write an object as JSON to the console output.



#### How to Use

Here's an example of how I use the interfaces and extensions nowadays. The job's structure was inspired by Jimmy Bogard's Vertical Slices architecture and applies it to Hangfire's background jobs.

```c#
public sealed class DoSomething {
    private sealed class Job :
    	IAsyncJob<JobParameters> {
		private readonly DbContext _context;
            
        public Job(
        	DbContext context) => _context = context;
        
        public async Task HandleAsync(
        	PerformContext console,
        	JobParameters parameters,
        	CancellationToken cancellationToken) {
            var someObject = await _context.SomeObjects.FirstOrDefaultAsync(
            	_ => _.Id = parameters.SomeObjectId, cancellationToken).ConfigureAwait(false);
            
            console.WriteObjectAndFlush(someObject);
            
            //	Do something with someObject...
        }
    }
    
    public sealed class JobParameters {
        public int SomeObjectId { get; set; }
    }
    
    public static string Enqueue(
    	JobParameters parameters) {
        var job = new Job(null);
        
        return job.Enqueue(parameters);
    }
}

DoSomething.Enqueue(new DoSomething.JobParameters {
    SomeObjectId = 1
});
```



#### Why?

I just wanted a more clear and structured way to implement Hangfire jobs because I found myself doing it multiple different ways and it bothered me.