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
    using Microsoft.Extensions.Logging;

    /// <summary>
    ///     Client used to manage and query orchestration instances
    /// </summary>
    public sealed class TaskHubClient
    {
        /// <summary>
        /// The orchestration service client for this task hub client
        /// </summary>
        public IOrchestrationServiceClient ServiceClient { get; }

        /// <summary>
        ///     Create a new TaskHubClient with the given OrchestrationServiceClient
        /// </summary>
        /// <param name="serviceClient">Object implementing the <see cref="IOrchestrationServiceClient"/> interface </param>
        public TaskHubClient(IOrchestrationServiceClient serviceClient)
        {
            this.ServiceClient = serviceClient ?? throw new ArgumentNullException(nameof(serviceClient));
        }
    }
}
