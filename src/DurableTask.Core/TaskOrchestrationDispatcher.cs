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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DurableTask.Core.History;
    using DurableTask.Core.Middleware;

    /// <summary>
    /// Dispatcher for orchestrations to handle processing and renewing, completion of orchestration events
    /// </summary>
    public class TaskOrchestrationDispatcher
    {
        readonly INameVersionObjectManager<TaskOrchestration> objectManager;
        readonly IOrchestrationService orchestrationService;

        readonly WorkItemDispatcher<TaskOrchestrationWorkItem> dispatcher;
        readonly DispatchMiddlewarePipeline dispatchPipeline;

        internal TaskOrchestrationDispatcher(
            IOrchestrationService orchestrationService,
            INameVersionObjectManager<TaskOrchestration> objectManager,
            DispatchMiddlewarePipeline dispatchPipeline)
        {
            this.objectManager = objectManager ?? throw new ArgumentNullException(nameof(objectManager));
            this.orchestrationService = orchestrationService ?? throw new ArgumentNullException(nameof(orchestrationService));
            this.dispatchPipeline = dispatchPipeline ?? throw new ArgumentNullException(nameof(dispatchPipeline));

            this.dispatcher = new WorkItemDispatcher<TaskOrchestrationWorkItem>(
                "TaskOrchestrationDispatcher",
                item => item == null ? string.Empty : item.InstanceId,

                )
            {

            };
        }

        /// <summary>
        /// Starts the dispatcher to start getting and processing orchestration events
        /// </summary>
        public async Task StartAsync()
        {
            await this.dispatcher.StartAsync();
        }

        protected Task<TaskOrchestrationWorkItem> OnFetchWorkItemAsync(TimeSpan receiveTimeout, CancellationToken cancellationToken)
        {
            return this
        }
    }
}
