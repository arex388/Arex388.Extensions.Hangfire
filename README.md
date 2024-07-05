# Arex388.Extensions.Hangfire

Arex388.Extensions.Hangfire is an extension library for [Hangfire](https://github.com/HangfireIO/Hangfire) containing a small set of interfaces and extensions to make working with Hangfire easier. It is highly opinionated and attempts to match [MediatR](https://github.com/jbogard/MediatR)'s naming conventions to Hangfire. It also requires the [Hangfire.Console](https://github.com/pieceofsummer/Hangfire.Console) library for easier console interactions.



> **BREAKING CHANGES**
>
> - **v3.0.0** marks `IAsyncJob`, `IAsyncJob<TParameters>` and their extensions, `PerformContext.Flush()` and `PerformContext.WriteException()` extensions as deprecated. They will be removed in **v4.0.0**. Use `IJobHandler` and `IJobHandler<TJob>` instead.
> - **v4.0.0** removes the deprecated interfaces and extensions from **v3.0.0**.
> - **v4.1.0** marks `JobStorageExtensions.CancelScheduled()` as deprecated. They will be removed in **v4.2.0**. Use `JobStorageExtensions.CancelScheduled<THandler>()` instead.
> - **v4.2.0** removes the deprecated extensions from **v4.1.0**.



#### Interfaces

The following interfaces are provided by this library.

- `IJob` - Marker interface to represent a job payload.
- `IJobHandler` - Defines a handler for a job without a payload.
- `IJobHandler<TJob>` - Defines a handler for a job with a payload.



#### Extensions

- `IJobHandler.Enqueue` - Enqueue a fire-and-forget background job without a payload.
- `IJobHandler.Recur` - Adds a recurring background job without a payload.
- `IJobHandler.Schedule` - Schedule a fire-and-forget background job without a payload into the future.
- `IJobHandler<TJob>.Enqueue` - Enqueue a fire-and-forget background job with a payload.
- `IJobHandler<TJob>.Recur` - Adds a recurring background job with a payload.
- `IJobHandler<TJob>.Schedule` - Schedule a fire-and-forget background job with a payload into the future.
- `JobStorage.CancelScheduled` - Cancels a scheduled background job.
- `PerformContext.WriteExceptionAndFlush` - Write an exception to the console output.
- `PerformContext.WriteHeaderAndFlush` - Write a value to the console output and wrap it with `=[ {value} ]=`.
- `PerformContext.WriteLineAndFlush` - Write a value to the console output.
- `PerformContext.WriteObjectAndFlush` - Write an object as JSON to the console output.



#### How to Use

The whole idea of the library was to provide a structured way to define background jobs, based on MediatR's style. It's basically Vertical Slices for Hangfire.

```c#
public static class DoSomething {
    public sealed class Job :
    	IJob {
        public required int Id { get; init; }
    }
    
    public static void Cancel(
    	Job job) => JobStorage.Current.CancelScheduled<JobHandler>(j => ((Job)j.Value.Job.Args[1]).Id == job.Id);
    
    public static string Enqueue(
    	Job job) => new JobHandler().Enqueue(job);
    
    public static string Recur(
    	Job job) => new JobHandler().Recur("name", job, "cronExpression");
    
    public static void Schedule(
    	Job job,
    	TimeSpan delay) => new JobHandler().Schedule(job, delay);
}

file sealed class JobHandler :
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

DoSomething.Cancel(new DoSomething.Job());
DoSomething.Enqueue(new DoSomething.Job());
DoSomething.Recur(new DoSomething.Job());
DoSomething.Schedule(new DoSomething.Job(), TimeSpan.FromMinutes(5));
```
