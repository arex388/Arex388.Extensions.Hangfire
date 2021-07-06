# Arex388.Extensions.Hangfire

> **NOTICE**
>
> `IAsyncProjectionJob<TDataProjection, TDataResult>` and `IAsyncProjectionJob<TParameters, TDataProjection, TDataResult>` are now deprecated. They were too opinionated about the data access.

A small set of [Hangfire][2] job interfaces for implementing the [Projection-Result pattern][3]. Implement the interfaces in your project so you can inject the appropriate services for your application. Also contains some helper extensions for jobs and Hangfire's `PerformContext` when used with [Hangfire.Console][4] NuGet package.

The extensions are a bit opinionated and require the use of Hangfire.Console.

#### Interfaces

- `IAsyncJob` - An asynchronous job.
- `IAsyncJob<TParameters>` - An asynchronous parameterized job.
- `IAsyncProjectionJob<TDataProjection, TDataResult>` - An asynchronous projection job. **DEPRECATED**
- `IAsyncProjectionJob<TParameters, TDataProjection, TDataResult>` - An asynchronous parameterized projection job. **DEPRECATED**

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

I typically implement the interfaces as abstract classes I can inherit from. In the abstract classes I inject services such as Entity Framework and [AutoMapper][1]. I use [EntityFramework-Plus][0] for its future queries to build up my projections and then AutoMapper to map them to the final result to use within the job.

Here's an example of a background job that pulls a list of customers by their status from the database, projects the results, and presumably sends them in an email. Abstract classes are included for reference.

```c#
public abstract class AsyncJob<TParameters> :
    IAsyncJob<TParameters>
    where TParameters : class {
    public abstract Task HandleAsync(
        PerformContext console,
        TParameters parameters,
        CancellationToken cancellationToken);
}

public abstract class AsyncProjectionJob<TParameters, TDataProjection, TDataResult> :
    AsyncJob<TParameters>,
    IAsyncProjectionJob<TParameters, TDataProjection, TDataResult>
    where TParameters : class
    where TDataProjection : class
    where TDataResult : class {
    protected IMapper Mapper { get; }
    protected IConfigurationProvider MapperConfig => Mapper.ConfigurationProvider;

    protected AsyncProjectionJob(
        IMapper mapper) => Mapper = mapper;

    public abstract TDataProjection GetDataProjection(
        TParameters parameters);

    public TDataResult GetDataResult(
        TParameters parameters) {
        var projection = GetDataProjection(parameters);

        return Mapper.Map<TDataResult>(projection);
    }
}

public sealed class CustomersByStatus {
    public sealed class Job :
        AsyncProjectionJob<JobParameters, JobDataProjection, JobDataResult> {
        private DbContext Context { get; set; }
        
        public Job(
            DbContext context,
            IMapper mapper) :
            base(mapper) => Context = context;
            
        public override async Task HandleAsync(
            PerformContext console,
            JobParameters parameters,
            CancellationToken cancellationToken) {
            var data = GetDataResult(parameters);
            
            if (!data.Customers.Any()) {
                console.Write("No customers were selected.");
                console.Flush();
                
                return;
            }
            
            //  Send email with customers somehow
            await ...
        }
            
        public override JobDataProjection GetDataProjection(
            JobParameters parameters) => new JobDataProjection {
            Customers = GetCustomers(parameters)
        };
            
        //  Future Queries

        private QueryFutureEnumerable<CustomerProjection> GetCustomers(
            JobParameters parameters) => Context.Customers.AsNoTracking().Where(
            c => c.IsActive == parameters.IsActive).ProjectTo<CustomerProjection>(MapperConfig).Future();
    }

    public sealed class JobParameters {
        public bool IsActive { get; set; }
    }
    
    public sealed class JobDataProjection {
        public QueryFutureEnumerable<CustomerProjection> Customers { get; set; }
    }
    
    public sealed class JobDataResult {
        public IEnumerable<CustomerProjection> Customers { get; set; }
    }
    
    //  Hangfire
    
    public static string Enqueue(
    	JobParameters parameters) {
        var job = new Job(null, null);
        
        return job.Enqueue(parameters);
    }
    
    //  Mappings
    
    public sealed class Mappings :
        AutoMapper.Profile {
        public Mappings() {
            CreateMap<Customer, CustomerProjection>();
        }
    }
    
    //	Models
    
    public sealed class CustomerProjection {
        public string Name { get; set; }
    }
}
```

You might notice that the job's structure looks similar to the vertical slices architecture, and that's because that's where most the inspiration came from. This also makes the job completely isolated from any other job in both data querying and processing.

Another benefit is that the enqueuing call is a method on the job's container class so you don't have to call into Hangfire everywhere you want to enqueue this job, you only call the job.

#### Why?

My goal with these extensions is to provide a clear and structured way to create Hangfire jobs and to allow for the most efficient way to query for data needed by the job while also being easy to use.

[0]:https://github.com/zzzprojects/EntityFramework-Plus
[1]: https://github.com/AutoMapper/AutoMapper
[2]: https://github.com/HangfireIO/Hangfire
[3]:https://arex388.com/blog/projection-result-pattern-improving-on-the-projection-view-pattern
[4]:https://github.com/pieceofsummer/Hangfire.Console