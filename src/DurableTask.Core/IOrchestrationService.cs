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
    using System.Threading;
    using System.Threading.Tasks;

    // TODO : MASTER (AFFANDAR)
    //      + implement batched message receive
    //      + proper exception model for orchestration service providers

    /// <summary>
    /// Orchestration Service interface for performing task hub management operations 
    /// and handling orchestrations and work items' state
    /// </summary>
    public interface IOrchestrationService
    {
        // Service management and lifecycle operations

        /// <summary>
        /// Starts the service initializing the required resources
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// Stops the orchestration service gracefully
        /// </summary>
        Task StopAsync();

        /// <summary>
        /// Stops the orchestration service with optional forced flag
        /// </summary>
        Task StopAsync(bool isForced);

        /// <summary>
        /// Deletes and Creates the necessary resources for the orchestration service and the instance store
        /// </summary>
        Task CreateAsync();

        /// <summary>
        /// Deletes and Creates the necessary resources for the orchestration service and optionally the instance store
        /// </summary>
        Task CreateAsync(bool recreateInstanceStore);
    }
}
