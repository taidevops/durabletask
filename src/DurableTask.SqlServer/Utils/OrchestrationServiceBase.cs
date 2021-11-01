// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace DurableTask.SqlServer.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DurableTask.Core;

    public abstract class OrchestrationServiceBase : IOrchestrationService, IOrchestrationServiceClient
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

        public virtual Task CreateAsync()
            => this.CreateAsync(recreateInstanceStore: false);

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
}
