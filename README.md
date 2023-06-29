# Arex388.Extensions.Hangfire

Arex388.Extensions.Hangfire is an extension library for [Hangfire](https://github.com/HangfireIO/Hangfire) containing a small set of interfaces and extensions to make working with Hangfire easier. It is highly opinionated and attempts to match [MediatR](https://github.com/jbogard/MediatR)'s naming conventions to Hangfire. It also requires the [Hangfire.Console](https://github.com/pieceofsummer/Hangfire.Console) library for easier console interactions.



<blockquote style="background:#e74c3c;border-left-color:#c0392b;border-radius:.5rem;color:#FFF;padding:1rem;text-shadow:0 1px 0 #c0392b">
    <b>BREAKING CHANGES</b>
    <ul>
        <li><b>v3.0.0</b> marks <span style="font-family:monospace">IAsyncJob</span>, <span style="font-family:monospace">IAsyncJob&lt;TParameters&gt;</span> and their extensions, <span style="font-family:monospace">PerformContext.Flush()</span> and <span style="font-family:monospace">PerformContext.WriteException()</span> extensions as deprecated. They will be removed in <b>v4.0.0</b>. Use <span style="font-family:monospace">IJobHandler</span> and <span style="font-family:monospace">IJobHandler&lt;TJob&gt;</span> instead.</li>
        <li><b>v4.0.0</b> removes the deprecated interfaces and extensions from <b>v3.0.0</b>.</li>
    </ul>
</blockquote>




#### Interfaces

The following interfaces are provided by this library.

- `IAsyncJob` - An asynchronous job handler. <b style="color:#e74c3c">Deprecated, use `IJobHandler` instead.</b>
- `IAsyncJob<TParameters>` - An asynchronous parameterized job handler. <b style="color:#e74c3c">Deprecated, use `IJobHandler<TJob>` instead.</b>
- `IJob` - Marker interface to represent a job payload.
- `IJobHandler` - Defines a handler for a job without a payload.
- `IJobHandler<TJob>` - Defines a handler for a job with a payload.



#### Extensions

- `IAsyncJob.Enqueue` - Enqueue a fire-and-forget background job immediately. <b style="color:#e74c3c">Deprecated.</b>

- `IAsyncJob.Recur` - Recur a background job. <b style="color:#e74c3c">Deprecated.</b>
- `IAsyncJob.Schedule` - Schedule a fire-and-forget background job in the future. <b style="color:#e74c3c">Deprecated.</b>
- `IJobHandler.Enqueue` - Enqueue a fire-and-forget background job without a payload.
- `IJobHandler.Recur` - Adds a recurring background job without a payload.
- `IJobHandler.Schedule` - Schedule a fire-and-forget background job without a payload into the future.
- `IJobHandler<TJob>.Enqueue` - Enqueue a fire-and-forget background job with a payload.
- `IJobHandler<TJob>.Recur` - Adds a recurring background job with a payload.
- `IJobHandler<TJob>.Schedule` - Schedule a fire-and-forget background job with a payload into the future.
- `PerformContext.Flush` - Write an empty line to the console output. <b style="color:#e74c3c">Deprecated, use `PerformContext.WriteLineAndFlush()` instead.</b>
- `PerformContext.WriteException` - Write an exception to the console output. <b style="color:#e74c3c">Deprecated, use `PerformContext.WriteExceptionAndFlush()` instead.</b>
- `PerformContext.WriteExceptionAndFlush` - Write an exception to the console output.
- `PerformContext.WriteLineAndFlush` - Write a value to the console output.
- `PerformContext.WriteObjectAndFlush` - Write an object as JSON to the console output.



#### How to Use

The whole idea of the library was to provide a structured way to define background jobs, based on MediatR's style. It's basically Vertical Slices for Hangfire.

```c#
public sealed class DoSomething {
    public sealed class Job :
    	IJob {
    }
    
    private sealed class JobHandler :
    	IJobHandler<Job> {
        public Task HandleAsync(
        	PerformContext console,
        	Job job,
        	CancellationToken cancellationToken) {
            //	Do something...
            
            //	Write to the console...
            console.WriteObjectAndFlush(job);
            
            //	Profit?
        }
    }
    
    public static string Enqueue(
    	Job job) => new JobHandler().Enqueue(job);
    
    public static string Recur(
    	Job job) => new JobHandler().Recur("name", job, "cronExpression");
    
    public static void Schedule(
    	Job job,
    	TimeSpan delay) => new JobHandler().Schedule(job, delay);
}

DoSomething.Enqueue(new DoSomething.Job());
DoSomething.Recur(new DoSomething.Job());
DoSomething.Schedule(new DoSomething.Job(), TimeSpan.FromMinutes(5));
```
