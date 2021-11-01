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
    using DurableTask.Core.Logging;
    using DurableTask.Core.Middleware;
    using Microsoft.Extensions.Logging;

    /// <summary>
    ///     Allows users to load the TaskOrchestration and TaskActivity classes and start
    ///     dispatching to these. Also allows CRUD operations on the Task Hub itself.
    /// </summary>
    public sealed class TaskHubWorker : IDisposable
    {
        readonly INameVersionObjectManager<TaskActivity> activityManager;
        readonly INameVersionObjectManager<TaskOrchestration> orchestrationManager;

        readonly DispatchMiddlewarePipeline orchestrationDispatchPipeline = new DispatchMiddlewarePipeline();
        readonly DispatchMiddlewarePipeline activityDispatchPipeline = new DispatchMiddlewarePipeline();

        readonly SemaphoreSlim slimLock = new SemaphoreSlim(1, 1);
        readonly LogHelper logHelper;

        /// <summary>
        /// Reference to the orchestration service used by the task hub worker
        /// </summary>
        // ReSharper disable once InconsistentNaming (avoid breaking change)
        public IOrchestrationService orchestrationService { get; }

        volatile bool isStarted;



        /// <summary>
        ///     Create a new <see cref="TaskHubWorker"/> with given <see cref="IOrchestrationService"/> and name version managers
        /// </summary>
        /// <param name="orchestrationService">The orchestration service implementation</param>
        /// <param name="orchestrationObjectManager">The <see cref="INameVersionObjectManager{TaskOrchestration}"/> for orchestrations</param>
        /// <param name="activityObjectManager">The <see cref="INameVersionObjectManager{TaskActivity}"/> for activities</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use for logging</param>
        public TaskHubWorker(
            IOrchestrationService orchestrationService,
            INameVersionObjectManager<TaskOrchestration> orchestrationObjectManager,
            INameVersionObjectManager<TaskActivity> activityObjectManager,
            ILoggerFactory loggerFactory = null)
        {
            this.orchestrationManager = orchestrationObjectManager ?? throw new ArgumentException("orchestrationObjectManager");
            this.activityManager = activityObjectManager ?? throw new ArgumentException("activityObjectManager");
            this.orchestrationService = orchestrationService ?? throw new ArgumentException("orchestrationService");
            this.logHelper = new LogHelper(loggerFactory?.CreateLogger("DurableTask.Core"));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            ((IDisposable)this.slimLock).Dispose();
        }
    }
}
