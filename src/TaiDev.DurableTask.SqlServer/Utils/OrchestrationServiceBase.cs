using DurableTask.Core;

namespace TaiDev.DurableTask.SqlServer.Utils;

public abstract class OrchestrationServiceBase : IOrchestrationService
{
    CancellationTokenSource? shutdownTokenSource;

    /// <summary>
    /// Gets a <see cref="CancellationToken"/> that can be used to react to shutdown events.
    /// </summary>
    protected CancellationToken ShutdownToken => this.shutdownTokenSource?.Token ?? CancellationToken.None;

    /// <summary>
    /// Gets the number of concurrent orchestration dispatchers for fetching orchestration work items.
    /// </summary>
    public virtual int TaskOrchestrationDispatcherCount => 1;

    /// <summary>
    /// Gets the number of concurrent activity dispatchers for fetching activity work items.
    /// </summary>
    public virtual int TaskActivityDispatcherCount => 1;

    public virtual int MaxConcurrentTaskOrchestrationWorkItems
        => Environment.ProcessorCount;

    public virtual int MaxConcurrentTaskActivityWorkItems
        => Environment.ProcessorCount;

    public virtual Task CreateAsync()
        => this.CreateAsync();

    public abstract Task CreateIfNotExistsAsync();

    public abstract Task CreateAsync(bool recreateInstanceStore);

    public virtual Task StartAsync()
    {
        this.shutdownTokenSource?.Dispose();
        this.shutdownTokenSource = new CancellationTokenSource();
        return Task.CompletedTask;
    }

    public virtual Task StopAsync() => this.StopAsync(isForced: false);

    public virtual Task StopAsync(bool isForced)
    {
        this.shutdownTokenSource?.Cancel();
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync()
        => this.DeleteAsync(deleteInstanceStore: true);

    public abstract Task DeleteAsync(bool deleteInstanceStore);
}
