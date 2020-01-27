# Arex388.Extensions.Hangfire

A small set of [Hangfire][2] job interfaces for implementing the [Projection-Result pattern][3]. Implement the interfaces in your project so you can inject the appropriate services for your application. Also contains some helper extensions for jobs and Hangfire's `PerformContext` when used with [Hangfire.Console][4] NuGet package.

The extensions are a bit opinionated and require the use of Hangfire.Console.

#### Interfaces

- `IAsyncJob` - An asynchronous job.
- `IAsyncJob<TParameters>` - An asynchronous parameterized job.
- `IAsyncProjectionJob<TDataProjection, TDataResult>` - An asynchronous projection job.
- `IAsyncProjectionJob<TParameters, TDataProjection, TDataResult>` - An asynchronous parameterized projection job.

#### Extensions

- `IAsyncJob.Enqueue` - Enqueue a fire-and-forget background job immediately.
- `IAsyncJob.Recur` - Recur a background job.
- `IAsyncJob.Schedule` - Schedule a fire-and-forget background job in the future.
- `PerformConsole.Flush` - Write an empty line to the console output.
- `PerformConsole.WriteException` - Write an exception to the console output.

#### How to Use

I typically implement the interfaces as abstract classes I can inherit from. In the abstract classes I inject services such as Entity Framework and [AutoMapper][1]. I use [EntityFramework-Plus][0] for its future queries to build up my projections and then AutoMapper to map them to the final result to use within the job.

#### Why?

My goal with these extensions is to provide a clear and structured way to create Hangfire jobs and to allow for the most efficient way to query for data needed by the job while also being easy to use.

[0]:https://github.com/zzzprojects/EntityFramework-Plus
[1]: https://github.com/AutoMapper/AutoMapper
[2]: https://github.com/HangfireIO/Hangfire
[3]:https://arex388.com/blog/projection-result-pattern-improving-on-the-projection-view-pattern
[4]:https://github.com/pieceofsummer/Hangfire.Console