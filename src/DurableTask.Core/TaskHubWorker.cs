//  ----------------------------------------------------------------------------------
//  Copyright Microsoft Corporation
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  http://www.apache.org/licenses/LICENSE-2.0
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//  ----------------------------------------------------------------------------------

namespace DurableTask.Core
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using DurableTask.Core.Middleware;
    using Microsoft.Extensions.Logging;

    /// <summary>
    ///     Allows users to load the TaskOrchestration and TaskActivity classes and start
    ///     dispatching to these. Also allows CRUD operations on the Task Hub itself.
    /// </summary>
    public sealed class TaskHubWorker : IDisposable
    {
        readonly INameVersionObjectManager<TaskOrchestration> orchestrationManager;

        readonly DispatchMiddlewarePipeline orchestrationDispatchPipeline = new DispatchMiddlewarePipeline();

        readonly SemaphoreSlim slimLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Reference to the orchestration service used by the task hub worker
        /// </summary>
        // ReSharper disable once InconsistentNaming (avoid breaking change)
        public IOrchestrationService orchestrationService { get; }

        volatile bool isStarted;

        TaskOrchestrationDispatcher orchestrationDispatcher;

        /// <summary>
        ///     Create a new TaskHubWorker with given OrchestrationService
        /// </summary>
        /// <param name="orchestrationService">Reference the orchestration service implementation</param>
        public TaskHubWorker(IOrchestrationService orchestrationService)
            : this(
                  orchestrationService,
                  new NameVersionObjectManager<TaskOrchestration>())
        {
        }

        /// <summary>
        ///     Create a new <see cref="TaskHubWorker"/> with given <see cref="IOrchestrationService"/> and name version managers
        /// </summary>
        /// <param name="orchestrationService">The orchestration service implementation</param>
        /// <param name="orchestrationObjectManager">The <see cref="INameVersionObjectManager{TaskOrchestration}"/> for orchestrations</param>
        public TaskHubWorker(
            IOrchestrationService orchestrationService,
            INameVersionObjectManager<TaskOrchestration> orchestrationObjectManager)
        {
            this.orchestrationManager = orchestrationObjectManager ?? throw new ArgumentException("orchestrationObjectManager");
            this.orchestrationService = orchestrationService ?? throw new ArgumentException("orchestrationService");
        }

        /// <summary>
        /// Gets the orchestration dispatcher
        /// </summary>
        public TaskOrchestrationDispatcher TaskOrchestrationDispatcher => this.orchestrationDispatcher;

        /// <summary>
        /// Adds a middleware delegate to the orchestration dispatch pipeline.
        /// </summary>
        /// <param name="middleware">Delegate to invoke whenever a message is dispatched to an orchestration.</param>
        public void AddOrchestrationDispatcherMiddleware(Func<DispatchMiddlewareContext, Func<Task>, Task> middleware)
        {
            this.orchestrationDispatchPipeline.Add(middleware ?? throw new ArgumentNullException(nameof(middleware)));
        }

        /// <summary>
        ///     Starts the TaskHubWorker so it begins processing orchestrations and activities
        /// </summary>
        /// <returns></returns>
        public async Task<TaskHubWorker> StartAsync()
        {
            await this.slimLock.WaitAsync();
            try
            {
                if (this.isStarted)
                {
                    throw new InvalidOperationException("Worker is already started");
                }

                this.orchestrationDispatcher = new TaskOrchestrationDispatcher(
                    this.orchestrationService,
                    this.orchestrationManager,
                    this.orchestrationDispatchPipeline);

                await this.orchestrationService.StartAsync();
                await this.orchestrationDispatcher.StartAsync();

                this.isStarted = true;
            }
            finally
            {
                this.slimLock.Release();
            }

            return this;
        }

        /// <summary>
        ///     Stops the TaskHubWorker
        /// </summary>
        /// <param name="isForced">True if forced shutdown, false if graceful shutdown</param>
        public async Task StopAsync(bool isForced)
        {
            await this.slimLock.WaitAsync();
            try
            {
                if (this.isStarted)
                {
                    await this.orchestrationService.StopAsync();

                    this.isStarted = false;
                }
                
            }
            finally
            {
                this.slimLock.Release();
            }
        }

        /// <summary>
        ///     Loads user defined TaskOrchestration classes in the TaskHubWorker
        /// </summary>
        /// <param name="taskOrchestrationTypes">Types deriving from TaskOrchestration class</param>
        /// <returns></returns>
        public TaskHubWorker AddTaskOrchestrations(params Type[] taskOrchestrationTypes)
        {
            foreach (Type type in taskOrchestrationTypes)
            {
                ObjectCreator<TaskOrchestration> creator = new DefaultObjectCreator<TaskOrchestration>(type);
                this.orchestrationManager.Add(creator);
            }

            return this;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            ((IDisposable)this.slimLock).Dispose();
        }
    }
}
